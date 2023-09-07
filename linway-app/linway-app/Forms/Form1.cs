using AutoMapper;
using InfrasServices.Services;
using linway_app.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Models;
using Models.Entities;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using static linway_app.Services.Delegates.DCliente;
//using static linway_app.Services.Delegates.AutoBackup;
using static linway_app.Services.Delegates.DProducto;

namespace linway_app.Forms
{
    public partial class Form1 : Form
    {
        public static IMapper mapper;
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
            ServicesObjects.ServCliente = servCliente;
            ServicesObjects.ServDetalleRecibo = servDetalleRecibo;
            ServicesObjects.ServDiaReparto = servDiaReparto;
            ServicesObjects.ServNotaDeEnvio = servNotaDeEnvio;
            ServicesObjects.ServPedido = servPedido;
            ServicesObjects.ServProducto = servProducto;
            ServicesObjects.ServProdVendido = servProdVendido;
            ServicesObjects.ServRecibo = servRecibo;
            ServicesObjects.ServRegistroVenta = servRegistroVenta;
            ServicesObjects.ServReparto = servReparto;
            ServicesObjects.ServVenta = servVenta;
            Form1.mapper = mapper;
            try {
                InitializeComponent();
            }
            catch (Exception e)
            {
                Logger.LogException(e);
                MessageBox.Show(e.Message);
                return;
            }
        }
        private void Form1_Load(object sender, EventArgs ev)
        {
            Actualizar();
            //generateDbBackup();
        }
        public void Actualizar()
        {
            List<Cliente> lstClientes = getClientes();
            CargarGridClientes(lstClientes);
            List<Producto> lstProductos = getProductos();
            CargarGridProductos(lstProductos);
        }
        private void CargarGridClientes(ICollection<Cliente> lstClientes)
        {
            if (lstClientes == null)
            {
                CrearPrimerCliente();
                Actualizar();
                return;
            }
            var grid = new List<ECliente>();
            foreach (Cliente cliente in lstClientes)
            {
                grid.Add(mapper.Map<ECliente>(cliente));
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
        private void CargarGridProductos(ICollection<Producto> lstProductos)
        {
            if (lstProductos == null) return;
            var grid = new List<EProducto>();
            foreach (Producto producto in lstProductos)
            {
                grid.Add(mapper.Map<EProducto>(producto));
            }
            dataGridView2.DataSource = grid;
            dataGridView2.Columns[0].Width = 48;
            dataGridView2.Columns[1].Width = 270;
            dataGridView2.Columns[2].Width = 72;
        }
        private void CrearPrimerCliente()
        {
            try
            {
                bool success = addClientePrimero();
                if (!success)
                {
                    MessageBox.Show("Hay problemas con la base de datos y no se puede seguir");
                    Close();
                }
            }
            catch (Exception e)
            {
                Logger.LogException(e);
                MessageBox.Show("Hay problemas con la base de datos y no se puede seguir: " + e.Message);
                Close();
            }
        }


        // MENUES
        void Frm_FormClosing(object sender, FormClosingEventArgs ev)
        {
            Actualizar();
        }
        private void AbrirClientes_ToolStripMenuItem_Click(object sender, EventArgs ev)
        {
            var form = Program.GetConfig().GetRequiredService<FormClientes>();
            form.FormClosing += Frm_FormClosing;
            form.Show();
        }
        private void AbrirProductos_ToolStripMenuItem_Click(object sender, EventArgs ev)
        {
            var form = Program.GetConfig().GetRequiredService<FormProductos>();
            form.FormClosing += Frm_FormClosing;
            form.Show();
        }
        private void AbrirCrearNotaDeEnvío_ToolStripMenuItem_Click(object sender, EventArgs ev)
        {
            var form = Program.GetConfig().GetRequiredService<FormCrearNota>();
            form.FormClosing += Frm_FormClosing;
            form.Show();
        }
        private void AbrirNotasDeEnvio_ToolStripMenuItem_Click(object sender, EventArgs ev)
        {
            var form = Program.GetConfig().GetRequiredService<FormNotasEnvio>();
            form.FormClosing += Frm_FormClosing;
            form.Show();
        }
        private void AbrirHojasDeReparto_ToolStripMenuItem_Click(object sender, EventArgs ev)
        {
            var form = Program.GetConfig().GetRequiredService<FormRepartos>();
            form.FormClosing += Frm_FormClosing;
            form.Show();
        }
        private void AbrirRecibos_ToolStripMenuItem_Click(object sender, EventArgs ev)
        {
            var form = Program.GetConfig().GetRequiredService<FormRecibos>();
            form.FormClosing += Frm_FormClosing;
            form.Show();
        }
        private void AbrirVentas_ToolStripMenuItem_Click(object sender, EventArgs ev)
        {
            var form = Program.GetConfig().GetRequiredService<FormVentas>();
            form.FormClosing += Frm_FormClosing;
            form.Show();
        }



        //BUSCAR CLIENTE
        void FiltrarDatosC(string texto)                      // filtra por dirección de clientes
        {
            List<Cliente> lstClientes = getClientes();
            if (lstClientes == null) return;
            var lstClientesFiltrados = new List<Cliente>();
            foreach (var cliente in lstClientes)
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

        //BUSCAR PRODUCTO
        void FiltrarDatosP(string texto)
        {
            List<Producto> lstProductos = getProductos();
            if (lstProductos == null) return;
            var lstProductosFilstrados = new List<Producto>();
            foreach (var producto in lstProductos)
            {
                if (producto.Nombre.ToLower().Contains(texto.ToLower())) lstProductosFilstrados.Add(producto);
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
