using linway_app.PresentationHelpers;
using linway_app.Services.FormServices;
using Microsoft.EntityFrameworkCore.Internal;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
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
                    var servicesContext = ServiceContext.Get(sp);
                    return await servicesContext.ClienteServices.GetPorIdAsync(clienteId);
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
                    var servicesContext = ServiceContext.Get(sp);
                    return await servicesContext.ClienteServices.GetPorDireccionAsync(direccion);
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
                    var servicesContext = ServiceContext.Get(sp);
                    //
                    {
                        // se eliminan sus Recibos con sus Detalles
                        List<Recibo> recibos = await servicesContext.ReciboServices.GetAllAsync();
                        List<Recibo> recibosDelCliente = recibos.FindAll(r => r.ClienteId == _clienteAEliminar.Id);
                        if (recibosDelCliente.Any())
                        {
                            servicesContext.DetalleReciboServices.DeleteMany(recibosDelCliente.SelectMany(r => r.DetalleRecibos).ToList());
                            servicesContext.ReciboServices.DeleteMany(recibosDelCliente);
                        }
                    }
                    {
                        // se eliminan sus Notas de Envío con sus Productos Vendidos
                        List<NotaDeEnvio> notas = await servicesContext.NotaDeEnvioServices.GetAllAsync();
                        List<NotaDeEnvio> notasDelCliente = notas.FindAll(n => n.ClienteId == _clienteAEliminar.Id);
                        servicesContext.ProdVendidoServices.DeleteMany(notasDelCliente.SelectMany(n => n.ProdVendidos).ToList());
                        servicesContext.NotaDeEnvioServices.DeleteMany(notasDelCliente);
                    }
                    {
                        // se eliminan sus Pedidos con sus Productos Vendidos, y se actualizan los Repartos a los que correspondan
                        List<Pedido> pedidos = await servicesContext.PedidoServices.GetAllAsync();
                        List<Pedido> pedidosDelCliente = pedidos.FindAll(p => p.ClienteId == _clienteAEliminar.Id);
                        servicesContext.ProdVendidoServices.DeleteMany(pedidosDelCliente.SelectMany(p => p.ProdVendidos).ToList());
                        servicesContext.PedidoServices.DeleteMany(pedidosDelCliente);
                        List<Reparto> repartos = await servicesContext.RepartoServices.GetAllAsync();
                        List<Reparto> repartosDelCliente = repartos.FindAll(r => r.Pedidos.Any(p => p.ClienteId == _clienteAEliminar.Id));
                        foreach (Reparto reparto in repartosDelCliente)
                        {
                            reparto.Pedidos.ToList().RemoveAll(p => p.ClienteId == _clienteAEliminar.Id);
                            RepartoServices.ActualizarCantidadesDeReparto(reparto);
                            servicesContext.RepartoServices.Edit(reparto);
                        }
                    }
                    {
                        // se eliminan sus Registros de Venta con sus Productos Vendidos
                        List<RegistroVenta> registros = await servicesContext.RegistroVentaServices.GetAllAsync();
                        List<RegistroVenta> registrosDelCliente = registros.FindAll(r => r.ClienteId == _clienteAEliminar.Id);
                        servicesContext.ProdVendidoServices.DeleteMany(registrosDelCliente.SelectMany(r => r.ProdVendidos).ToList());
                        servicesContext.RegistroVentaServices.DeleteMany(registrosDelCliente);
                    }
                    //
                    servicesContext.ClienteServices.Delete(_clienteAEliminar);
                    //
                    bool guardado = await servicesContext.SavingServices.SaveAsync();
                    if (!guardado)
                    {
                        servicesContext.SavingServices.DiscardChanges();
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
