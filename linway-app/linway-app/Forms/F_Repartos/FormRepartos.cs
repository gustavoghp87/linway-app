using linway_app.PresentationHelpers;
using linway_app.Services.FormServices;
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
                    var servicesContext = ServiceContext.Get(sp);
                    return await servicesContext.DiaRepartoServices.GetAllAsync();
                },
                "No se pudieron buscar los Días de Reparto",
                null
            );
            _lstDiaRepartos = diaRepartos;
            ActualizarPedidosYGridDePedidos();
        }
        private async Task ReCargarHDR(string diaReparto, string nombreReparto)
        {
            await Actualizar();
            List<Pedido> pedidos = _lstDiaRepartos.Find(x => x.Dia == diaReparto)?.Repartos?.ToList().Find(x => x.Nombre == nombreReparto)?.Pedidos?.ToList().OrderBy(x => x.Orden).ToList() ?? new List<Pedido>();
            //    _scope,
            //    async sp =>
            //    {
            //        var servicesContext = ServiceContext.Get(sp);
            //        List<DiaReparto> lstDiasRep = await servicesContext.DiaRepartoServices.GetAllAsync();
            //        Reparto reparto = lstDiasRep
            //            .Find(x => x.Dia == diaReparto).Repartos.ToList()
            //            .Find(x => x.Nombre == nombreReparto);
            //        var pedidos = await servicesContext.PedidoServices.GetPorRepartoIdAsync(reparto.Id);
            //        return pedidos.OrderBy(x => x.Orden).ToList();
            //    },
            //    "No se pudieron buscar los Pedidos",
            //    null
            //);
            //if (pedidos == null)
            //{
            //    return;
            //}
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
            groupBox10.Visible = false;
            label30ReposicionarObjetivo.Text = "";
            label31ReposicionarReferencia.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox2.Text = "";
            textBox6.Text = "";
            textBox1AgregarRepartoNombre.Text = "";
            textBox5EliminarPedido_Direccion.Text = "";
            textBox7.Text = "";
            label8AgregarPedidoDireccion.Text = "";
            comboBox6.Text = "";
            comboBox7.Text = "";
            comboBox8.Text = "";
            comboBox9Repartos_Nombre.Text = "";
            comboBox10Repartos_Dia.Text = "";
            //checkBox1.Checked = false;
            label32EliminarPedido_Direccion.Text = "";
            label36.Text = "";
            checkBox2.Checked = false;
        }
        private async Task<bool> EliminarPedidoAsync(Pedido pedidoAEliminar)  // handler
        {
            bool logrado = await UIExecutor.ExecuteAsync(
                _scope,
                async sp => {
                    var servicesContext = ServiceContext.Get(sp);
                    Reparto reparto = await servicesContext.RepartoServices.GetPorIdAsync(pedidoAEliminar.RepartoId);
                    //
                    List<ProdVendido> prodVendidosDelPedido = pedidoAEliminar.ProdVendidos.ToList();
                    foreach (ProdVendido prodVendido in prodVendidosDelPedido)
                    {
                        prodVendido.PedidoId = null;
                    }
                    servicesContext.ProdVendidoServices.EditOrDeleteMany(prodVendidosDelPedido);
                    //
                    servicesContext.PedidoServices.Delete(pedidoAEliminar);
                    //
                    bool guardado = await servicesContext.SavingServices.SaveAsync();
                    if (!guardado)
                    {
                        servicesContext.SavingServices.DiscardChanges();
                        MessageBox.Show("No se hicieron cambios");
                    }
                    return guardado;
                },
                "No se pudo realizar",
                this
            );
            return logrado;
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
            label39.Text = "Día " + comboBox1ListaDias.Text + " -> Reparto: " + comboBox2ListaRepartos.Text;
        }
        private void PosicionarDestino_ToolStripMenuItem_Click(object sender, EventArgs ev)
        {
            LimpiarPantalla();
            groupBox7.Visible = true;
            label27.Text = "Día " + comboBox1ListaDias.Text + " -> Reparto: " + comboBox2ListaRepartos.Text;
        }
        private void BorrarUnDestino_ToolStripMenuItem_Click(object sender, EventArgs ev)
        {
            LimpiarPantalla();
            groupBox8.Visible = true;
        }
        private void EliminarRepartoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LimpiarPantalla();
            groupBox10.Visible = true;
        }
    }
}
