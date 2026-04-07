using Models;
using Models.Entities;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace linway_app.Forms
{
    public partial class FormCrearNota : Form
    {
        private void ActualizarGrid()
        {
            var grid = new List<EProdVendido>();
            foreach (ProdVendido prodVendido in _lstProdVendidosAAgregar)
            {
                grid.Add(Form1.Mapper.Map<EProdVendido>(prodVendido));
            }
            dataGridView4.DataSource = grid;
            dataGridView4.Columns[0].Width = 28;
            dataGridView4.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight;
            dataGridView4.Columns[1].Width = 200;
            dataGridView4.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight;
            dataGridView4.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight;
            dataGridView4.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight;
        }
        private void LimpiarLista()
        {
            _lstProdVendidosAAgregar.Clear();
            _lstProductosAAgregar.Clear();
            ActualizarGrid();
            label42ImporteTotal.Text = "";
        }
        private void LimpiarLista_Click(object sender, EventArgs ev)
        {
            LimpiarLista();
        }
    }
}
