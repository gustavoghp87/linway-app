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
    public partial class FormNotasEnvio : Form
    {
        private List<DiaReparto> _lstDiaRepartos = new List<DiaReparto>();
        private List<NotaDeEnvio> _lstNotaDeEnvios = new List<NotaDeEnvio>();
        private List<ProdVendido> _lstProdVendidos = new List<ProdVendido>();
        private IServiceScope _scope;
        public FormNotasEnvio()
        {
            InitializeComponent();
            _scope = Program.LinwayServiceProvider.CreateScope();
            FormClosed += (s, e) => _scope.Dispose();
        }
        private async void FormNotas_Load(object sender, EventArgs ev)
        {
            await ActualizarNotas();
            ActualizarGrid1(_lstNotaDeEnvios);
        }
        private async Task ActualizarNotas()
        {
            var respuesta = await UIExecutor.ExecuteAsync(
                _scope,
                async sp =>
                {
                    var diaRepartoServices = sp.GetRequiredService<IDiaRepartoServices>();
                    var notaDeEnvioServices = sp.GetRequiredService<INotaDeEnvioServices>();
                    List<NotaDeEnvio> notas = await notaDeEnvioServices.GetAllAsync();
                    List<DiaReparto> dias = await diaRepartoServices.GetAllAsync();
                    return (notas, dias);
                },
                "No se pudieron buscar las Notas de Envío o los Repartos por Día",
                null
            );
            if (respuesta.notas == null || respuesta.dias == null)
            {
                return;
            }
            _lstNotaDeEnvios = respuesta.notas;
            _lstDiaRepartos = respuesta.dias;
            labelCantidadDeNotas.Text = _lstNotaDeEnvios.Count.ToString() + " notas de envio.";
        }
        private async void ExportarAExcel_Btn_Click(object sender, EventArgs ev)
        {
            bool logrado = await UIExecutor.ExecuteAsync(
                _scope,
                async sp => {
                    var exportarServices = sp.GetRequiredService<IExportarServices>();
                    var notaDeEnvioServices = sp.GetRequiredService<INotaDeEnvioServices>();
                    List<NotaDeEnvio> notas = await notaDeEnvioServices.GetAllAsync();
                    exportarServices.ExportarNotas(notas);
                    return true;
                },
                "Algo falló",
                this
            );
            if (!logrado)
            {
                return;
            }
            button2.Text = "TERMINADO";
        }
    }
}
