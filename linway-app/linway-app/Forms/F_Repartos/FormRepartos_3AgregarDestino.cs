using linway_app.PresentationHelpers;
using linway_app.Services.FormServices;
using linway_app.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace linway_app.Forms
{
    public partial class FormRepartos : Form
    {
        private Cliente _clienteAReparto;
        private async void ComboBox4_SelectedIndexChanged(object sender, EventArgs ev)
        {
            string diaReparto = comboBox4.SelectedItem.ToString();
            List<Reparto> repartos = await UIExecutor.ExecuteAsync(
                _scope,
                async sp =>
                {
                    var diaRepartoServices = sp.GetRequiredService<IDiaRepartoServices>();
                    List<DiaReparto> lstDiasRep = await diaRepartoServices.GetDiaRepartosAsync();
                    return lstDiasRep.Find(x => x.Dia == diaReparto && x.Estado != null && x.Estado != "Eliminado").Reparto.ToList();
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
            _clienteAReparto = null;
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
            if (cliente == null)
            {
                return;
            }
            _clienteAReparto = cliente;
        }
        private async void TextBox6_TextChanged(object sender, EventArgs ev)
        {
            _clienteAReparto = null;
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
            if (cliente == null)
            {
                return;
            }
            _clienteAReparto = cliente;
        }
        private async void AgregarDestinoAReparto_btn1_Click(object sender, EventArgs ev)
        {
            string diaReparto = comboBox4.Text;
            string direccion = label8.Text;
            if (_clienteAReparto == null || diaReparto == "")
            {
                MessageBox.Show("Error, verificar los campos");
                return;
            }
            string nombreReparto = comboBox5.Text;
            await ReCargarHDR(diaReparto, nombreReparto);
            //comboBox1.SelectedIndex = comboBox4.SelectedIndex;
            //comboBox2.SelectedIndex = comboBox5.SelectedIndex;
            bool logrado = await UIExecutor.ExecuteAsync(
                _scope,
                async sp => {
                    var savingServices = sp.GetRequiredService<ISavingServices>();
                    var diaRepartoServices = sp.GetRequiredService<IDiaRepartoServices>();
                    var pedidoServices = sp.GetRequiredService<IPedidoServices>();
                    List<DiaReparto> lstDiasRep = await diaRepartoServices.GetDiaRepartosAsync();
                    Reparto reparto = lstDiasRep
                        .Find(x => x.Dia == diaReparto && x.Estado != null && x.Estado != "Eliminado").Reparto.ToList()
                        .Find(x => x.Nombre == nombreReparto && x.Estado != null && x.Estado != "Eliminado") ?? throw new Exception("No se pudo encontrar el Reparto");
                    if (_lstPedidos.Exists(x => x.ClienteId == _clienteAReparto.Id && x.RepartoId == reparto.Id))
                    {
                        savingServices.DiscardChanges();
                        MessageBox.Show("Ese cliente ya estaba en el Reparto");
                        return false;
                    }
                    var pedido = PedidoServices.CrearPedido(_clienteAReparto, reparto);
                    await pedidoServices.AddPedidoAsync(pedido);
                    bool guardado = await savingServices.SaveAsync();
                    if (!guardado)
                    {
                        savingServices.DiscardChanges();
                        MessageBox.Show("No se hicieron cambios");
                    }
                    return guardado;
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
            await ActualizarCombobox1();
            await UpdateGrid();
            _clienteAReparto = null;
        }
    }
}
