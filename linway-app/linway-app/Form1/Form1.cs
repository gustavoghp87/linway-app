using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Threading;


namespace linway_app
{
    public partial class Form1 : Form
    {
        List<Cliente> listaClientes = new List<Cliente>();
        List<Producto> listaProductos = new List<Producto>();

        public Form1()
        {
            try { InitializeComponent(); } catch (Exception e) { MessageBox.Show(e.ToString()); }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Actualizar();
            
            // location (12, 96)     size (340, 281)
            dataGridView2.Columns[0].Width = 48;
            dataGridView2.Columns[1].Width = 220;
            dataGridView2.Columns[2].Width = 72;

            // location (12, 388)     size (898, 150)
            dataGridView1.Columns[0].Width = 40;
            dataGridView1.Columns[1].Width = 250;
            dataGridView1.Columns[2].Width = 60;
            dataGridView1.Columns[3].Width = 60;
            dataGridView1.Columns[4].Width = 200;
            dataGridView1.Columns[5].Width = 65;
            dataGridView1.Columns[6].Width = 65;

            //Wait(5000);
        }

        public void Actualizar()
        {
            try
            {
                listaClientes = new FormClientes().CargarClientes();
                dataGridView1.DataSource = listaClientes.ToArray();
            }
            catch (Exception e)
            {
                MessageBox.Show("Error al actualizar clientes: " + e.ToString());
            }
            try
            {
                listaProductos = new FormProductos().CargarProductos();
                dataGridView2.DataSource = listaProductos.ToArray();
            }
            catch (Exception e)
            {
                MessageBox.Show("Error al actualizar productos: " + e.ToString());
            }
            //Wait(5000);
        }

        public void Wait(int milliseconds)
        {
            var timer1 = new System.Windows.Forms.Timer();
            if (milliseconds == 0 || milliseconds < 0) return;

            // Console.WriteLine("start wait timer");
            timer1.Interval = milliseconds;
            timer1.Enabled = true;
            timer1.Start();

            timer1.Tick += (s, e) =>
            {
                timer1.Enabled = false;
                timer1.Stop();
                // Console.WriteLine("stop wait timer");
            };

            while (timer1.Enabled)
            {
                MessageBox.Show("Actualizando...");
                Actualizar();
            }
        }

        private void botonActualizar_Click(object sender, EventArgs e)
        {
            Actualizar();
        }


        // MENUES .
        private void verClientesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormClientes fClientes = new FormClientes();
            fClientes.AgregarClientes();
            fClientes.Show();
        }

        private void agregarProductoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormProductos fAgregar = new FormProductos();
            fAgregar.AgregarProductos();
            fAgregar.Show();
        }

        private void modificarProductoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormProductos fModificar = new FormProductos();
            fModificar.ModificarProductos();
            fModificar.Show();
        }

        private void modificarClienteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormClientes fModificar = new FormClientes();
            fModificar.ModificarClientes();
            fModificar.Show();
        }

        private void crearNotaDeEnvíoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormCrearNota crearNotaEnvio = new FormCrearNota();
            crearNotaEnvio.cargarProductosYClientes(listaProductos, listaClientes);
            crearNotaEnvio.Show();
        }

        private void verNotasDeEnvíoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormNotasEnvio ventanaNotas = new FormNotasEnvio();
            ventanaNotas.RecibirProductos(listaProductos);
            ventanaNotas.Show();
        }

        private void borrarProductoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormProductos fBorrar = new FormProductos();
            fBorrar.BorrarProductos();
            fBorrar.Show();
        }

        private void borrarClienteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormClientes fBorrar = new FormClientes();
            fBorrar.BorrarClientes();
            fBorrar.Show();
        }

        private void verHojasDeRepartoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormReparto fHDR = new FormReparto();
            fHDR.Show();
        }

        private void verRecibosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormRecibos recibos = new FormRecibos();
            recibos.CargarClientes(listaClientes);
            recibos.Show();
        }

        private void verVentasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormVentas fVentas = new FormVentas();
            fVentas.ObtenerDatos(listaClientes, listaProductos);
            fVentas.Show();
        }

        //BUSCAR CLIENTE
        void FiltrarDatosC(string texto)
        {
            List<Cliente> ListaFiltradaC = new List<Cliente>();
            Cliente[] ArrayC = new Cliente[listaClientes.Count];
            listaClientes.CopyTo(ArrayC);
            for (int i = 0; i < listaClientes.Count; i++)
            {
                string Actual = ArrayC[i].Direccion;
                if (Actual.Contains(texto))
                {
                    ListaFiltradaC.Add(ArrayC[i]);
                }
            }
            dataGridView1.DataSource = ListaFiltradaC.ToArray();
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
            List<Producto> ListaFiltradaP = new List<Producto>();
            Producto[] ArrayP = new Producto[listaProductos.Count];
            listaProductos.CopyTo(ArrayP);
            for (int i = 0; i < listaProductos.Count; i++)
            {
                string Actual = ArrayP[i].Nombre;
                if (Actual.Contains(texto))
                {
                    ListaFiltradaP.Add(ArrayP[i]);
                }
            }
            dataGridView2.DataSource = ListaFiltradaP.ToArray();
        }

        private void button10_Click(object sender, EventArgs e)
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
