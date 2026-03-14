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
        private Cliente _cliente;
        private Producto _producto;
        private Reparto _reparto;
        private Pedido _pedido;
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
            if (_cliente == null || _lstProdVendidosAAgregar == null || _lstProdVendidosAAgregar.Count == 0)
            {
                MessageBox.Show("Verifique los campos");
                return;
            }
            bool agregarProdVendidosARegistrosYVentas = checkBox4.Checked;
            bool enviarAHojaDeReparto = checkBox3.Checked;
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
                    NotaDeEnvio nuevaNota = new NotaDeEnvio
                    {
                        ClienteId = _cliente.Id,
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
                            ClienteId = _cliente.Id,
                            Fecha = DateTime.Now.ToString(Constants.FormatoDeFecha),
                            NombreCliente = _cliente.Direccion
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
                        bool existiaPedido = _pedido != null;
                        if (!existiaPedido)
                        {
                            if (_reparto == null)
                            {
                                savingServices.DiscardChanges();
                                MessageBox.Show("Falta el Reparto");
                                return null;
                            }
                            _pedido = PedidoServices.CrearPedido(_cliente, _reparto);
                        }
                        foreach (ProdVendido prodVendido in _lstProdVendidosAAgregar)
                        {
                            prodVendido.Pedido = _pedido;
                        }
                        // pedido.Entregar = 1;
                        PedidoServices.ActualizarCantidadesYDescripcionDePedido(_pedido, true);
                        RepartoServices.ActualizarCantidadesDeReparto(_pedido.Reparto);
                        if (existiaPedido)
                        {
                            pedidoServices.EditPedido(_pedido);
                        }
                        else
                        {
                            await pedidoServices.AddPedidoAsync(_pedido);
                        }
                        repartoServices.EditReparto(_pedido.Reparto);
                    }
                    prodVendidoServices.AddProdVendidos(_lstProdVendidosAAgregar);
                    nuevaNota.Cliente = _cliente;  // para que no falle la dirección al imprimir
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
