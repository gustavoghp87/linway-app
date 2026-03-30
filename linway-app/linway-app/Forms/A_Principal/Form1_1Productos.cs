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
            var grid = new List<EProducto>();
            foreach (Producto producto in lstProductos)
            {
                grid.Add(Mapper.Map<EProducto>(producto));
            }
            dataGridViewProductos.DataSource = grid;
            dataGridViewProductos.Columns[0].Width = 48;
            dataGridViewProductos.Columns[1].Width = 270;
            dataGridViewProductos.Columns[2].Width = 72;
        }
        private void FiltrarDatosP(string texto)
        {
            var lstProductosFilstrados = _lstProductos.FindAll(pr => pr.Nombre.ToLower().Contains(texto.ToLower()));
            CargarGridProductos(lstProductosFilstrados);
        }
        private void Button10_Click(object sender, EventArgs ev)
        {
            if (BuscadorProductos.Visible)
            {
                buttonMostrarFiltroProductos.Text = ">>";
                labelBuscarPorDireccion.Visible = false;
                BuscadorProductos.Visible = false;
                BuscadorProductos.Text = "";
            }
            else
            {
                buttonMostrarFiltroProductos.Text = "<<";
                labelBuscarPorDireccion.Visible = true;
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
