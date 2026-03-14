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
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            textBox18.Text = "";
            radioButton1.Checked = false;
            radioButton2.Checked = false;
        }
        private async void AgregarCliente_Click(object sender, EventArgs ev)
        {
            if (textBox2.Text == "" || (!radioButton1.Checked && !radioButton2.Checked))
            {
                MessageBox.Show("Los campos Dirección y Responsable Inscr/Monotributo son obligatorios");
                return;
            }
            TipoR tipo = TipoR.Monotributo;
            if (radioButton2.Checked)
            {
                tipo = TipoR.Inscripto;
            }
            var nuevoCliente = new Cliente
            {
                Direccion = textBox18.Text != ""
                    ? textBox2.Text + " - " + textBox18.Text
                    : textBox2.Text,
                CodigoPostal = textBox4.Text,
                Telefono = textBox5.Text,
                Nombre = textBox1.Text,
                Cuit = textBox3.Text,
                Tipo = tipo.ToString()
            };
            var logrado = await UIExecutor.ExecuteAsync(
                _scope,
                async sp => {
                    var savingServices = sp.GetRequiredService<ISavingServices>();
                    var clienteServices = sp.GetRequiredService<IClienteServices>();
                    await clienteServices.AddClienteAsync(nuevoCliente);
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
            button2.PerformClick();
        }
    }
}
