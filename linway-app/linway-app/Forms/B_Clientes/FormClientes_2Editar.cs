using linway_app.PresentationHelpers;
using linway_app.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Models;
using Models.Enums;
using System;
using System.Windows.Forms;

namespace linway_app.Forms
{
    public partial class FormClientes : Form
    {
        private void LimpiarEditar_Click(object sender, EventArgs ev)
        {
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
            var direccion = textBox23.Text;
            return !string.IsNullOrEmpty(direccion) && direccion != "No encontrado";
        }
        private void DoIt(Cliente cliente)
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
            DoIt(cliente);
        }
        private async void TextBox6_TextChanged(object sender, EventArgs ev)
        {
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
            DoIt(cliente);
        }
        private async void Editar_Click(object sender, EventArgs ev)
        {
            if (!TodoOkModificarC())
            {
                MessageBox.Show("Verifique que los campos sean correctos");
                return;
            }
            string direccion = label23.Text;
            bool logrado = await UIExecutor.ExecuteAsync(
                _scope,
                async sp =>
                {
                    var savingServices = sp.GetRequiredService<ISavingServices>();
                    var clienteServices = sp.GetRequiredService<IClienteServices>();
                    var orquestacionServices = sp.GetRequiredService<IOrquestacionServices>();
                    Cliente cliente = await clienteServices.GetClientePorDireccionExactaAsync(direccion);
                    if (cliente == null)
                    {
                        return false;  // no debería pasar por el chequeo previo
                    }
                    cliente.Direccion = textBox23.Text;
                    cliente.Telefono = textBox24.Text;
                    cliente.CodigoPostal = textBox25.Text;
                    cliente.Nombre = textBox11.Text;
                    cliente.Cuit = textBox10.Text;
                    if (radioButton3.Checked)
                    {
                        cliente.Tipo = TipoR.Inscripto.ToString();
                    }
                    else
                    {
                        cliente.Tipo = TipoR.Monotributo.ToString();
                    }
                    await orquestacionServices.EditClienteYDireccionEnPedidosAsync(cliente);
                    return await savingServices.SaveAsync();
                },
                "No se pudo buscar el cliente, no se modificó o no se pudo actualizar dirección en los Repartos",
                this
            );
            if (!logrado)
            {
                return;
            }
            button8.PerformClick();
        }
    }
}
