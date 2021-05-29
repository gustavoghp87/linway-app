using linway_app.Models;
using linway_app.Repositories;
using linway_app.Services;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace linway_app.Forms
{
    public partial class Form1 : Form
    {
        private readonly IServicioCliente _servCliente;
        private readonly IServicioProducto _servProducto;

        public Form1(IServicioCliente servCliente, IServicioProducto servProducto)
        {
            _servCliente = servCliente;
            _servProducto = servProducto;
            try { InitializeComponent(); } catch (Exception e) { MessageBox.Show(e.ToString()); return; }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            Actualizar();
        }

        public void Actualizar()
        {
            var lstClientes = GetClientes();
            if (lstClientes != null && lstClientes.Count > 0)
            {
                dataGridView1.DataSource = lstClientes.ToArray();
                dataGridView1.Columns[0].Width = 40;
                dataGridView1.Columns[1].Width = 250;
                dataGridView1.Columns[2].Width = 60;
                dataGridView1.Columns[3].Width = 60;
                dataGridView1.Columns[4].Width = 200;
                dataGridView1.Columns[5].Width = 65;
                dataGridView1.Columns[6].Width = 65;
            }
            var lstProductos = GetProductos();
            if (lstProductos != null && lstClientes.Count > 0)
            {
                dataGridView2.DataSource = lstProductos.ToArray();
                dataGridView2.Columns[0].Width = 48;
                dataGridView2.Columns[1].Width = 220;
                dataGridView2.Columns[2].Width = 72;
            }
        }
        private List<Cliente> GetClientes()
        {
            List<Cliente> lstClientes = _servCliente.GetAll();
            return lstClientes;
        }
        private List<Producto> GetProductos()
        {
            List<Producto> lstProductos = _servProducto.GetAll();
            return lstProductos;
        }


        // MENUES
        void Frm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Actualizar();
        }
        private void AbrirClientes_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormClientes form = new FormClientes(new ServicioCliente(new UnitOfWork(new LinwaydbContext())));
            form.FormClosing += Frm_FormClosing;
            form.Show();
        }
        private void AbrirProductos_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormProductos form = new FormProductos(new ServicioProducto(new UnitOfWork(new LinwaydbContext())));
            form.FormClosing += Frm_FormClosing;
            form.Show();
        }
        private void AbrirCrearNotaDeEnvío_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormCrearNota form = new FormCrearNota(new ServicioNotaDeEnvio(new UnitOfWork(new LinwaydbContext())));
            form.FormClosing += Frm_FormClosing;
            form.Show();
        }
        private void AbrirNotasDeEnvio_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormNotasEnvio form = new FormNotasEnvio();
            form.FormClosing += Frm_FormClosing;
            form.Show();
        }
        private void AbrirHojasDeReparto_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormReparto form = new FormReparto();
            form.FormClosing += Frm_FormClosing;
            form.Show();
        }
        private void AbrirRecibos_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormRecibos form = new FormRecibos();
            form.FormClosing += Frm_FormClosing;
            form.Show();
        }
        private void AbrirVentas_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormVentas form = new FormVentas();
            form.FormClosing += Frm_FormClosing;
            form.Show();
        }



        //BUSCAR CLIENTE
        void FiltrarDatosC(string texto)  // filtra por dirección de clientes
        {
            var lstClientes = GetClientes();
            var lstClientesFiltrados = new List<ICliente>();
            foreach (var cliente in lstClientes)
            {
                if (cliente.Direccion.ToLower().Contains(texto.ToLower()))
                {
                    lstClientesFiltrados.Add(cliente);
                }
            }
            dataGridView1.DataSource = lstClientesFiltrados.ToArray();
        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {
            FiltrarDatosC(BuscadorClientes.Text);
        }

        private void button5_Click(object sender, EventArgs e)
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

        //BUSCAR PRODUCTO
        void FiltrarDatosP(string texto)
        {
            List<Producto> lstProductos = GetProductos();
            List<Producto> lstProductosFilstrados = new List<Producto>();
            foreach (var producto in lstProductos)
            {
                if (producto.Nombre.ToLower().Contains(texto.ToLower()))
                {
                    lstProductosFilstrados.Add(producto);
                }
            }
            dataGridView2.DataSource = lstProductosFilstrados.ToArray();
        }

        private void Button10_Click(object sender, EventArgs e)
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

        private void BuscadorProductos_TextChanged(object sender, EventArgs e)
        {
            FiltrarDatosP(BuscadorProductos.Text);
        }

    }
}
