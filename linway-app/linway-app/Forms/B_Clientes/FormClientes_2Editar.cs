using AppLinway.PresentationHelpers;
using AppServices.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Models;
using Models.Enums;
using System;
using System.Windows.Forms;

namespace AppLinway.Forms
{
    public partial class FormClientes : Form
    {
        private Cliente _clienteAEditar;
        private void LimpiarEditar_Click(object sender, EventArgs ev)
        {
            _clienteAEditar = null;
            label23EditarDireccionActual.Text = "";
            radioButton3EditarInscripto.Checked = false;
            radioButton4EditarMonotributo.Checked = false;
            textBox6EditarBusquedaDireccion.Text = "";
            textBox10EditarCuitActual.Text = "";
            textBox11EditarNombreActual.Text = "";
            textBox14EditarBusquedaNumero.Text = "";
            textBox23EditarDireccionNueva.Text = "";
            textBox24EditarTelefonoActual.Text = "";
            textBox25EditarCpActual.Text = "";
        }
        bool TodoOkModificarC()
        {
            var direccionNueva = textBox23EditarDireccionNueva.Text;
            return _clienteAEditar != null && direccionNueva != "" && (radioButton3EditarInscripto.Checked || radioButton4EditarMonotributo.Checked);
        }
        private void ActualizarEtiquetasDeClienteAEDitar(Cliente cliente)
        {
            if (cliente == null)
            {
                label23EditarDireccionActual.Text = "No encontrado";
                textBox11EditarNombreActual.Text = "";
                textBox10EditarCuitActual.Text = "";
                textBox23EditarDireccionNueva.Text = "";
                textBox24EditarTelefonoActual.Text = "";
                textBox25EditarCpActual.Text = "";
                radioButton3EditarInscripto.Checked = false;
                radioButton4EditarMonotributo.Checked = false;
                return;
            }
            label23EditarDireccionActual.Text = cliente.Direccion;
            textBox23EditarDireccionNueva.Text = cliente.Direccion;
            textBox24EditarTelefonoActual.Text = cliente.Telefono?.ToString();
            textBox25EditarCpActual.Text = cliente.CodigoPostal?.ToString();
            textBox11EditarNombreActual.Text = cliente.Nombre;
            textBox10EditarCuitActual.Text = cliente.Cuit;
            if (cliente.Tipo == TipoR.Inscripto.ToString())
            {
                radioButton3EditarInscripto.Checked = true;
            }
            else
            {
                radioButton4EditarMonotributo.Checked = true;
            }
        }
        private async void TextBox14_TextChanged(object sender, EventArgs ev)  // cliente por Id
        {
            _clienteAEditar = null;
            var numeroDeCliente = textBox14EditarBusquedaNumero.Text;
            if (numeroDeCliente == "")
            {
                label23EditarDireccionActual.Text = "";
                textBox11EditarNombreActual.Text = "";
                textBox10EditarCuitActual.Text = "";
                textBox23EditarDireccionNueva.Text = "";
                textBox24EditarTelefonoActual.Text = "";
                textBox25EditarCpActual.Text = "";
                radioButton3EditarInscripto.Checked = false;
                radioButton4EditarMonotributo.Checked = false;
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
            if (cliente != null)
            {
                _clienteAEditar = cliente;
            }
            ActualizarEtiquetasDeClienteAEDitar(cliente);
        }
        private async void TextBox6_TextChanged(object sender, EventArgs ev)  // cliente por dirección
        {
            _clienteAEditar = null;
            var direccion = textBox6EditarBusquedaDireccion.Text;
            if (direccion == "")
            {
                label23EditarDireccionActual.Text = "";
                textBox11EditarNombreActual.Text = "";
                textBox10EditarCuitActual.Text = "";
                textBox23EditarDireccionNueva.Text = "";
                textBox24EditarTelefonoActual.Text = "";
                textBox25EditarCpActual.Text = "";
                radioButton3EditarInscripto.Checked = false;
                radioButton4EditarMonotributo.Checked = false;
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
            _clienteAEditar.Direccion = textBox23EditarDireccionNueva.Text;
            _clienteAEditar.Telefono = textBox24EditarTelefonoActual.Text;
            _clienteAEditar.CodigoPostal = textBox25EditarCpActual.Text;
            _clienteAEditar.Nombre = textBox11EditarNombreActual.Text;
            _clienteAEditar.Cuit = textBox10EditarCuitActual.Text;
            _clienteAEditar.Tipo = radioButton3EditarInscripto.Checked ? TipoR.Inscripto.ToString() : TipoR.Monotributo.ToString();
            var resultado = await UIExecutor.ExecuteAsync(
                _scope,
                async sp =>
                {
                    var useCase = _scope.ServiceProvider.GetRequiredService<IEditarClienteUseCase>();
                    return await useCase.ExecuteAsync(_clienteAEditar.Direccion, _clienteAEditar.CodigoPostal, _clienteAEditar.Telefono, _clienteAEditar.Nombre, _clienteAEditar.Cuit, _clienteAEditar.Tipo);
                },
                "No se pudo modificar el cliente",
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
            _clienteAEditar = null;
            button8EditarLimpiar.PerformClick();
        }
    }
}
