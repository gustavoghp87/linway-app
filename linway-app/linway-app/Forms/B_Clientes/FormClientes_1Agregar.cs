using AppLinway.PresentationHelpers;
using AppServices.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Models.Enums;
using System;
using System.Windows.Forms;

namespace AppLinway.Forms
{
    public partial class FormClientes : Form
    {
        private void LimpiarAgregar_Click(object sender, EventArgs ev)
        {
            textBox1AgregarNombre.Text = "";
            textBox2.Text = "";
            textBox3AgregarCuit.Text = "";
            textBox4AgregarCP.Text = "";
            textBox5AgregarTelefono.Text = "";
            textBox18AgregarLocalidad.Text = "";
            radioButton1AgregarRInscripto.Checked = false;
            radioButton2AgregarRInscripto.Checked = false;
        }
        private async void AgregarCliente_Click(object sender, EventArgs ev)
        {
            var direccion = textBox2.Text;
            if (direccion == "" || (!radioButton1AgregarRInscripto.Checked && !radioButton2AgregarRInscripto.Checked))
            {
                MessageBox.Show("Los campos Dirección y Responsable Inscr/Monotributo son obligatorios");
                return;
            }
            var localidad = textBox18AgregarLocalidad.Text;
            var codigoPostal = textBox4AgregarCP.Text;
            var telefono = textBox5AgregarTelefono.Text;
            var nombre = textBox1AgregarNombre.Text;
            var cuit = textBox3AgregarCuit.Text;
            var tipo = radioButton2AgregarRInscripto.Checked ? TipoR.Inscripto.ToString() : TipoR.Monotributo.ToString();
            var resultado = await UIExecutor.ExecuteAsync(
                _scope,
                async sp => {
                    var useCase = _scope.ServiceProvider.GetRequiredService<IAgregarClienteUseCase>();
                    return await useCase.ExecuteAsync(direccion, localidad, codigoPostal, telefono, nombre, cuit, tipo);
                },
                "No se pudo agregar el Cliente",
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
            button2AgregarLimpiar.PerformClick();
        }
    }
}
