using AppLinway.PresentationHelpers;
using AppServices.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Models;
using Models.Entities;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace AppLinway.Forms
{
    public partial class FormRecibos : Form
    {
        private Cliente _clienteAgregarRecibo;
        private readonly List<DetalleRecibo> _lstDetallesAAgregar = new List<DetalleRecibo>();
        private decimal _subTo = 0;
        private void ActualizarGridDetalles()
        {
            var grid2 = new List<EDetalleRecibo>();
            foreach (DetalleRecibo detalleRecibo in _lstDetallesAAgregar)
            {
                grid2.Add(Form1.Mapper.Map<EDetalleRecibo>(detalleRecibo));
            }
            dataGridView2.DataSource = grid2;
            dataGridView2.Columns[0].Width = 140;
            dataGridView2.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight;
        }
        private async void ClienteId_TextChanged(object sender, EventArgs ev)  // cliente por Id
        {
            _clienteAgregarRecibo = null;
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
            var cliente = await UIExecutor.ExecuteAsync(
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
                label15.Text = "No encontrado";
                button6.Enabled = false;
                return;
            }
            _clienteAgregarRecibo = cliente;
            label15.Text = cliente.Direccion;
            if (_lstDetallesAAgregar.Count != 0)
            {
                button6.Enabled = true;
            }
        }
        private async void TextBox9_TextChanged(object sender, EventArgs ev)  // cliente por direccion
        {
            _clienteAgregarRecibo = null;
            string direccion = textBox9.Text;
            if (direccion == "")
            {
                label15.Text = "";
                button6.Enabled = false;
                return;
            }
            var cliente = await UIExecutor.ExecuteAsync(
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
                label15.Text = "No encontrado";
                button6.Enabled = false;
                return;
            }
            _clienteAgregarRecibo = cliente;
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
            if (_clienteAgregarRecibo == null)
            {
                MessageBox.Show("Seleccionar un Cliente primero");
                return;
            }
            if (_lstDetallesAAgregar.Count == 0)
            {
                MessageBox.Show("Agregar al menos un Detalle");
                return;
            }
            await CargarRecibos();
            var resultado = await UIExecutor.ExecuteAsync(
                _scope,
                async sp =>
                {
                    var useCase = _scope.ServiceProvider.GetRequiredService<IAgregarReciboUseCase>();
                    return await useCase.ExecuteAsync(_clienteAgregarRecibo, _lstDetallesAAgregar);
                },
                "No se pudo agregar el Recibo",
                this
            );
            if (resultado == null || !resultado.Success)
            {
                if (resultado?.ErrorMessage != null)
                {
                    MessageBox.Show(resultado.ErrorMessage);
                }
                return;
            }
            LimpiarCampos();
            textBox6.Text = "";
            textBox8.Text = "";
            label15.Text = "";
            button6.Enabled = false;
            label18.Text = "0";
            _lstDetallesAAgregar.Clear();
            _subTo = 0;
            await Actualizar();
            _clienteAgregarRecibo = null;
        }
    }
}
