using AutoMapper;
using linway_app.PresentationHelpers;
using linway_app.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;
//using static linway_app.Services.Delegates.AutoBackup;

namespace linway_app.Forms
{
    public partial class Form1 : Form
    {
        public static IMapper Mapper;
        private List<Cliente> _lstClientes;
        private List<Producto> _lstProductos;
        private IServiceScope _scope;  // scope por formulario para que no se pierdan los atributos generados por Lazy Loading
        public Form1(IMapper mapper)
        {
            Mapper = mapper;
            try {
                InitializeComponent();
            }
            catch (Exception e)
            {
                Logger.LogException(e);
                MessageBox.Show(e.Message);
                return;
            }
            _scope = Program.LinwayServiceProvider.CreateScope();
            FormClosed += (s, e) => _scope.Dispose();
        }
        private async void Form1_Load(object sender, EventArgs ev)
        {
            await Actualizar();
            //generateDbBackup();
        }
        private async Task Actualizar()
        {
            var respuesta = await UIExecutor.ExecuteAsync(
                _scope,
                async sp =>
                {
                    var clienteServices = sp.GetRequiredService<IClienteServices>();
                    var productoServices = sp.GetRequiredService<IProductoServices>();
                    List<Producto> productos = await productoServices.GetProductosAsync();
                    List<Cliente> clientes = await clienteServices.GetClientesAsync();
                    return (clientes, productos);
                },
                "No se pudieron buscar los Clientes y los Productos",
                null
            );
            if (respuesta.clientes == null || respuesta.productos == null)
            {
                return;
            }
            _lstClientes = respuesta.clientes;
            _lstProductos = respuesta.productos;
            CargarGridClientes(_lstClientes);
            CargarGridProductos(_lstProductos);
            FiltrarDatosC(BuscadorClientes.Text);
            FiltrarDatosP(BuscadorProductos.Text);
        }
        private async void Frm_FormClosing(object sender, FormClosingEventArgs ev)
        {
            _scope.Dispose();
            _scope = Program.LinwayServiceProvider.CreateScope();  // renueva scope para que tome los cambios hechos en otros scopes
            await Actualizar();
        }
        private void AbrirClientes_ToolStripMenuItem_Click(object sender, EventArgs ev)
        {
            var form = Program.LinwayServiceProvider.GetRequiredService<FormClientes>();
            form.FormClosing += Frm_FormClosing;
            form.Show();
        }
        private void AbrirProductos_ToolStripMenuItem_Click(object sender, EventArgs ev)
        {
            var form = Program.LinwayServiceProvider.GetRequiredService<FormProductos>();
            form.FormClosing += Frm_FormClosing;
            form.Show();
        }
        private void AbrirCrearNotaDeEnvío_ToolStripMenuItem_Click(object sender, EventArgs ev)
        {
            var form = Program.LinwayServiceProvider.GetRequiredService<FormCrearNota>();
            form.FormClosing += Frm_FormClosing;
            form.Show();
        }
        private void AbrirNotasDeEnvio_ToolStripMenuItem_Click(object sender, EventArgs ev)
        {
            var form = Program.LinwayServiceProvider.GetRequiredService<FormNotasEnvio>();
            form.FormClosing += Frm_FormClosing;
            form.Show();
        }
        private void AbrirHojasDeReparto_ToolStripMenuItem_Click(object sender, EventArgs ev)
        {
            var form = Program.LinwayServiceProvider.GetRequiredService<FormRepartos>();
            form.FormClosing += Frm_FormClosing;
            form.Show();
        }
        private void AbrirRecibos_ToolStripMenuItem_Click(object sender, EventArgs ev)
        {
            var form = Program.LinwayServiceProvider.GetRequiredService<FormRecibos>();
            form.FormClosing += Frm_FormClosing;
            form.Show();
        }
        private void AbrirVentas_ToolStripMenuItem_Click(object sender, EventArgs ev)
        {
            var form = Program.LinwayServiceProvider.GetRequiredService<FormVentas>();
            form.FormClosing += Frm_FormClosing;
            form.Show();
        }
    }
}
