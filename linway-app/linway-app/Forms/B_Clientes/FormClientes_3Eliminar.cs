using AppLinway.PresentationHelpers;
using AppServices.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Models;
using System;
using System.Windows.Forms;

namespace AppLinway.Forms
{
    public partial class FormClientes : Form
    {
        private Cliente _clienteAEliminar;
        private async void BorrarPorId_textBox_TextChanged(object sender, EventArgs ev)  // cliente por Id
        {
            _clienteAEliminar = null;
            var numeroDeCliente = textBox22.Text;
            if (numeroDeCliente == "" || !long.TryParse(textBox22.Text, out long clienteId))
            {
                label47EliminarDireccion.Visible = false;
                label47EliminarDireccion.Text = "";
                button23EliminarCliente.Enabled = false;
                return;
            }
            label47EliminarDireccion.Visible = true;
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
            var direccion = textBoxDireEnBorrar.Text;
            if (direccion == "")
            {
                label47EliminarDireccion.Visible = false;
                label47EliminarDireccion.Text = "";
                button23EliminarCliente.Enabled = false;
                return;
            }
            label47EliminarDireccion.Visible = true;
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
            var resultado = await UIExecutor.ExecuteAsync(
                _scope,
                async sp => {
                    var useCase = _scope.ServiceProvider.GetRequiredService<IEliminarClienteUseCase>();
                    return await useCase.ExecuteAsync(_clienteAEliminar);
                },
                "No se pudo eliminar el Cliente",
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
