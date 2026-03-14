using linway_app.PresentationHelpers;
using linway_app.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Models;
using Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace linway_app.Forms
{
    public partial class FormClientes : Form
    {
        private Cliente _clienteAEditar;
        private void LimpiarEditar_Click(object sender, EventArgs ev)
        {
            _clienteAEditar = null;
            label23.Text = "";
            radioButton3.Checked = false;
            radioButton4.Checked = false;
            textBox6.Text = "";
            textBox10.Text = "";
            textBox11.Text = "";
            textBox14.Text = "";
            textBox23.Text = "";
            textBox24.Text = "";
            textBox25.Text = "";
        }
        bool TodoOkModificarC()
        {
            //var direccionLabel = label23.Text;
            //var cuit = textBox10.Text;
            //var nombre = textBox11.Text;
            //var telefono = textBox24.Text;
            //var cp = textBox25.Text;
            //var direccion = textBox23.Text;
            return _clienteAEditar != null;
        }
        private void ActualizarEtiquetasDeClienteAEDitar(Cliente cliente)
        {
            if (cliente == null)
            {
                label23.Text = "No encontrado";
                textBox11.Text = "";
                textBox10.Text = "";
                textBox23.Text = "";
                textBox24.Text = "";
                textBox25.Text = "";
                radioButton3.Checked = false;
                radioButton4.Checked = false;
                return;
            }
            label23.Text = cliente.Direccion;
            textBox23.Text = cliente.Direccion;
            textBox24.Text = cliente.Telefono?.ToString();
            textBox25.Text = cliente.CodigoPostal?.ToString();
            textBox11.Text = cliente.Nombre;
            textBox10.Text = cliente.Cuit;
            if (cliente.Tipo == TipoR.Inscripto.ToString())
            {
                radioButton3.Checked = true;
            }
            else
            {
                radioButton4.Checked = true;
            }
        }
        private async void TextBox14_TextChanged(object sender, EventArgs ev)
        {
            _clienteAEditar = null;
            var numeroDeCliente = textBox14.Text;
            if (numeroDeCliente == "")
            {
                label23.Text = "";
                textBox11.Text = "";
                textBox10.Text = "";
                textBox23.Text = "";
                textBox24.Text = "";
                textBox25.Text = "";
                radioButton3.Checked = false;
                radioButton4.Checked = false;
                return;
            }
            if (!long.TryParse(numeroDeCliente, out long clienteId))
            {
                return;
            }
            Cliente cliente = await UIExecutor.ExecuteAsync(
                _scope,
                async sp => {
                    var clienteServices = sp.GetRequiredService<IClienteServices>();
                    return await clienteServices.GetClientePorIdAsync(clienteId);
                },
                "No se pudo buscar el Cliente",
                null
            );
            if (cliente != null)
            {
                _clienteAEditar = cliente;
            }
            ActualizarEtiquetasDeClienteAEDitar(cliente);
        }
        private async void TextBox6_TextChanged(object sender, EventArgs ev)
        {
            _clienteAEditar = null;
            string direccion = textBox6.Text;
            if (direccion == "")
            {
                label23.Text = "";
                textBox11.Text = "";
                textBox10.Text = "";
                textBox23.Text = "";
                textBox24.Text = "";
                textBox25.Text = "";
                radioButton3.Checked = false;
                radioButton4.Checked = false;
                return;
            }
            Cliente cliente = await UIExecutor.ExecuteAsync(
                _scope,
                async sp => {
                    var clienteServices = sp.GetRequiredService<IClienteServices>();
                    return await clienteServices.GetClientePorDireccionAsync(direccion);
                },
                "No se pudo buscar el Cliente",
                null
            );
            if (cliente != null)
            {
                _clienteAEditar = cliente;
            }
            ActualizarEtiquetasDeClienteAEDitar(cliente);
        }
        private async void Editar_Click(object sender, EventArgs ev)
        {
            if (!TodoOkModificarC())
            {
                MessageBox.Show("Verifique que los campos sean correctos");
                return;
            }
            bool logrado = await UIExecutor.ExecuteAsync(
                _scope,
                async sp =>
                {
                    var savingServices = sp.GetRequiredService<ISavingServices>();
                    var clienteServices = sp.GetRequiredService<IClienteServices>();
                    var diaRepartoServices = sp.GetRequiredService<IDiaRepartoServices>();
                    var pedidoServices = sp.GetRequiredService<IPedidoServices>();
                    _clienteAEditar.Direccion = textBox23.Text;
                    _clienteAEditar.Telefono = textBox24.Text;
                    _clienteAEditar.CodigoPostal = textBox25.Text;
                    _clienteAEditar.Nombre = textBox11.Text;
                    _clienteAEditar.Cuit = textBox10.Text;
                    _clienteAEditar.Tipo = radioButton3.Checked ? TipoR.Inscripto.ToString() : TipoR.Monotributo.ToString();
                    clienteServices.EditCliente(_clienteAEditar);
                    List<DiaReparto> dias = await diaRepartoServices.GetDiaRepartosAsync();
                    List<Pedido> pedidosAEditar = dias
                        .SelectMany(dia => dia.Reparto)
                        .SelectMany(reparto => reparto.Pedidos)
                        .Where(pedido => pedido.ClienteId == _clienteAEditar.Id)
                        .ToList();
                    foreach (var pedido in pedidosAEditar)
                    {
                        pedido.Direccion = _clienteAEditar.Direccion;
                    }
                    pedidoServices.EditPedidos(pedidosAEditar);
                    bool guardado = await savingServices.SaveAsync();
                    if (!guardado)
                    {
                        savingServices.DiscardChanges();
                        MessageBox.Show("No se hicieron cambios");
                    }
                    return guardado;
                },
                "No se pudo buscar el cliente, no se modificó o no se pudo actualizar dirección en los Repartos",
                this
            );
            if (!logrado)
            {
                return;
            }
            _clienteAEditar = null;
            button8.PerformClick();
        }
    }
}
