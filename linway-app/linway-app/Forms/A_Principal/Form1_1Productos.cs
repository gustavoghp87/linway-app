using Models;
using Models.Entities;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace linway_app.Forms
{
    public partial class Form1 : Form
    {
        private void CargarGridProductos(ICollection<Producto> lstProductos)
        {
            if (lstProductos == null) {
                return;
            }
            var grid = new List<EProducto>();
            foreach (Producto producto in lstProductos)
            {
                grid.Add(Mapper.Map<EProducto>(producto));
            }
            dataGridView2.DataSource = grid;
            dataGridView2.Columns[0].Width = 48;
            dataGridView2.Columns[1].Width = 270;
            dataGridView2.Columns[2].Width = 72;
        }
        private void FiltrarDatosP(string texto)
        {
            var lstProductosFilstrados = new List<Producto>();
            foreach (var producto in _lstProductos)
            {
                if (producto.Nombre.ToLower().Contains(texto.ToLower()))
                {
                    lstProductosFilstrados.Add(producto);
                }
            }
            CargarGridProductos(lstProductosFilstrados);
        }
        private void Button10_Click(object sender, EventArgs ev)
        {
            if (BuscadorProductos.Visible)
            {
                button10.Text = ">>";
                label26.Visible = false;
                BuscadorProductos.Visible = false;
                BuscadorProductos.Text = "";
            }
            else
            {
                button10.Text = "<<";
                label26.Visible = true;
                BuscadorProductos.Visible = true;
                BuscadorClientes.Text = "";
            }
        }
        private void BuscadorProductos_TextChanged(object sender, EventArgs ev)
        {
            FiltrarDatosP(BuscadorProductos.Text);
        }
    }
}
