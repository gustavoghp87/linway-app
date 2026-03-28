using linway_app.PresentationHelpers;
using linway_app.Services.Interfaces;
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
        private List<Venta> _lstVentas = new List<Venta>();
        private List<RegistroVenta> _lstRegistros = new List<RegistroVenta>();
        private string showing = "agregarReg";
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
            var response = await UIExecutor.ExecuteAsync(
                _scope,
                async sp => {
                    var ventaServices = sp.GetRequiredService<IVentaServices>();
                    var registroVentaServices = sp.GetRequiredService<IRegistroVentaServices>();
                    List<Venta> ventas = await ventaServices.GetAllAsync();
                    List<RegistroVenta> registros = await registroVentaServices.GetAllAsync();
                    return (ventas, registros);
                },
                "No se pudieron buscar las Ventas y los Registros de Ventas",
                null
            );
            if (response.ventas == null || response.registros == null)
            {
                return;
            }
            _lstVentas = response.ventas;
            _lstRegistros = response.registros;
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
        private void VerRegistro_ToolStripMenuItem_Click(object sender, EventArgs ev)
        {
            LimpiarPantalla();
            showing = "verReg";
            gbVerRegistro.Visible = true;
            ActualizarGrid2ProdVendidos(new List<ProdVendido>());
        }
        private void NuevaVenta_ToolStripMenuItem_Click(object sender, EventArgs ev)
        {
            LimpiarPantalla();
            showing = "agregarReg";
            gbNuevaVenta.Visible = true;
            _lstAgregarVentas.Clear();
            ActualizarGrid5(_lstAgregarVentas);
        }
        private void ReiniciarVentas_ToolStripMenuItem_Click(object sender, EventArgs ev)
        {
            LimpiarPantalla();
            groupBox7.Visible = true;
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
                    var exportarServices = sp.GetRequiredService<IExportarServices>();
                    exportarServices.ExportarVentas(_lstVentas);
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
