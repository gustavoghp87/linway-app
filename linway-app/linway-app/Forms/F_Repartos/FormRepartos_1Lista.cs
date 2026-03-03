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
        private bool _usandoDialogo = false;  // ver que aparecen los eliminados en eliminar cliente de recorrido (por lista)
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
            dataGridView1.Columns[2].Width = 230;
            dataGridView1.Columns[3].Width = 320;
            dataGridView1.Columns[4].Width = 53;
            dataGridView1.Columns[5].Width = 30;
            dataGridView1.Columns[6].Width = 30;
            dataGridView1.Columns[7].Width = 30;
            dataGridView1.Columns[8].Width = 30;
            dataGridView1.Columns[9].Width = 30;
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
            string dia = comboBox1.Text;
            string nombreReparto = comboBox2.Text;
            if (dia == "" || nombreReparto == "")
            {
                return;
            }
            DialogResult dialogResult = MessageBox.Show("Exportar " + dia + " - " + nombreReparto + " ¿Confirmar?", "Exportar Reparto a Excel", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                bool logrado = await UIExecutor.ExecuteAsync(
                    _scope,
                    async sp =>
                    {
                        var orquestacionServices = sp.GetRequiredService<IOrquestacionServices>();
                        await orquestacionServices.ExportRepartoAsync(dia, nombreReparto);
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
        private async void ComboBox1_SelectedIndexChanged(object sender, EventArgs ev)
        {
            comboBox2.Visible = true;
            label2.Visible = true;
            ActualizarGrid(new List<Pedido>());
            if (_lstDiaRepartos.Count == 0)
            {
                return;
            }
            string dia = comboBox1.SelectedItem.ToString();
            List<Reparto> repartos = await UIExecutor.ExecuteAsync(
                _scope,
                async sp =>
                {
                    var orquestacionServices = sp.GetRequiredService<IOrquestacionServices>();
                    return await orquestacionServices.GetRepartosPorDiaAsync(dia);
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
        private async Task UpdateGrid()
        {
            string dia = comboBox1.Text;  // .SelectedItem.ToString();
            if (dia == "")
            {
                return;
            }
            string nombre = comboBox2.Text;
            bool soloAEntregar = checkBox1.Checked;
            var respuesta = await UIExecutor.ExecuteAsync(
                _scope,
                async sp =>
                {
                    var orquestacionServices = sp.GetRequiredService<IOrquestacionServices>();
                    var pedidoServices = sp.GetRequiredService<IPedidoServices>();
                    Reparto reparto = await orquestacionServices.GetRepartoPorDiaYNombreAsync(dia, nombre);
                    if (reparto == null)
                    {
                        throw new Exception("No se encontró Reparto");
                    }
                    List<Pedido> pedidos;
                    var pedidos1 = await pedidoServices.GetPedidosPorRepartoId(reparto.Id);
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
        // eliminar destino
        private async void DataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
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
                            return await savingServices.SaveAsync();
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
