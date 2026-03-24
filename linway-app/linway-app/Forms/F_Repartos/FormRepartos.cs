using linway_app.PresentationHelpers;
using linway_app.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace linway_app.Forms
{
    public partial class FormRepartos : Form
    {
        private List<Pedido> _lstPedidos = new List<Pedido>();
        private List<DiaReparto> _lstDiaRepartos = new List<DiaReparto>();
        private readonly IServiceScope _scope;
        public FormRepartos()
        {
            InitializeComponent();
            _scope = Program.LinwayServiceProvider.CreateScope();
            FormClosed += (s, e) => _scope.Dispose();
        }
        private async void FormReparto_Load(object sender, EventArgs ev)
        {
            await Actualizar();
            checkBox1.Checked = true;
            dataGridView1.CellClick += new DataGridViewCellEventHandler(DataGridView1_CellClick);
        }
        private async Task Actualizar()
        {
            List<DiaReparto> diaRepartos = await UIExecutor.ExecuteAsync(
                _scope,
                async sp => {
                    var diaRepartoServices = sp.GetRequiredService<IDiaRepartoServices>();
                    return await diaRepartoServices.GetDiaRepartosAsync();
                },
                "No se pudieron buscar los Días de Reparto",
                null
            );
            if (diaRepartos == null || diaRepartos.Count == 0)
            {
                await CrearDias();
                return;
            }
            _lstDiaRepartos = diaRepartos;
            await UpdateGrid();
        }
        private async Task CrearDias()
        {
            List<DiaReparto> diaDeRepartos = await UIExecutor.ExecuteAsync(
                _scope,
                async sp =>
                {
                    var savingServices = sp.GetRequiredService<ISavingServices>();
                    var diaRepartoServicios = sp.GetRequiredService<IDiaRepartoServices>();
                    var nuevoDia = new DiaReparto();
                    string[] dias = new string[] { "Lunes", "Martes", "Miércoles", "Jueves", "Viernes", "Sábado" };
                    foreach (string dia in dias)
                    {
                        nuevoDia.Dia = dia;
                        diaRepartoServicios.AddDiaReparto(nuevoDia);
                    }
                    bool logradoFinal = await savingServices.SaveAsync();
                    if (!logradoFinal)
                    {
                        MessageBox.Show("No se crearon los Días de Reparto");
                        return null;
                    }
                    return await diaRepartoServicios.GetDiaRepartosAsync();
                },
                "Algo falló",
                this
            );
            if (diaDeRepartos == null || diaDeRepartos.Count == 0)
            {
                MessageBox.Show("Algo falla con la base de datos");
                return;
            }
            _lstDiaRepartos = diaDeRepartos;
        }
        private async Task ReCargarHDR(string diaReparto, string nombreReparto)
        {
            await Actualizar();
            List<Pedido> pedidos = await UIExecutor.ExecuteAsync(
                _scope,
                async sp =>
                {
                    var diaRepartoServices = sp.GetRequiredService<IDiaRepartoServices>();
                    var pedidoServices = sp.GetRequiredService<IPedidoServices>();
                    List<DiaReparto> lstDiasRep = await diaRepartoServices.GetDiaRepartosAsync();
                    Reparto reparto = lstDiasRep
                        .Find(x => x.Dia == diaReparto && x.Estado != "Eliminado").Reparto.ToList()
                        .Find(x => x.Nombre == nombreReparto && x.Estado != "Eliminado");
                    var pedidos = await pedidoServices.GetPedidosPorRepartoIdAsync(reparto.Id);
                    return pedidos.OrderBy(x => x.Orden).ToList();
                },
                "No se pudieron buscar los Pedidos",
                null
            );
            if (pedidos == null)
            {
                return;
            }
            _lstPedidos = pedidos;
        }
        private void LimpiarPantalla()
        {
            gpNuevoReparto.Visible = false;
            groupBox2.Visible = false;
            groupBox4.Visible = false;
            groupBox5.Visible = false;
            groupBox6.Visible = false;
            groupBox7.Visible = false;
            groupBox8.Visible = false;
            groupBox9.Visible = false;
            label30.Text = "";
            label31.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox2.Text = "";
            textBox6.Text = "";
            textBox1AgregarRepartoNombre.Text = "";
            textBox5.Text = "";
            textBox7.Text = "";
            label8AgregarPedidoDireccion.Text = "";
            comboBox6.Text = "";
            comboBox7.Text = "";
            comboBox8.Text = "";
            comboBox9.Text = "";
            comboBox10.Text = "";
            //checkBox1.Checked = false;
            label32.Text = "";
            label36.Text = "";
        }
        // MENUES
        private void AgregarReparto_ToolStripMenuItem_Click(object sender, EventArgs ev)
        {
            LimpiarPantalla();
            gpNuevoReparto.Visible = true;
            comboBox3.DataSource = _lstDiaRepartos;
            comboBox3.DisplayMember = "Dia";
            comboBox3.ValueMember = "Dia";
        }
        private void AgregarDestino_ToolStripMenuItem_Click(object sender, EventArgs ev)
        {
            LimpiarPantalla();
            groupBox2.Visible = true;
        }
        private void TodasLuAVi_ToolStripMenuItem_Click(object sender, EventArgs ev)
        {
            LimpiarPantalla();
            groupBox4.Visible = true;
        }
        private void DiaEspecífico_ToolStripMenuItem_Click(object sender, EventArgs ev)
        {
            LimpiarPantalla();
            groupBox5.Visible = true;
        }
        private void RepartoSeleccionado_ToolStripMenuItem_Click(object sender, EventArgs ev)
        {
            LimpiarPantalla();
            groupBox6.Visible = true;
        }
        private void Destino_ToolStripMenuItem_Click(object sender, EventArgs ev)
        {
            LimpiarPantalla();
            groupBox9.Visible = true;
            label39.Text = "Día " + comboBox1ListaDia.Text + " -> Reparto: " + comboBox2.Text;
        }
        private void PosicionarDestino_ToolStripMenuItem_Click(object sender, EventArgs ev)
        {
            LimpiarPantalla();
            groupBox7.Visible = true;
            label27.Text = "Día " + comboBox1ListaDia.Text + " -> Reparto: " + comboBox2.Text;
        }
        private void BorrarUnDestino_ToolStripMenuItem_Click(object sender, EventArgs ev)
        {
            LimpiarPantalla();
            groupBox8.Visible = true;
        }
    }
}
