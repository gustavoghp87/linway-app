using AppLinway.PresentationHelpers;
using AppServices.EntityServices;
using AppServices.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace AppLinway.Forms
{
    public partial class FormRepartos : Form
    {
        private Cliente _clienteAReparto;
        private void ComboBox4_SelectedIndexChanged(object sender, EventArgs ev)
        {
            string diaReparto = comboBox4AgregarPedidido_Dia.SelectedItem.ToString();
            List<Reparto> repartos = _lstDiaRepartos.Find(x => x.Dia == diaReparto).Repartos.ToList();
            comboBox5AgregarPedido_Nombre.DataSource = repartos;
            comboBox5AgregarPedido_Nombre.DisplayMember = "Nombre";
            comboBox5AgregarPedido_Nombre.ValueMember = "Nombre";
        }
        private void Button4_Click(object sender, EventArgs ev)
        {
            LimpiarPantalla();
        }
        private void TextBox2_KeyPress(object sender, KeyPressEventArgs ev)
        {
            if (!char.IsNumber(ev.KeyChar) && ev.KeyChar != (char)Keys.Back)
            {
                ev.Handled = true;
            }
        }
        private async void TextBox2_TextChanged(object sender, EventArgs ev)  // cliente por Id
        {
            _clienteAReparto = null;
            string numeroDeCliente = textBox2.Text;
            if (numeroDeCliente == "")
            {
                label8AgregarPedidoDireccion.Text = "No encontrado";
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
            label8AgregarPedidoDireccion.Text = cliente != null ? cliente.Direccion : "No encontrado";
            if (cliente == null)
            {
                return;
            }
            _clienteAReparto = cliente;
        }
        private async void TextBox6_TextChanged(object sender, EventArgs ev)  // cliente por dirección
        {
            _clienteAReparto = null;
            string direccion = textBox6.Text;
            if (direccion == "")
            {
                label8AgregarPedidoDireccion.Text = "No encontrado";
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
            label8AgregarPedidoDireccion.Text = cliente != null ? cliente.Direccion : "No encontrado";
            if (cliente == null)
            {
                return;
            }
            _clienteAReparto = cliente;
        }
        private async void AgregarDestinoAReparto_btn1_Click(object sender, EventArgs ev)
        {
            string diaReparto = comboBox4AgregarPedidido_Dia.Text;
            string direccion = label8AgregarPedidoDireccion.Text;
            if (_clienteAReparto == null || diaReparto == "")
            {
                MessageBox.Show("Error, verificar los campos");
                return;
            }
            string nombreReparto = comboBox5AgregarPedido_Nombre.Text;
            //await ReCargarHDR(diaReparto, nombreReparto);
            //comboBox1.SelectedIndex = comboBox4.SelectedIndex;
            //comboBox2.SelectedIndex = comboBox5.SelectedIndex;
            Reparto reparto = _lstDiaRepartos
                .Find(x => x.Dia == diaReparto).Repartos.ToList()
                .Find(x => x.Nombre == nombreReparto);
            if (_lstPedidos.Exists(x => x.ClienteId == _clienteAReparto.Id && x.RepartoId == reparto.Id))
            {
                MessageBox.Show("Ese cliente ya estaba en el Reparto");
                return;
            }
            var pedido = PedidoServices.GetNuevoPedido(_clienteAReparto, reparto);
            var resultado = await UIExecutor.ExecuteAsync(
                _scope,
                async sp => {
                    var useCase = _scope.ServiceProvider.GetRequiredService<IAgregarPedidoUseCase>();
                    return await useCase.ExecuteAsync(pedido);
                },
                "No se pudo realizar",
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
            LimpiarPantalla();
            await Actualizar();
            _clienteAReparto = null;
        }
    }
}
