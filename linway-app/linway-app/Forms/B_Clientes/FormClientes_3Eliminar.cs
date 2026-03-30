using linway_app.PresentationHelpers;
using linway_app.Services.FormServices;
using linway_app.Services.Interfaces;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.DependencyInjection;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace linway_app.Forms
{
    public partial class FormClientes : Form
    {
        private Cliente _clienteAEliminar;
        private async void BorrarPorId_textBox_TextChanged(object sender, EventArgs ev)  // cliente por Id
        {
            _clienteAEliminar = null;
            string numeroDeCliente = textBox22.Text;
            if (numeroDeCliente == "" || !long.TryParse(textBox22.Text, out long clienteId))
            {
                label47EliminarDireccion.Visible = false;
                label47EliminarDireccion.Text = "";
                button23EliminarCliente.Enabled = false;
                return;
            }
            label47EliminarDireccion.Visible = true;
            Cliente cliente = await UIExecutor.ExecuteAsync(
                _scope,
                async sp => {
                    var clienteServices = sp.GetRequiredService<IClienteServices>();
                    return await clienteServices.GetPorIdAsync(clienteId);
                },
                "No se pudo buscar el Cliente",
                null
            );
            if (cliente == null)
            {
                label47EliminarDireccion.Text = "No encontrado";
                button23EliminarCliente.Enabled = false;
                return;
            }
            _clienteAEliminar = cliente;
            label47EliminarDireccion.Text = cliente.Direccion;
            button23EliminarCliente.Enabled = true;
        }
        private async void BorrarPorDire_textBox_TextChanged(object sender, EventArgs ev)  // cliente por dirección
        {
            _clienteAEliminar = null;
            string direccion = textBoxDireEnBorrar.Text;
            if (direccion == "")
            {
                label47EliminarDireccion.Visible = false;
                label47EliminarDireccion.Text = "";
                button23EliminarCliente.Enabled = false;
                return;
            }
            label47EliminarDireccion.Visible = true;
            Cliente cliente = await UIExecutor.ExecuteAsync(
                _scope,
                async sp => {
                    var clienteServices = sp.GetRequiredService<IClienteServices>();
                    return await clienteServices.GetPorDireccionAsync(direccion);
                },
                "No se pudo buscar el Cliente",
                null
            );
            if (cliente == null)
            {
                label47EliminarDireccion.Text = "No encontrado";
                button23EliminarCliente.Enabled = false;
                return;
            }
            _clienteAEliminar = cliente;
            label47EliminarDireccion.Text = cliente.Direccion;
            button23EliminarCliente.Enabled = true;
        }
        private async void EliminarCliente_Click(object sender, EventArgs ev)
        {
            if (!cbSeguroBorrar.Checked)
            {
                return;
            }
            bool logrado = await UIExecutor.ExecuteAsync(
                _scope,
                async sp => {
                    var savingServices = sp.GetRequiredService<ISavingServices>();
                    var clienteServices = sp.GetRequiredService<IClienteServices>();
                    var detalleReciboServices = sp.GetRequiredService<IDetalleReciboServices>();
                    var notaDeEnvioServices = sp.GetRequiredService<INotaDeEnvioServices>();
                    var pedidoServices = sp.GetRequiredService<IPedidoServices>();
                    var prodVendidoServices = sp.GetRequiredService<IProdVendidoServices>();
                    var reciboServices = sp.GetRequiredService<IReciboServices>();
                    var registroVentaServices = sp.GetRequiredService<IRegistroVentaServices>();
                    var repartoServices = sp.GetRequiredService<IRepartoServices>();
                    //
                    {
                        // se eliminan sus Recibos con sus Detalles
                        List<Recibo> recibos = await reciboServices.GetAllAsync();
                        List<Recibo> recibosDelCliente = recibos.FindAll(r => r.ClienteId == _clienteAEliminar.Id);
                        if (recibosDelCliente.Any())
                        {
                            detalleReciboServices.DeleteMany(recibosDelCliente.SelectMany(r => r.DetalleRecibos).ToList());
                            reciboServices.DeleteMany(recibosDelCliente);
                        }
                    }
                    {
                        // se eliminan sus Notas de Envío con sus Productos Vendidos
                        List<NotaDeEnvio> notas = await notaDeEnvioServices.GetAllAsync();
                        List<NotaDeEnvio> notasDelCliente = notas.FindAll(n => n.ClienteId == _clienteAEliminar.Id);
                        prodVendidoServices.DeleteMany(notasDelCliente.SelectMany(n => n.ProdVendidos).ToList());
                        notaDeEnvioServices.DeleteMany(notasDelCliente);
                    }
                    {
                        // se eliminan sus Pedidos con sus Productos Vendidos, y se actualizan los Repartos a los que correspondan
                        List<Pedido> pedidos = await pedidoServices.GetAllAsync();
                        List<Pedido> pedidosDelCliente = pedidos.FindAll(p => p.ClienteId == _clienteAEliminar.Id);
                        prodVendidoServices.DeleteMany(pedidosDelCliente.SelectMany(p => p.ProdVendidos).ToList());
                        pedidoServices.DeleteMany(pedidosDelCliente);
                        List<Reparto> repartos = await repartoServices.GetAllAsync();
                        List<Reparto> repartosDelCliente = repartos.FindAll(r => r.Pedidos.Any(p => p.ClienteId == _clienteAEliminar.Id));
                        foreach (Reparto reparto in repartosDelCliente)
                        {
                            reparto.Pedidos.ToList().RemoveAll(p => p.ClienteId == _clienteAEliminar.Id);
                            RepartoServices.ActualizarCantidadesDeReparto(reparto);
                            repartoServices.Edit(reparto);
                        }
                    }
                    {
                        // se eliminan sus Registros de Venta con sus Productos Vendidos
                        List<RegistroVenta> registros = await registroVentaServices.GetAllAsync();
                        List<RegistroVenta> registrosDelCliente = registros.FindAll(r => r.ClienteId == _clienteAEliminar.Id);
                        prodVendidoServices.DeleteMany(registrosDelCliente.SelectMany(r => r.ProdVendidos).ToList());
                        registroVentaServices.DeleteMany(registrosDelCliente);
                    }
                    //
                    clienteServices.Delete(_clienteAEliminar);
                    //
                    bool guardado = await savingServices.SaveAsync();
                    if (!guardado)
                    {
                        savingServices.DiscardChanges();
                        MessageBox.Show("No se hicieron cambios");
                    }
                    return guardado;
                },
                "No se pudo eliminar el Cliente",
                this
            );
            if (!logrado)
            {
                return;
            }
            _clienteAEliminar = null;
            textBox22.Text = "";
            textBoxDireEnBorrar.Text = "";
            label47EliminarDireccion.Text = "";
            label47EliminarDireccion.Visible = false;
            button23EliminarCliente.Enabled = false;
            cbSeguroBorrar.Checked = false;
        }
    }
}
