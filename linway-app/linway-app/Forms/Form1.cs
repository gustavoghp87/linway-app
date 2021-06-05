using linway_app.Models;
using linway_app.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using static linway_app.Forms.Delegates.DClientes;
using static linway_app.Forms.Delegates.DProductos;

namespace linway_app.Forms
{
    public partial class Form1 : Form
    {
        public static IServicioCliente _servCliente;
        public static IServicioDetalleRecibo _servDetalleRecibo;
        public static IServicioDiaReparto _servDiaReparto;
        public static IServicioNotaDeEnvio _servNotaDeEnvio;
        public static IServicioPedido _servPedido;
        public static IServicioProducto _servProducto;
        public static IServicioProdVendido _servProdVendido;
        public static IServicioRecibo _servRecibo;
        public static IServicioRegistroVenta _servRegistroVenta;
        public static IServicioReparto _servReparto;
        public static IServicioVenta _servVenta;
        public Form1(
            IServicioCliente servCliente,
            IServicioDetalleRecibo servDetalleRecibo,
            IServicioDiaReparto servDiaReparto,
            IServicioNotaDeEnvio servNotaDeEnvio,
            IServicioPedido servPedido,
            IServicioProducto servProducto,
            IServicioProdVendido servProdVendido,
            IServicioRecibo servRecibo,
            IServicioRegistroVenta servRegistroVenta,
            IServicioReparto servReparto,
            IServicioVenta servVenta
            )
        {
            _servCliente = servCliente;
            _servDetalleRecibo = servDetalleRecibo;
            _servDiaReparto = servDiaReparto;
            _servNotaDeEnvio = servNotaDeEnvio;
            _servPedido = servPedido;
            _servProducto = servProducto;
            _servProdVendido = servProdVendido;
            _servRecibo = servRecibo;
            _servRegistroVenta = servRegistroVenta;
            _servReparto = servReparto;
            _servVenta = servVenta;
            try { InitializeComponent(); } catch (Exception e) { MessageBox.Show(e.Message); return; }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            Actualizar();
        }
        public void Actualizar()
        {
            List<Cliente> lstClientes = getClientes();
            CargarGrid1(lstClientes);
            List<Producto> lstProductos = getProductos();
            CargarGrid2(lstProductos);
        }
        private void CargarGrid1(List<Cliente> lstClientes)
        {
            if (lstClientes != null && lstClientes.Count > 0)
            {
                dataGridView1.DataSource = lstClientes.ToArray();
                dataGridView1.Columns[0].Width = 40;
                dataGridView1.Columns[1].Width = 250;
                dataGridView1.Columns[2].Width = 60;
                dataGridView1.Columns[3].Width = 90;
                dataGridView1.Columns[4].Width = 90;
                dataGridView1.Columns[5].Width = 65;
                dataGridView1.Columns[6].Width = 65;
                dataGridView1.Columns[7].Visible = false;
                dataGridView1.Columns[8].Visible = false;
                dataGridView1.Columns[9].Visible = false;
                dataGridView1.Columns[10].Visible = false;
                dataGridView1.Columns[11].Visible = false;
            }
            else
            {
                CrearPrimerCliente();
                Actualizar();
            }
        }
        private void CargarGrid2(List<Producto> lstProductos)
        {
            if (lstProductos != null && lstProductos.Count > 0)
            {
                dataGridView2.DataSource = lstProductos.ToArray();
                dataGridView2.Columns[0].Width = 48;
                dataGridView2.Columns[1].Width = 220;
                dataGridView2.Columns[2].Width = 72;
                dataGridView2.Columns[3].Visible = false;
                dataGridView2.Columns[4].Visible = false;
                dataGridView2.Columns[5].Visible = false;
            }
        }
        private void CrearPrimerCliente()
        {
            try
            {
                bool response = addCliente(new Cliente
                {
                    Name = "Cliente Particular X",
                    Direccion = "Cliente Particular X",
                    Estado = "Activo"
                });
                if (!response)
                {
                    MessageBox.Show("Hay problemas con la base de datos y no se puede seguir");
                    Close();
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show("Hay problemas con la base de datos y no se puede seguir: " + exception.Message);
                Close();
            }
        }


        // MENUES
        void Frm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Actualizar();
        }
        private void AbrirClientes_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = Program.GetConfig().GetRequiredService<FormClientes>();
            form.FormClosing += Frm_FormClosing;
            form.Show();
        }
        private void AbrirProductos_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = Program.GetConfig().GetRequiredService<FormProductos>();
            form.FormClosing += Frm_FormClosing;
            form.Show();
        }
        private void AbrirCrearNotaDeEnvío_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = Program.GetConfig().GetRequiredService<FormCrearNota>();
            form.FormClosing += Frm_FormClosing;
            form.Show();
        }
        private void AbrirNotasDeEnvio_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = Program.GetConfig().GetRequiredService<FormNotasEnvio>();
            form.FormClosing += Frm_FormClosing;
            form.Show();
        }
        private void AbrirHojasDeReparto_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = Program.GetConfig().GetRequiredService<FormReparto>();
            form.FormClosing += Frm_FormClosing;
            form.Show();
        }
        private void AbrirRecibos_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = Program.GetConfig().GetRequiredService<FormRecibos>();
            form.FormClosing += Frm_FormClosing;
            form.Show();
        }
        private void AbrirVentas_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = Program.GetConfig().GetRequiredService<FormVentas>();
            form.FormClosing += Frm_FormClosing;
            form.Show();
        }



        //BUSCAR CLIENTE
        void FiltrarDatosC(string texto)  // filtra por dirección de clientes
        {
            var lstClientes = getClientes();
            var lstClientesFiltrados = new List<Cliente>();
            foreach (var cliente in lstClientes)
            {
                if (cliente.Direccion.ToLower().Contains(texto.ToLower()))
                {
                    lstClientesFiltrados.Add(cliente);
                }
            }
            CargarGrid1(lstClientesFiltrados);
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
            List<Producto> lstProductos = getProductos();
            List<Producto> lstProductosFilstrados = new List<Producto>();
            foreach (var producto in lstProductos)
            {
                if (producto.Nombre.ToLower().Contains(texto.ToLower()))
                {
                    lstProductosFilstrados.Add(producto);
                }
            }
            CargarGrid2(lstProductosFilstrados);
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
