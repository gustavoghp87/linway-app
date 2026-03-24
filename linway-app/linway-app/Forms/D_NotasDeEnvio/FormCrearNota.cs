using linway_app.PresentationHelpers;
using linway_app.Services.FormServices;
using linway_app.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Models;
using Models.Entities;
using System;
using System.Collections.Generic;
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
            bool imprimir = checkBox1Imprimir.Checked;
            if (enviarAHojaDeReparto && _reparto == null)
            {
                MessageBox.Show("Falta el Reparto");
                return;
            }
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
                        var nuevoRegistro = new RegistroVenta
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
                            _pedido = PedidoServices.GetNuevoPedido(_cliente, _reparto);
                        }
                        foreach (ProdVendido prodVendido in _lstProdVendidosAAgregar)
                        {
                            prodVendido.Pedido = _pedido;
                            _pedido.ProdVendidos.Add(prodVendido);
                        }
                        PedidoServices.ActualizarCantidadesYDescripcionDePedido(_pedido, true);  // pedido.Entregar = 1;
                        if (existiaPedido)
                        {
                            pedidoServices.EditPedido(_pedido);
                        }
                        else
                        {
                            await pedidoServices.AddPedidoAsync(_pedido);
                        }
                        // reparto
                        foreach (var p in _reparto.Pedidos)
                        {
                            if (p.Id == _pedido.Id)
                            {
                                p.ProdVendidos = _pedido.ProdVendidos;
                            }
                        }
                        RepartoServices.ActualizarCantidadesDeReparto(_pedido.Reparto);
                        repartoServices.EditReparto(_pedido.Reparto);
                    }
                    prodVendidoServices.AddProdVendidos(_lstProdVendidosAAgregar);
                    if (imprimir)  // imprimir
                    {
                        nuevaNota.Cliente = _cliente;
                        foreach (ProdVendido prodVendido in _lstProdVendidosAAgregar)
                        {
                            prodVendido.Producto = _lstProductosAAgregar.Find(p => p.Id == prodVendido.ProductoId);
                        }
                        nuevaNota.ProdVendidos = _lstProdVendidosAAgregar;
                    }
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
            _pedido = null;
            _reparto = null;
            if (imprimir)  // imprimir
            {
                var form = Program.LinwayServiceProvider.GetRequiredService<FormImprimirNota>();
                form.Rellenar_Datos(nuevaNota);
                form.Show(this);  // no necesita renovar scope porque el cambio a imprimido no se muestra en este form
            }
            Close();
        }
        private void CerrarBtn_Click(object sender, EventArgs ev)
        {
            Close();
        }
    }
}
