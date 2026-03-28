using Models;
using Models.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace linway_app.Forms
{
    public partial class FormVentas : Form
    {
        private void ActualizarGrid3Ventas()
        {
            var grid = new List<EVenta>();
            foreach (Venta venta in _lstVentas)
            {
                grid.Add(Form1.Mapper.Map<EVenta>(venta));
            }
            grid = grid.OrderBy(x => x.ProductoId).ToList();
            dataGridView3.DataSource = grid;
            dataGridView3.Columns[0].Width = 35;
            dataGridView3.Columns[2].Width = 40;
            dataGridView3.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        }
    }
}
