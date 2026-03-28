using linway_app.PresentationHelpers;
using linway_app.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Models;
using Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace linway_app.Forms
{
    public partial class FormRepartos : Form
    {
        private int _selectedIndex = -1;
        private bool _usandoDialogo = false;  // ver que aparecen los eliminados en eliminar cliente de recorrido (por lista)>
        private void ActualizarGridDePedidos(ICollection<Pedido> lstPedidos)
        {
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
            string diaReparto = comboBox1ListaDias.Text;
            string nombreReparto = comboBox2ListaRepartos.Text;
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
                        var exportarServices = sp.GetRequiredService<IExportarServices>();
                        Reparto reparto = _lstDiaRepartos
                            .Find(x => x.Dia == diaReparto).Repartos.ToList()
                            .Find(x => x.Nombre == nombreReparto);
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
        private void ActualizarCantidadesDeReparto(Reparto reparto)
        {
            label14.Text = reparto.Ta.ToString();
            label15.Text = reparto.Te.ToString();
            label16.Text = reparto.Td.ToString();
            label17.Text = reparto.Tt.ToString();
            label18.Text = reparto.Tae.ToString();
            label21.Text = reparto.TotalB.ToString();
            label22.Text = reparto.Tl.ToString() + " litros";
        }
        private void ActualizarCombobox1()
        {
            var diaSeleccionado = comboBox1ListaDias.SelectedItem;
            if (diaSeleccionado == null)
            {
                return;
            }
            comboBox2ListaRepartos.Visible = true;
            label2.Visible = true;
            string diaReparto = diaSeleccionado.ToString();
            List<Reparto> repartos = _lstDiaRepartos.Find(x => x.Dia == diaReparto).Repartos.OrderBy(x => x.Id).ToList();
            //
            comboBox2ListaRepartos.DataSource = repartos.Count > 0 ? repartos : null;
            comboBox2ListaRepartos.DisplayMember = "Nombre";
            comboBox2ListaRepartos.ValueMember = "Nombre";
            if (_selectedIndex == -1)
            {
                comboBox2ListaRepartos.SelectedIndex = repartos.Count > 0 ? 0 : -1;
            }
            else
            {
                comboBox2ListaRepartos.SelectedIndex = _selectedIndex;
            }
            _selectedIndex = comboBox2ListaRepartos.SelectedIndex;
        }
        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs ev)
        {
            ActualizarCombobox1();
        }
        private async void ComboBox2_SelectedIndexChanged(object sender, EventArgs ev)
        {
            _selectedIndex = -1;
            ActualizarPedidosYGridDePedidos();
        }
        private void CambiarSoloAEntregar()
        {
            bool soloAEntregar = checkBox1.Checked;
            if (soloAEntregar)
            {
                ActualizarGridDePedidos(_lstPedidos.FindAll(x => x.Entregar == 1));
            }
            else
            {
                ActualizarGridDePedidos(_lstPedidos);
            }
        }
        private void ActualizarPedidosYGridDePedidos()
        {
            var repartoSeleccionado = comboBox2ListaRepartos.SelectedItem;
            if (repartoSeleccionado == null)
            {
                return;
            }
            Reparto reparto = (Reparto)repartoSeleccionado;
            List<Pedido> pedidos = reparto.Pedidos.ToList();
            _lstPedidos = pedidos;
            ActualizarCantidadesDeReparto(reparto);
            CambiarSoloAEntregar();
        }
        private async void CheckBox1_CheckedChanged(object sender, EventArgs ev)
        {
            CambiarSoloAEntregar();
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
                    bool logrado = await EliminarPedidoAsync(pedido);
                    if (!logrado)
                    {
                        return;
                    }
                    MessageBox.Show($"Eliminado {direccion} ID: {pedido.Id}");
                    LimpiarPantalla();
                    await Actualizar();
                }
            }
            _usandoDialogo = false;
        }
    }
}
