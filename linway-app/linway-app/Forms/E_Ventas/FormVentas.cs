using linway_app.PresentationHelpers;
using Microsoft.Extensions.DependencyInjection;
using Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace linway_app.Forms
{
    public partial class FormVentas : Form
    {
        private List<DiaReparto> _dias = new List<DiaReparto>();
        private List<RegistroVenta> _lstRegistros = new List<RegistroVenta>();
        private List<Venta> _lstVentas = new List<Venta>();
        private readonly IServiceScope _scope;
        public FormVentas()
        {
            InitializeComponent();
            _scope = Program.LinwayServiceProvider.CreateScope();
            FormClosed += (s, e) => _scope.Dispose();
        }
        private async void FormVentas_Load(object sender, EventArgs ev)
        {
            await Actualizar();
        }
        private async Task Actualizar()
        {
            var respuesta = await UIExecutor.ExecuteAsync(
                _scope,
                async sp => {
                    var servicesContext = ServiceContext.Get(sp);
                    List<DiaReparto> dias = await servicesContext.DiaRepartoServices.GetAllAsync();
                    List<Venta> ventas = await servicesContext.VentaServices.GetAllAsync();
                    List<RegistroVenta> registros = await servicesContext.RegistroVentaServices.GetAllAsync();
                    return (dias, ventas, registros);
                },
                "No se pudieron buscar las Ventas y los Registros de Ventas",
                null
            );
            if (respuesta.dias == null || respuesta.ventas == null || respuesta.registros == null)
            {
                return;
            }
            _dias = respuesta.dias;
            _lstRegistros = respuesta.registros;
            _lstVentas = respuesta.ventas;
            ActualizarGrid1Registros(_lstRegistros);
            ActualizarGrid3Ventas();
        }
        private void LimpiarPantalla()
        {
            gbNuevaVenta.Visible = false;
            gbVerRegistro.Visible = false;
            groupBox7.Visible = false;
            label28.Text = "";
            label20.Text = "";
            textBox19.Text = "";
            textBox3.Text = "";
            checkBox2.Checked = false;
            cbSeguro.Checked = false;
            textBox12.Text = "";
            textBox13.Text = "";
            textBox1.Text = "";
            labelFecha.Text = "";
            labelTotal.Text = "";
            cbSeguroBorrar.Checked = false;
            gbBorrarReg.Visible = false;
            tbDesde.Text = "";
            tbHasta.Text = "";
            checkBox1.Checked = false;
            checkBox3EliminarRegistroRestarDeVentas.Checked = false;
        }
        private void CancelarClick_Click(object sender, EventArgs ev)
        {
            LimpiarPantalla();
        }
        private void SoloNumero_KeyPress(object sender, KeyPressEventArgs ev)
        {
            if (!char.IsNumber(ev.KeyChar) && ev.KeyChar != (char)Keys.Back)
            {
                ev.Handled = true;
            }
        }
        private void SoloNumeroYNegativo_KeyPress(object sender, KeyPressEventArgs ev)
        {
            if (!char.IsControl(ev.KeyChar) && !char.IsDigit(ev.KeyChar) && ev.KeyChar != '-')
            {
                ev.Handled = true;
            }
            if (ev.KeyChar == '-' && (sender as TextBox).Text.Length > 0)
            {
                ev.Handled = true;
            }
        }
        private void NuevaVenta_ToolStripMenuItem_Click(object sender, EventArgs ev)
        {
            LimpiarPantalla();
            gbNuevaVenta.Visible = true;
            _lstAgregarVentas.Clear();
            ActualizarGrid5(_lstAgregarVentas);
        }
        private void ReiniciarVentas_ToolStripMenuItem_Click(object sender, EventArgs ev)
        {
            LimpiarPantalla();
            groupBox7.Visible = true;
        }
        private void VerRegistro_ToolStripMenuItem_Click(object sender, EventArgs ev)
        {
            LimpiarPantalla();
            gbVerRegistro.Visible = true;
            ActualizarGrid2ProdVendidos(new List<ProdVendido>());
        }
        private void BorrarRegistros_ToolStripMenuItem_Click(object sender, EventArgs ev)
        {
            LimpiarPantalla();
            gbBorrarReg.Visible = true;
        }
        private async void ExportBtn_Click_1(object sender, EventArgs ev)
        {
            await Actualizar();
            bool logrado = await UIExecutor.ExecuteAsync(
                _scope,
                async sp =>
                {
                    var servicesContext = ServiceContext.Get(sp);
                    servicesContext.ExportarServices.ExportarVentas(_lstVentas);
                    return true;
                },
                "No se pudieron buscar las Ventas y los Registros de Ventas",
                null
            );
            if (!logrado)
            {
                return;
            }
            ExportBtn.Text = "Terminado";
        }
    }
}
