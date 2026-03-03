using linway_app.PresentationHelpers;
using linway_app.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Models;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace linway_app.Forms
{
    public partial class FormRepartos : Form
    {
        private async void ComboBox4_SelectedIndexChanged(object sender, EventArgs ev)
        {
            string dia = comboBox4.SelectedItem.ToString();
            List<Reparto> repartos = await UIExecutor.ExecuteAsync(
                _scope,
                async sp =>
                {
                    var orquestacionServices = sp.GetRequiredService<IOrquestacionServices>();
                    return await orquestacionServices.GetRepartosPorDiaAsync(dia);
                },
                "No se pudieron buscar los Repartos por Día",
                null
            );
            if (repartos == null)
            {
                return;
            }
            comboBox5.DataSource = repartos;
            comboBox5.DisplayMember = "Nombre";
            comboBox5.ValueMember = "Nombre";
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
        private async void TextBox2_TextChanged(object sender, EventArgs ev)  // Agregar destino a recorrido
        {
            string numeroDeCliente = textBox2.Text;
            if (numeroDeCliente == "")
            {
                label8.Text = "No encontrado";
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
            label8.Text = cliente != null ? cliente.Direccion : "No encontrado";
        }
        private async void TextBox6_TextChanged(object sender, EventArgs ev)
        {
            string direccion = textBox6.Text;
            if (direccion == "")
            {
                label8.Text = "No encontrado";
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
            label8.Text = cliente != null ? cliente.Direccion : "No encontrado";
        }
        private async void AgregarDestinoAReparto_btn1_Click(object sender, EventArgs ev)
        {
            if (label8.Text == "No encontrado" || comboBox4.Text == "")
            {
                MessageBox.Show("Error, verificar los campos");
                return;
            }
            string dia = comboBox4.Text;
            string nombre = comboBox5.Text;
            string direccion = label8.Text;
            await ReCargarHDR(comboBox4.Text, comboBox5.Text);                // día y reparto
            //comboBox1.SelectedIndex = comboBox4.SelectedIndex;
            //comboBox2.SelectedIndex = comboBox5.SelectedIndex;
            bool logrado = await UIExecutor.ExecuteAsync(
                _scope,
                async sp => {
                    var savingServices = sp.GetRequiredService<ISavingServices>();
                    var clienteServices = sp.GetRequiredService<IClienteServices>();
                    var orquestacionServices = sp.GetRequiredService<IOrquestacionServices>();
                    var pedidoServices = sp.GetRequiredService<IPedidoServices>();
                    Cliente cliente = await clienteServices.GetClientePorDireccionExactaAsync(direccion);
                    if (cliente == null)
                    {
                        throw new Exception("No se pudo encontrar el Cliente");  // no debería pasar por el chequeo previo
                    }
                    Reparto reparto = await orquestacionServices.GetRepartoPorDiaYNombreAsync(dia, nombre);
                    if (reparto == null)
                    {
                        throw new Exception("No se pudo encontrar el Reparto");
                    }
                    if (_lstPedidos.Exists(x => x.ClienteId == cliente.Id && x.RepartoId == reparto.Id))
                    {
                        MessageBox.Show("Ese cliente ya estaba en el Reparto");
                        return false;
                    };
                    Pedido pedido = await orquestacionServices.GetPedidoPorRepartoYClienteGenerarSiNoExisteAsync(reparto.Id, cliente.Id);
                    pedido.Entregar = 1;
                    pedidoServices.AddPedido(pedido);
                    return await savingServices.SaveAsync();
                },
                "No se pudo realizar",
                this
            );
            if (!logrado)
            {
                return;
            }
            LimpiarPantalla();
            await Actualizar();
        }
    }
}
