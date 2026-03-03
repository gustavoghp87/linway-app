using linway_app.PresentationHelpers;
using linway_app.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Models;
using System;
using System.Windows.Forms;

namespace linway_app.Forms
{
    public partial class FormRecibos : Form
    {
        private async void ClienteId_TextChanged(object sender, EventArgs ev)
        {
            string numeroDeCliente = textBox6.Text;
            if (numeroDeCliente == "")
            {
                label15.Text = "";
                button6.Enabled = false;
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
            if (cliente == null)
            {
                label15.Text = "No encontrado";
                button6.Enabled = false;
                return;
            }
            label15.Text = cliente.Direccion;
            if (_lstDetallesAAgregar.Count != 0)
            {
                button6.Enabled = true;
            }
        }
        private async void TextBox9_TextChanged(object sender, EventArgs ev)
        {
            string direccion = textBox9.Text;
            if (direccion == "")
            {
                label15.Text = "";
                button6.Enabled = false;
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
            if (cliente == null)
            {
                label15.Text = "No encontrado";
                button6.Enabled = false;
                return;
            }
            label15.Text = cliente.Direccion;
            if (_lstDetallesAAgregar.Count != 0)
            {
                button6.Enabled = true;
            }
        }
        private bool AlgunDetSeleccionado()
        {
            return radioButton1.Checked || radioButton2.Checked || radioButton3.Checked || radioButton4.Checked;
        }
        private void AgregarDetalle_Click(object sender, EventArgs ev)
        {
            if (textBox8.Text == "" || !AlgunDetSeleccionado())
            {
                MessageBox.Show("Completar correctamente los campos");
                return;
            }
            if (!decimal.TryParse(textBox8.Text, out decimal importe))
            {
                return;
            }
            var nuevoDetalle = new DetalleRecibo();
            if (radioButton1.Checked)
            {
                nuevoDetalle.Detalle = "Saldo a Favor";
                nuevoDetalle.Importe = importe * -1;
            }
            if (radioButton2.Checked)
            {
                nuevoDetalle.Detalle = "Desc. por devol.";
                nuevoDetalle.Importe = importe * -1;
            }
            if (radioButton3.Checked)
            {
                nuevoDetalle.Detalle = "Saldo pendiente";
                nuevoDetalle.Importe = importe;
            }
            if (radioButton4.Checked)
            {
                nuevoDetalle.Detalle = "Factura N° " + textBox7.Text;
                nuevoDetalle.Importe = importe;
            }
            _lstDetallesAAgregar.Add(nuevoDetalle);
            ActualizarGridDetalles();
            _subTo = 0;
            foreach (DetalleRecibo recibo in _lstDetallesAAgregar)
            {
                _subTo += recibo.Importe;
            }
            label18.Text = _subTo.ToString();
            LimpiarCampos();
            if (label15.Text != "" && label15.Text != "No encontrado")
            {
                button6.Enabled = true;
            }
        }
        private void Limpiar_Click(object sender, EventArgs ev)
        {
            LimpiarCampos();
            textBox6.Text = "";
            textBox8.Text = "";
            textBox9.Text = "";
            label15.Text = "";
            button6.Enabled = false;
            label18.Text = "0";
            _lstDetallesAAgregar.Clear();
            ActualizarGridDetalles();
        }
        private async void CrearRecibo_Click(object sender, EventArgs ev)
        {
            await CargarRecibos();
            string direccion = label15.Text;
            bool logrado = await UIExecutor.ExecuteAsync(
                _scope,
                async sp =>
                {
                    var savingServices = sp.GetRequiredService<ISavingServices>();
                    var clienteService = sp.GetRequiredService<IClienteServices>();
                    Cliente cliente = await clienteService.GetClientePorDireccionExactaAsync(direccion);
                    var reciboService = sp.GetRequiredService<IReciboServices>();
                    var detalleReciboService = sp.GetRequiredService<IDetalleReciboServices>();
                    if (cliente == null)
                    {
                        return false;
                    }
                    var nuevoRecibo = new Recibo
                    {
                        ClienteId = cliente.Id,
                        DireccionCliente = label15.Text,
                        ImporteTotal = _subTo,
                        Impreso = 0,
                        Fecha = DateTime.Now.ToString(Constants.FormatoDeFecha)
                    };
                    reciboService.AddRecibo(nuevoRecibo);
                    foreach (DetalleRecibo detalle in _lstDetallesAAgregar)
                    {
                        detalle.Recibo = nuevoRecibo;
                    }
                    detalleReciboService.AddDetalles(_lstDetallesAAgregar);
                    return await savingServices.SaveAsync();
                },
                "No se pudo agregar el Recibo",
                this
            );
            if (!logrado)
            {
                return;
            }
            LimpiarCampos();
            textBox6.Text = "";
            textBox8.Text = "";
            label15.Text = "";
            button6.Enabled = false;
            label18.Text = "0";
            _lstDetallesAAgregar.Clear();
            await Actualizar();
        }
    }
}
