using linway_app.PresentationHelpers;
using linway_app.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Models;
using Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace linway_app.Forms
{
    public partial class FormRepartos : Form
    {
        private bool _usandoDialogo = false;  // ver que aparecen los eliminados en eliminar cliente de recorrido (por lista)>
        private void ActualizarGrid(ICollection<Pedido> lstPedidos)
        {
            if (lstPedidos == null)
            {
                return;
            }
            var grid1 = new List<EPedido>();
            foreach (Pedido pedido in lstPedidos)
            {
                grid1.Add(Form1.Mapper.Map<EPedido>(pedido));
            }
            grid1 = grid1.OrderBy(x => x.Orden).ToList();
            dataGridView1.Columns.Clear();
            dataGridView1.DataSource = grid1;
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[1].Width = 37;
            dataGridView1.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridView1.Columns[2].Width = 260;  // dirección
            dataGridView1.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridView1.Columns[3].Width = 420;  // productos
            dataGridView1.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridView1.Columns[4].Width = 53;   // entregar
            dataGridView1.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridView1.Columns[5].Width = 40;   // litros
            dataGridView1.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridView1.Columns[6].Width = 30;   // A
            dataGridView1.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridView1.Columns[7].Width = 30;   // E
            dataGridView1.Columns[7].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridView1.Columns[8].Width = 30;   // D
            dataGridView1.Columns[8].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridView1.Columns[9].Width = 30;   // T
            dataGridView1.Columns[9].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridView1.Columns[10].Width = 30;   // Ae
            dataGridView1.Columns[10].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridView1.Columns[11].Visible = false;      // orden
            dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;
            var eliminarColumna = new DataGridViewButtonColumn
            {
                Name = "Eliminar",
                HeaderText = "Eliminar",
                Text = "Eliminar",
                UseColumnTextForButtonValue = true,
                Visible = true,
                Width = 36
            };
            dataGridView1.Columns.RemoveAt(12);
            dataGridView1.Columns.Insert(12, eliminarColumna);
        }
        private async void Exportar_Click(object sender, EventArgs ev)
        {
            string diaReparto = comboBox1ListaDia.Text;
            string nombreReparto = comboBox2.Text;
            if (diaReparto == "" || nombreReparto == "")
            {
                return;
            }
            DialogResult dialogResult = MessageBox.Show("Exportar " + diaReparto + " - " + nombreReparto + " ¿Confirmar?", "Exportar Reparto a Excel", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                bool logrado = await UIExecutor.ExecuteAsync(
                    _scope,
                    async sp =>
                    {
                        var diaRepartoServices = sp.GetRequiredService<IDiaRepartoServices>();
                        var exportarServices = sp.GetRequiredService<IExportarServices>();
                        List<DiaReparto> lstDiasRep = await diaRepartoServices.GetDiaRepartosAsync();
                        Reparto reparto = lstDiasRep
                            .Find(x => x.Dia == diaReparto && x.Estado != "Eliminado").Reparto.ToList()
                            .Find(x => x.Nombre == nombreReparto && x.Estado != "Eliminado");
                        exportarServices.ExportarReparto(reparto);
                        return true;
                    },
                    "No se pudo exportar",
                    this
                );
                if (!logrado)
                {
                    return;
                }
                exportarButton.Text = "Terminado";
            }
        }
        private void VerDatos(Reparto reparto)
        {
            if (reparto == null)
            {
                return;
            }
            label14.Text = reparto.Ta.ToString();
            label15.Text = reparto.Te.ToString();
            label16.Text = reparto.Td.ToString();
            label17.Text = reparto.Tt.ToString();
            label18.Text = reparto.Tae.ToString();
            label21.Text = reparto.TotalB.ToString();
            label22.Text = reparto.Tl.ToString() + " litros";
        }
        private async Task ActualizarCombobox1()
        {
            if (comboBox1ListaDia.SelectedItem == null)
            {
                return;
            }
            comboBox2.Visible = true;
            label2.Visible = true;
            ActualizarGrid(new List<Pedido>());
            string diaReparto = comboBox1ListaDia.SelectedItem.ToString();
            List<Reparto> repartos = await UIExecutor.ExecuteAsync(
                _scope,
                async sp =>
                {
                    var diaRepartoServices = sp.GetRequiredService<IDiaRepartoServices>();
                    List<DiaReparto> lstDiasRep = await diaRepartoServices.GetDiaRepartosAsync();
                    return lstDiasRep.Find(x => x.Dia == diaReparto && x.Estado != "Eliminado").Reparto.OrderBy(x => x.Id).ToList();
                },
                "No se pudieron buscar los Repartos por Día",
                null
            );
            if (repartos == null)
            {
                return;
            }
            comboBox2.SelectedIndexChanged -= ComboBox2_SelectedIndexChanged;  // evita error de concurrencia de DbContext
            comboBox2.DataSource = repartos.Count > 0 ? repartos : null;
            comboBox2.SelectedIndexChanged += ComboBox2_SelectedIndexChanged;
            comboBox2.DisplayMember = "Nombre";
            comboBox2.ValueMember = "Nombre";
        }
        private async void ComboBox1_SelectedIndexChanged(object sender, EventArgs ev)
        {
            await ActualizarCombobox1();
        }
        private async Task UpdateGrid()
        {
            string diaReparto = comboBox1ListaDia.Text;  // .SelectedItem.ToString();
            if (diaReparto == "")
            {
                return;
            }
            string nombreReparto = comboBox2.Text;
            bool soloAEntregar = checkBox1.Checked;
            var respuesta = await UIExecutor.ExecuteAsync(
                _scope,
                async sp =>
                {
                    var diaRepartoServices = sp.GetRequiredService<IDiaRepartoServices>();
                    var pedidoServices = sp.GetRequiredService<IPedidoServices>();
                    List<DiaReparto> lstDiasRep = await diaRepartoServices.GetDiaRepartosAsync();
                    Reparto reparto = lstDiasRep
                        .Find(x => x.Dia == diaReparto && x.Estado != "Eliminado").Reparto.ToList()
                        .Find(x => x.Nombre == nombreReparto && x.Estado != "Eliminado") ?? throw new Exception("No se encontró Reparto");
                    List<Pedido> pedidos;
                    var pedidos1 = await pedidoServices.GetPedidosPorRepartoIdAsync(reparto.Id);
                    if (soloAEntregar)
                    {
                        pedidos = pedidos1.Where(x => x.Entregar == 1).ToList();
                    }
                    else
                    {
                        pedidos = pedidos1.ToList();
                    }
                    return (reparto, pedidos);
                },
                "No se pudieron buscar los Repartos por Día",
                null
            );
            if (respuesta.reparto == null || respuesta.pedidos == null)
            {
                return;
            }
            _lstPedidos = respuesta.pedidos;
            VerDatos(respuesta.reparto);
            ActualizarGrid(_lstPedidos);
        }
        private async void ComboBox2_SelectedIndexChanged(object sender, EventArgs ev)
        {
            await UpdateGrid();
        }
        private async void CheckBox1_CheckedChanged(object sender, EventArgs ev)
        {
            if (!checkBox1.Checked)
            {
                await Actualizar();
                return;
            }
            var ldFiltrada = new List<Pedido>();
            foreach (Pedido pedido in _lstPedidos)
            {
                if (pedido.Entregar == 1) ldFiltrada.Add(pedido);
            }
            ActualizarGrid(ldFiltrada);
        }
        private async void DataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)  // eliminar destino desde la lista
        {
            if (_usandoDialogo || !(e.RowIndex >= 0 && e.ColumnIndex >= 0))
            {
                return;
            }
            _usandoDialogo = true;
            var cell = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex];
            if (cell != null && cell.EditedFormattedValue != null && cell.EditedFormattedValue.ToString() == "Eliminar")
            {
                int row = e.RowIndex;
                Pedido pedido = _lstPedidos.OrderBy(x => x.Orden).ToList()[row];
                string direccion = pedido.Direccion;
                DialogResult result = MessageBox.Show(
                    $"¿Eliminar {direccion} ? Cliente: {pedido.ClienteId}",
                    "Se va a eliminar " + direccion,
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Question
                );
                if (result == DialogResult.OK)
                {
                    bool logrado = await UIExecutor.ExecuteAsync(
                        _scope,
                        async sp => {
                            var savingServices = sp.GetRequiredService<ISavingServices>();
                            var pedidoServices = sp.GetRequiredService<IPedidoServices>();
                            pedidoServices.DeletePedido(pedido);
                            bool guardado = await savingServices.SaveAsync();
                            if (!guardado)
                            {
                                savingServices.DiscardChanges();
                                MessageBox.Show("No se hicieron cambios");
                            }
                            return guardado;
                        },
                        "No se pudo realizar",
                        this
                    );
                    if (!logrado)
                    {
                        return;
                    }
                    MessageBox.Show($"Eliminado {direccion} ID: {pedido.Id}");
                    await UpdateGrid();
                }
            }
            _usandoDialogo = false;
        }
    }
}
