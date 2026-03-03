using linway_app.PresentationHelpers;
using linway_app.Services.Excel;
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
        private List<Venta> _lstVentas;
        private List<RegistroVenta> _lstRegistros;
        private readonly List<Venta> _lstAgregarVentas;
        private string showing = "agregarReg";
        private readonly IServiceScope _scope;
        public FormVentas()
        {
            InitializeComponent();
            _scope = Program.LinwayServiceProvider.CreateScope();
            FormClosed += (s, e) => _scope.Dispose();
            //
            _lstVentas = new List<Venta>();
            _lstRegistros = new List<RegistroVenta>();
            _lstAgregarVentas = new List<Venta>();
        }
        private async void FormVentas_Load(object sender, EventArgs ev)
        {
            await Actualizar();
        }
        private async Task Actualizar()
        {
            await UIExecutor.ExecuteAsync(
                _scope,
                async sp => {
                    var ventaServices = sp.GetRequiredService<IVentaServices>();
                    var registroVentaServices = sp.GetRequiredService<IRegistroVentaServices>();
                    _lstVentas = await ventaServices.GetVentasAsync();
                    _lstRegistros = await registroVentaServices.GetRegistroVentasAsync();
                    return true;
                },
                "No se pudieron buscar las Ventas y los Registros de Ventas",
                null
            );
            ActualizarGrid1Registros(_lstRegistros);
            ActualizarGrid3Ventas();
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
        private async void ExportBtn_Click_1(object sender, EventArgs ev)
        {
            await Actualizar();
            bool logrado = await UIExecutor.ExecuteAsync(
                _scope,
                sp =>
                {
                    var exportarServices = sp.GetRequiredService<IExportarServices>();
                    exportarServices.ExportarVentas(_lstVentas);
                    return Task.FromResult(true);
                },
                "No se pudieron buscar las Ventas y los Registros de Ventas",
                null
            );
            if (!logrado)
            {
                MessageBox.Show("No se pudieron exportar Ventas");
                return;
            }
            ExportBtn.Text = "Terminado";
        }
    }
}
