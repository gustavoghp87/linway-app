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
            if (_lstProdVendidosAAgregar == null)
            {
                return;
            }
            var grid = new List<EProdVendido>();
            foreach (ProdVendido prodVendido in _lstProdVendidosAAgregar)
            {
                grid.Add(Form1.Mapper.Map<EProdVendido>(prodVendido));
            }
            dataGridView4.DataSource = grid;
            dataGridView4.Columns[0].Width = 28;
            dataGridView4.Columns[1].Width = 200;
        }
        private void LimpiarLista_Click(object sender, EventArgs ev)
        {
            _lstProdVendidosAAgregar.Clear();
            _lstProductosAAgregar.Clear();
            ActualizarGrid();
            label42.Text = "";
        }
    }
}
