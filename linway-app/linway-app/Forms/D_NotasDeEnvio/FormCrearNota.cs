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
        private List<DiaReparto> _lstDias = new List<DiaReparto>();
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
        private async void FormCrearNota_Load(object sender, EventArgs ev)
        {
            ActualizarGrid();
            List<DiaReparto> diaRepartos = await UIExecutor.ExecuteAsync(
                _scope,
                async sp =>
                {
                    var diaRepartoServices = sp.GetRequiredService<IDiaRepartoServices>();
                    List<DiaReparto> lstDiasRep = await diaRepartoServices.GetAllAsync();
                    return lstDiasRep;
                },
                "No se pudieron buscar los Días de Reparto",
                null
            );
            if (diaRepartos == null)
            {
                return;
            }
            _lstDias = diaRepartos;
        }
        private void SoloNumero_KeyPress(object sender, KeyPressEventArgs ev)
        {
            if (!char.IsNumber(ev.KeyChar) && ev.KeyChar != (char)Keys.Back)
            {
                ev.Handled = true;
            }
        }
        private void EstablecerReparto()
        {
            if (_cliente == null || _reparto == null)
            {
                _pedido = null;
                return;
            }
            _pedido = _reparto.Pedidos.ToList().FirstOrDefault(x => x.ClienteId == _cliente.Id);
        }
        private async void ConfirmarCrearNota_Click(object sender, EventArgs ev)
        {
            if (_cliente == null || _lstProdVendidosAAgregar.Count == 0)
            {
                MessageBox.Show("Verifique los campos");
                return;
            }
            bool agregarProdVendidosARegistrosYVentas = checkBox4.Checked;
            bool enviarAHojaDeReparto = checkBox3EnviarA_HDR.Checked;
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
                    var notaDeEnvioServices = sp.GetRequiredService<INotaDeEnvioServices>();
                    var pedidoServices = sp.GetRequiredService<IPedidoServices>();
                    var prodVendidoServices = sp.GetRequiredService<IProdVendidoServices>();
                    var repartoServices = sp.GetRequiredService<IRepartoServices>();
                    var registroVentaServices = sp.GetRequiredService<IRegistroVentaServices>();
                    var ventaServices = sp.GetRequiredService<IVentaServices>();
                    NotaDeEnvio nuevaNota = new NotaDeEnvio
                    {
                        Cliente = _cliente,
                        ClienteId = _cliente.Id,
                        Detalle = NotaDeEnvioServices.ExtraerDetalleDeNotaDeEnvio(_lstProdVendidosAAgregar),
                        Fecha = DateTime.Now.ToString(Constants.FormatoDeFecha),
                        ImporteTotal = NotaDeEnvioServices.ExtraerImporteDeNotaDeEnvio(_lstProdVendidosAAgregar),
                        Impresa = 0,
                        //ProdVendidos = _lstProdVendidosAAgregar
                    };
                    notaDeEnvioServices.Add(nuevaNota);
                    foreach (ProdVendido prodVendido in _lstProdVendidosAAgregar)
                    {
                        prodVendido.NotaDeEnvio = nuevaNota;
                        prodVendido.NotaDeEnvioId = nuevaNota.Id;
                    }
                    if (agregarProdVendidosARegistrosYVentas)
                    {
                        var nuevoRegistro = new RegistroVenta
                        {
                            ClienteId = _cliente.Id,
                            Fecha = DateTime.Now.ToString(Constants.FormatoDeFecha),
                            NombreCliente = _cliente.Direccion
                        };
                        registroVentaServices.Add(nuevoRegistro);
                        foreach (var prodVendido in _lstProdVendidosAAgregar)
                        {
                            prodVendido.RegistroVenta = nuevoRegistro;
                            prodVendido.RegistroVentaId = nuevoRegistro.Id;
                            //_lstProdVendidosAAgregar.Find(x => x.Id == prodVendido.Id).RegistroVenta = nuevoRegistro;  // para que el siguiente checkbox no pise los cambios
                        }
                        await ventaServices.UpdateDesdeProdVendidosAsync(_lstProdVendidosAAgregar, true);
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
                            pedidoServices.Edit(_pedido);
                        }
                        else
                        {
                            await pedidoServices.AddAsync(_pedido);
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
                        repartoServices.Edit(_pedido.Reparto);
                    }
                    //
                    prodVendidoServices.AddMany(_lstProdVendidosAAgregar);
                    //
                    if (imprimir)
                    {
                        foreach (ProdVendido prodVendido in _lstProdVendidosAAgregar)
                        {
                            prodVendido.Producto = _lstProductosAAgregar.Find(p => p.Id == prodVendido.ProductoId);
                        }
                    }
                    //
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
            if (imprimir)  // imprimir
            {
                var form = Program.LinwayServiceProvider.GetRequiredService<FormImprimirNota>();
                form.Rellenar_Datos(nuevaNota);
                form.Show(this);  // no necesita renovar scope porque el cambio a imprimido no se muestra en este form
            }
            _cliente = null;
            _pedido = null;
            _reparto = null;
            textBox15ClienteNumeroBusqueda.Text = "";
            textBox1ClienteDireccionBusqueda.Text = "";
            label36ClienteDireccion.Text = "";
            labelClienteId.Text = "";
            label33Dia.Visible = false;
            label34Nombre.Visible = false;
            comboBox3NombreDeReparto.Text = "";
            comboBox3NombreDeReparto.Visible = false;
            comboBox4DiaDeReparto.Visible = false;
            checkBox4.Checked = false;
            checkBox3EnviarA_HDR.Checked = false;
            checkBox1Imprimir.Checked = false;
            LimpiarLista();
        }
        private void CerrarBtn_Click(object sender, EventArgs ev)
        {
            Close();
        }
    }
}
