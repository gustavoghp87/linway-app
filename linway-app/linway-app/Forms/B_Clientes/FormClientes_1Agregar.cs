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
            string direccion = textBox2.Text;
            if (direccion == "" || (!radioButton1AgregarRInscripto.Checked && !radioButton2AgregarRInscripto.Checked))
            {
                MessageBox.Show("Los campos Dirección y Responsable Inscr/Monotributo son obligatorios");
                return;
            }
            TipoR tipo = radioButton2AgregarRInscripto.Checked ? TipoR.Inscripto : TipoR.Monotributo;
            var nuevoCliente = new Cliente
            {
                Direccion = textBox18AgregarLocalidad.Text != ""
                    ? direccion + " - " + textBox18AgregarLocalidad.Text
                    : direccion,
                CodigoPostal = textBox4AgregarCP.Text,
                Telefono = textBox5AgregarTelefono.Text,
                Nombre = textBox1AgregarNombre.Text,
                Cuit = textBox3AgregarCuit.Text,
                Tipo = tipo.ToString()
            };
            var logrado = await UIExecutor.ExecuteAsync(
                _scope,
                async sp => {
                    var savingServices = sp.GetRequiredService<ISavingServices>();
                    var clienteServices = sp.GetRequiredService<IClienteServices>();
                    await clienteServices.AddAsync(nuevoCliente);
                    bool guardado = await savingServices.SaveAsync();
                    if (!guardado)
                    {
                        savingServices.DiscardChanges();
                        MessageBox.Show("No se hicieron cambios");
                    }
                    return guardado;
                },
                "No se pudo agregar el Cliente",
                this
            );
            if (!logrado)
            {
                return;
            }
            button2AgregarLimpiar.PerformClick();
        }
    }
}
