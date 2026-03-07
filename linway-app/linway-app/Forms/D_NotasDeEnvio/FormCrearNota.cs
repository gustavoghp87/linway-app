using linway_app.PresentationHelpers;
using linway_app.Services.FormServices;
using linway_app.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace linway_app.Forms
{
    public partial class FormCrearNota : Form
    {
        private readonly List<ProdVendido> _lstProdVendidosAAgregar = new List<ProdVendido>();
        private readonly List<Producto> _lstProductosAAgregar = new List<Producto>();
        private readonly IServiceScope _scope;
        public FormCrearNota()
        {
            InitializeComponent();
            _scope = Program.LinwayServiceProvider.CreateScope();
            FormClosed += (s, e) => _scope.Dispose();
        }
        private void FormCrearNota_Load(object sender, EventArgs ev)
        {
            ActualizarGrid();
        }
        private void SoloNumero_KeyPress(object sender, KeyPressEventArgs ev)
        {
            if (!char.IsNumber(ev.KeyChar) && ev.KeyChar != (char)Keys.Back)
            {
                ev.Handled = true;
            }
        }
        private async void ConfirmarCrearNota_Click(object sender, EventArgs ev)
        {
            if (labelClienteId.Text == "" || labelClienteId.Text == "No encontrado" || _lstProdVendidosAAgregar == null || _lstProdVendidosAAgregar.Count == 0)
            {
                MessageBox.Show("Verifique los campos");
                return;
            }
            string direccion = label36.Text;
            bool agregarProdVendidosARegistrosYVentas = checkBox4.Checked;
            bool enviarAHojaDeReparto = checkBox3.Checked;
            string diaReparto = comboBox4.Text;
            string nombreReparto = comboBox3.Text;
            NotaDeEnvio nuevaNota = await UIExecutor.ExecuteAsync(
                _scope,
                async sp => {
                    var savingServices = sp.GetRequiredService<ISavingServices>();
                    var clienteServices = sp.GetRequiredService<IClienteServices>();
                    var diaRepartoServices = sp.GetRequiredService<IDiaRepartoServices>();
                    var notaDeEnvioServices = sp.GetRequiredService<INotaDeEnvioServices>();
                    var pedidoServices = sp.GetRequiredService<IPedidoServices>();
                    var prodVendidoServices = sp.GetRequiredService<IProdVendidoServices>();
                    var repartoServices = sp.GetRequiredService<IRepartoServices>();
                    var registroVentaServices = sp.GetRequiredService<IRegistroVentaServices>();
                    var ventaServices = sp.GetRequiredService<IVentaServices>();
                    Cliente cliente = await clienteServices.GetClientePorDireccionExactaAsync(direccion);
                    NotaDeEnvio nuevaNota = new NotaDeEnvio
                    {
                        ClienteId = cliente.Id,
                        Fecha = DateTime.Now.ToString(Constants.FormatoDeFecha),
                        Impresa = 0,
                        Detalle = NotaDeEnvioServices.ExtraerDetalleDeNotaDeEnvio(_lstProdVendidosAAgregar),
                        ImporteTotal = NotaDeEnvioServices.ExtraerImporteDeNotaDeEnvio(_lstProdVendidosAAgregar)
                    };
                    notaDeEnvioServices.AddNotaDeEnvio(nuevaNota);
                    foreach (ProdVendido prodVendido in _lstProdVendidosAAgregar)
                    {
                        prodVendido.NotaDeEnvio = nuevaNota;
                    }
                    if (agregarProdVendidosARegistrosYVentas)
                    {
                        RegistroVenta nuevoRegistro = new RegistroVenta
                        {
                            ClienteId = cliente.Id,
                            Fecha = DateTime.Now.ToString(Constants.FormatoDeFecha),
                            NombreCliente = cliente.Direccion
                        };
                        registroVentaServices.AddRegistroVenta(nuevoRegistro);
                        foreach (var prodVendido in _lstProdVendidosAAgregar)
                        {
                            prodVendido.RegistroVenta = nuevoRegistro;
                            _lstProdVendidosAAgregar.Find(x => x.Id == prodVendido.Id).RegistroVenta = nuevoRegistro;  // para que el siguiente checkbox no pise los cambios
                        }
                        await ventaServices.UpdateVentasDesdeProdVendidosAsync(_lstProdVendidosAAgregar, true);
                    }
                    if (enviarAHojaDeReparto)
                    {
                        List<DiaReparto> lstDiasRep = await diaRepartoServices.GetDiaRepartosAsync();
                        Reparto reparto = lstDiasRep
                            .Find(x => x.Dia == diaReparto && x.Estado != null && x.Estado != "Eliminado").Reparto.ToList()
                            .Find(x => x.Nombre == nombreReparto && x.Estado != null && x.Estado != "Eliminado");

                        //Pedido pedido = await orquestacionServices.GetPedidoPorRepartoYClienteGenerarSiNoExisteAsync(reparto.Id, cliente.Id);
                        Pedido pedido = reparto.Pedidos.ToList().Find(x => x.ClienteId == cliente.Id && x.Estado != "Eliminado");
                        bool existiaPedido = pedido != null;
                        if (!existiaPedido)
                        {
                            pedido = new Pedido()
                            {
                                Cliente = cliente,
                                Direccion = cliente.Direccion,
                                Reparto = reparto,
                                Entregar = 1,
                                Estado = "Activo",
                                ProductosText = "",
                                L = 0,
                                A = 0,
                                Ae = 0,
                                D = 0,
                                E = 0,
                                T = 0
                            };
                        }
                        foreach (ProdVendido prodVendido in _lstProdVendidosAAgregar)
                        {
                            prodVendido.Pedido = pedido;
                        }
                        // pedido.Entregar = 1;
                        PedidoServices.ActualizarEtiquetasDePedido(pedido, true);
                        RepartoServices.ActualizarEtiquetasDeReparto(pedido.Reparto);
                        if (existiaPedido)
                        {
                            pedidoServices.EditPedido(pedido);
                        }
                        else
                        {
                            await pedidoServices.AddPedidoAsync(pedido);
                        }
                        repartoServices.EditReparto(pedido.Reparto);
                    }
                    prodVendidoServices.AddProdVendidos(_lstProdVendidosAAgregar);
                    nuevaNota.Cliente = cliente;  // para que no falle la dirección al imprimir
                    foreach (ProdVendido prodVendido in _lstProdVendidosAAgregar)
                    {
                        prodVendido.Producto = _lstProductosAAgregar.Find(p => p.Id == prodVendido.ProductoId);  // para que no falten los productos al imprimir
                    }
                    nuevaNota.ProdVendidos = _lstProdVendidosAAgregar;
                    bool guardado = await savingServices.SaveAsync();
                    if (!guardado)
                    {
                        savingServices.DiscardChanges();
                        MessageBox.Show("No se hicieron cambios");
                        return null;
                    }
                    return nuevaNota;
                },
                "No se pudo realizar",
                this
            );
            if (nuevaNota == null)
            {
                return;
            }
            if (checkBox1.Checked)  // imprimir
            {
                var form = Program.LinwayServiceProvider.GetRequiredService<FormImprimirNota>();
                form.Rellenar_Datos(nuevaNota);
                form.Show(this);
            }
            Close();
        }
        private void CerrarBtn_Click(object sender, EventArgs ev)
        {
            Close();
        }
    }
}
