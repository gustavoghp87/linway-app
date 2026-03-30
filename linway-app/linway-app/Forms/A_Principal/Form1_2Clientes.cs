using Models;
using Models.Entities;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace linway_app.Forms
{
    public partial class Form1 : Form
    {
        private void CargarGridClientes(ICollection<Cliente> lstClientes)
        {
            var grid = new List<ECliente>();
            foreach (Cliente cliente in lstClientes)
            {
                grid.Add(Mapper.Map<ECliente>(cliente));
            }
            dataGridViewClientes.DataSource = grid.ToArray();
            dataGridViewClientes.Columns[0].Width = 40;
            dataGridViewClientes.Columns[1].Width = 350;
            dataGridViewClientes.Columns[2].Width = 90;
            dataGridViewClientes.Columns[3].Width = 120;
            dataGridViewClientes.Columns[4].Width = 65;
            dataGridViewClientes.Columns[5].Width = 65;
            dataGridViewClientes.Columns[6].Width = 40;
        }
        private void FiltrarDatosC(string texto)  // filtra por dirección de clientes
        {
            var lstClientesFiltrados = _lstClientes.FindAll(c => c.Direccion.ToLower().Contains(texto.ToLower()));
            CargarGridClientes(lstClientesFiltrados);
        }
        private void TextBox8_TextChanged(object sender, EventArgs ev)
        {
            FiltrarDatosC(BuscadorClientes.Text);
        }
        private void Button5_Click(object sender, EventArgs ev)
        {
            if (BuscadorClientes.Visible)
            {
                buttonMostrarFiltroClientes.Text = ">>";
                labelBuscarPorNombreCliente.Visible = false;
                BuscadorClientes.Visible = false;
                BuscadorClientes.Text = "";
            }
            else
            {
                buttonMostrarFiltroClientes.Text = "<<";
                labelBuscarPorNombreCliente.Visible = true;
                BuscadorClientes.Visible = true;
                BuscadorClientes.Text = "";
            }
        }
    }
}
