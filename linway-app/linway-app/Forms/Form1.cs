using AutoMapper;
using linway_app.Models;
using linway_app.Models.Entities;
using linway_app.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using static linway_app.Services.Delegates.DCliente;
using static linway_app.Services.Delegates.DProducto;

namespace linway_app.Forms
{
    public partial class Form1 : Form
    {
        public static IServiceBase<Cliente> _servCliente;
        public static IServiceBase<DetalleRecibo> _servDetalleRecibo;
        public static IServiceBase<DiaReparto> _servDiaReparto;
        public static IServiceBase<NotaDeEnvio> _servNotaDeEnvio;
        public static IServiceBase<Pedido> _servPedido;
        public static IServiceBase<Producto> _servProducto;
        public static IServiceBase<ProdVendido> _servProdVendido;
        public static IServiceBase<Recibo> _servRecibo;
        public static IServiceBase<RegistroVenta> _servRegistroVenta;
        public static IServiceBase<Reparto> _servReparto;
        public static IServiceBase<Venta> _servVenta;
        public static IMapper _mapper;
        public Form1(
            IServiceBase<Cliente> servCliente,
            IServiceBase<DetalleRecibo> servDetalleRecibo,
            IServiceBase<DiaReparto> servDiaReparto,
            IServiceBase<NotaDeEnvio> servNotaDeEnvio,
            IServiceBase<Pedido> servPedido,
            IServiceBase<Producto> servProducto,
            IServiceBase<ProdVendido> servProdVendido,
            IServiceBase<Recibo> servRecibo,
            IServiceBase<RegistroVenta> servRegistroVenta,
            IServiceBase<Reparto> servReparto,
            IServiceBase<Venta> servVenta,
            IMapper mapper
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
            _mapper = mapper;
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
            if (lstClientes != null)
            {
                List<ECliente> grid = new List<ECliente>();
                foreach (Cliente cliente in lstClientes)
                {
                    grid.Add(_mapper.Map<ECliente>(cliente));
                }
                dataGridView1.DataSource = grid.ToArray();
                dataGridView1.Columns[0].Width = 40;
                dataGridView1.Columns[1].Width = 350;
                dataGridView1.Columns[2].Width = 120;
                dataGridView1.Columns[3].Width = 40;
                dataGridView1.Columns[4].Width = 90;
                dataGridView1.Columns[5].Width = 65;
                dataGridView1.Columns[6].Width = 65;
            }
            else
            {
                CrearPrimerCliente();
                Actualizar();
            }
        }
        private void CargarGrid2(List<Producto> lstProductos)
        {
            if (lstProductos != null)
            {
                List<EProducto> grid = new List<EProducto>();
                foreach (Producto producto in lstProductos)
                {
                    grid.Add(_mapper.Map<EProducto>(producto));
                }
                dataGridView2.DataSource = grid;
                dataGridView2.Columns[0].Width = 48;
                dataGridView2.Columns[1].Width = 220;
                dataGridView2.Columns[2].Width = 72;
            }
        }
        private void CrearPrimerCliente()
        {
            try
            {
                bool response = addClientePrimero();
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
            var form = Program.GetConfig().GetRequiredService<FormRepartos>();
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
            List<Cliente> lstClientes = getClientes();
            List<Cliente> lstClientesFiltrados = new List<Cliente>();
            foreach (var cliente in lstClientes)
            {
                if (cliente.Direccion.ToLower().Contains(texto.ToLower()))
                {
                    lstClientesFiltrados.Add(cliente);
                }
            }
            CargarGrid1(lstClientesFiltrados);
        }
        private void TextBox8_TextChanged(object sender, EventArgs e)
        {
            FiltrarDatosC(BuscadorClientes.Text);
        }
        private void Button5_Click(object sender, EventArgs e)
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
                if (producto.Nombre.ToLower().Contains(texto.ToLower())) lstProductosFilstrados.Add(producto);
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
