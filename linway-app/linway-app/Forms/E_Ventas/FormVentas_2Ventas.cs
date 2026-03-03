using Models;
using Models.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace linway_app.Forms
{
    public partial class FormVentas : Form
    {
        private void ActualizarGrid2ProdVendidos (ICollection<ProdVendido> lstProdVendidos)
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
            if (showing == "agregarReg")
            {
                dataGridView2.Columns[0].Width = 150;
            }
            if (showing == "verReg")
            {
                dataGridView2.Columns[0].Width = 28;
                dataGridView2.Columns[1].Width = 150;
            }
        }
        private void ActualizarGrid3Ventas()
        {
            if (_lstVentas == null)
            {
                return;
            }
            var grid = new List<EVenta>();
            foreach (Venta venta in _lstVentas)
            {
                grid.Add(Form1.Mapper.Map<EVenta>(venta));
            }
            grid = grid.OrderBy(x => x.Detalle).ToList();
            dataGridView3.DataSource = grid;
            dataGridView3.Columns[0].Width = 20;
            dataGridView3.Columns[1].Width = 40;
        }

        private void ActualizarGrid5(ICollection<Venta> lstVentas)
        {
            if (lstVentas == null)
            {
                return;
            }
            var grid = new List<EVenta>();
            foreach (Venta venta in lstVentas)
            {
                grid.Add(Form1.Mapper.Map<EVenta>(venta));
            }
            grid = grid.OrderBy(x => x.Detalle).ToList();
            dataGridView5.DataSource = grid;
            dataGridView5.Columns[0].Width = 280;
        }
    }
}
