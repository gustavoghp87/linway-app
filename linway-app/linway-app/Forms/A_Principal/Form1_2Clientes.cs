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
            dataGridView1.DataSource = grid.ToArray();
            dataGridView1.Columns[0].Width = 40;
            dataGridView1.Columns[1].Width = 350;
            dataGridView1.Columns[2].Width = 90;
            dataGridView1.Columns[3].Width = 120;
            dataGridView1.Columns[4].Width = 65;
            dataGridView1.Columns[5].Width = 65;
            dataGridView1.Columns[6].Width = 40;
        }
        private void FiltrarDatosC(string texto)                      // filtra por dirección de clientes
        {
            var lstClientesFiltrados = new List<Cliente>();
            foreach (var cliente in _lstClientes)
            {
                if (cliente.Direccion.ToLower().Contains(texto.ToLower()))
                {
                    lstClientesFiltrados.Add(cliente);
                }
            }
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
                button5.Text = ">>";
                label11.Visible = false;
                BuscadorClientes.Visible = false;
                BuscadorClientes.Text = "";
            }
            else
            {
                button5.Text = "<<";
                label11.Visible = true;
                BuscadorClientes.Visible = true;
                BuscadorClientes.Text = "";
            }
        }
    }
}
