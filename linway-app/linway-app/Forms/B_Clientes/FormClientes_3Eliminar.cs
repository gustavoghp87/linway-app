using linway_app.PresentationHelpers;
using linway_app.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Models;
using System;
using System.Windows.Forms;

namespace linway_app.Forms
{
    public partial class FormClientes : Form
    {
        private async void BorrarPorId_textBox_TextChanged(object sender, EventArgs ev)
        {
            string numeroDeCliente = textBox22.Text;
            if (numeroDeCliente == "" || !long.TryParse(textBox22.Text, out long clienteId))
            {
                label47.Visible = false;
                label47.Text = "";
                button23.Enabled = false;
                return;
            }
            label47.Visible = true;
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
                label47.Text = "No encontrado";
                button23.Enabled = false;
                return;
            }
            label47.Text = cliente.Direccion;
            button23.Enabled = true;
        }
        private async void BorrarPorDire_textBox_TextChanged(object sender, EventArgs ev)
        {
            string direccion = textBoxDireEnBorrar.Text;
            if (direccion == "")
            {
                label47.Visible = false;
                label47.Text = "";
                button23.Enabled = false;
                return;
            }
            label47.Visible = true;
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
                label47.Text = "No encontrado";
                button23.Enabled = false;
                return;
            }
            label47.Text = cliente.Direccion;
            button23.Enabled = true;
        }
        private async void EliminarCliente_Click(object sender, EventArgs ev)
        {
            if (!cbSeguroBorrar.Checked)
            {
                return;
            }
            string direccion = label47.Text;
            bool logrado = await UIExecutor.ExecuteAsync(
                _scope,
                async sp => {
                    var savingServices = sp.GetRequiredService<ISavingServices>();
                    var clienteServices = sp.GetRequiredService<IClienteServices>();
                    Cliente cliente = await clienteServices.GetClientePorDireccionExactaAsync(direccion);
                    clienteServices.DeleteCliente(cliente);
                    return await savingServices.SaveAsync();
                },
                "No se pudo eliminar el Cliente",
                this
            );
            if (!logrado)
            {
                return;
            }
            textBox22.Text = "";
            textBoxDireEnBorrar.Text = "";
            label47.Text = "";
            label47.Visible = false;
            button23.Enabled = false;
            cbSeguroBorrar.Checked = false;
        }
    }
}
