using linway_app.PresentationHelpers;
using linway_app.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Models;
using Models.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace linway_app.Forms
{
    public partial class FormNotasEnvio : Form
    {
        private List<NotaDeEnvio> _lstNotaDeEnvios = new List<NotaDeEnvio>();
        private List<ProdVendido> _lstProdVendidos = new List<ProdVendido>();
        private readonly IServiceScope _scope;
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
            var notas = await UIExecutor.ExecuteAsync(
                _scope,
                async sp => {
                    var notaDeEnvioServices = sp.GetRequiredService<INotaDeEnvioServices>();
                    return await notaDeEnvioServices.GetNotaDeEnviosAsync();
                },
                "No se pudieron buscar las Notas de Envío",
                null
            );
            if (notas == null)
            {
                return;
            }
            _lstNotaDeEnvios = notas;
            lCantNotas.Text = _lstNotaDeEnvios.Count.ToString() + " notas de envio.";
        }
        private void ActualizarGrid2(ICollection<ProdVendido> lstProdVendidos)
        {
            if (lstProdVendidos == null)
            {
                return;
            }
            var grid = new List<EProdVendido>();
            foreach (ProdVendido prodVendido in lstProdVendidos)
            {
                grid.Add(Form1.Mapper.Map<EProdVendido>(prodVendido));
            }
            dataGridView2.DataSource = grid;
            dataGridView2.Columns[0].Width = 25;
            dataGridView2.Columns[1].Width = 200;
        }
        private async void ExportarAExcel_Btn_Click(object sender, EventArgs ev)
        {
            bool logrado = await UIExecutor.ExecuteAsync(
                _scope,
                async sp => {
                    var savingServices = sp.GetRequiredService<ISavingServices>();
                    var exportarServices = sp.GetRequiredService<IExportarServices>();
                    var notaDeEnvioServices = sp.GetRequiredService<INotaDeEnvioServices>();
                    ICollection<NotaDeEnvio> notas = await notaDeEnvioServices.GetNotaDeEnviosAsync();
                    exportarServices.ExportarNotas(notas);
                    return await savingServices.SaveAsync();
                },
                "Algo falló",
                this
            );
            if (!logrado)
            {
                MessageBox.Show("No se pudieron exportar Notas");
                return;
            }
            button2.Text = "TERMINADO";
        }
    }
}
