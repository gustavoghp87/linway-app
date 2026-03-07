using linway_app.PresentationHelpers;
using linway_app.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace linway_app.Forms
{
    public partial class FormNotasEnvio : Form
    {
        private async void ComboBox3_SelectedIndexChanged(object sender, EventArgs ev)
        {
            if ((new List<string>() { "Impresas", "(Seleccionar)", "Todas" }).Contains(comboBox3.SelectedItem.ToString()))
            {
                label13.Visible = false;
                label12.Visible = false;
                //
                textBox5.TextChanged -= TextBox5_TextChanged;  // evita error de concurrencia de DbContext
                textBox5.Visible = false;
                textBox5.Text = "";
                textBox5.TextChanged += TextBox5_TextChanged;
                //
                textBox4.TextChanged -= TextBox4_TextChanged;  // evita error de concurrencia de DbContext
                textBox4.Visible = false;
                textBox4.Text = "";
                textBox4.TextChanged += TextBox4_TextChanged;
            }
            else if (comboBox3.SelectedItem.ToString() == "Establecer rango")
            {
                textBox4.TextChanged -= TextBox4_TextChanged;  // evita error de concurrencia de DbContext
                textBox4.Visible = true;
                textBox4.Text = "";
                textBox4.TextChanged += TextBox4_TextChanged;
                //
                textBox5.TextChanged -= TextBox5_TextChanged;  // evita error de concurrencia de DbContext
                textBox5.Visible = true;
                textBox5.TextChanged += TextBox5_TextChanged;
                //
                label13.Visible = true;
                label12.Visible = true;
            }
            var lista = await ObtenerListaABorrar();
            label10.Text = lista.Count.ToString();
        }
        private async Task<List<NotaDeEnvio>> ObtenerListaABorrar()
        {
            await ActualizarNotas();
            var listaABorrar = new List<NotaDeEnvio>();
            if (_lstNotaDeEnvios == null || _lstNotaDeEnvios.Count == 0)
            {
                return listaABorrar;
            }
            string opcion = comboBox3.SelectedItem.ToString();
            if (opcion == "Establecer rango" && textBox5.Text != "" && textBox4.Text != "")
            {
                try
                {
                    int j = int.Parse(textBox4.Text);
                    for (int i = int.Parse(textBox5.Text); i <= j; i++)
                    {
                        NotaDeEnvio nota = _lstNotaDeEnvios.Find(x => x.Id == i);
                        if (nota != null)
                        {
                            listaABorrar.Add(nota);
                        }
                    }
                }
                catch {
                    MessageBox.Show("Rango establecido incorrecto");
                    return new List<NotaDeEnvio>();
                }
            }
            else if (opcion == "Todas")
            {
                foreach (NotaDeEnvio nota in _lstNotaDeEnvios)
                {
                    if (nota != null)
                    {
                        listaABorrar.Add(nota);
                    }
                }
            }
            else if (opcion == "Impresas")
            {
                foreach (NotaDeEnvio nota in _lstNotaDeEnvios)
                {
                    if (nota != null && nota.Impresa == 1)
                    {
                        listaABorrar.Add(nota);
                    }
                }
            }
            return listaABorrar;
        }
        private async void TextBox5_TextChanged(object sender, EventArgs ev)
        {
            var lista = await ObtenerListaABorrar();
            label10.Text = lista.Count.ToString();
        }
        private async void TextBox4_TextChanged(object sender, EventArgs ev)
        {
            var lista = await ObtenerListaABorrar();
            label10.Text = lista.Count.ToString();
        }
        private void Button3_Click(object sender, EventArgs ev)
        {
            if ((new List<string>() { "Establecer rango", "Impresas", "Todas" }).Contains(comboBox3.SelectedItem.ToString()))
            {
                label11.Visible = true;
                button4.Visible = true;
                button5.Visible = true;
                button3.Visible = false;
                MessageBox.Show("Confirme si desea borrar las notas de envio seleccionadas");
            }
        }
        private void Button5_Click(object sender, EventArgs ev)
        {
            comboBox3.SelectedIndexChanged -= ComboBox3_SelectedIndexChanged;  // evita error de concurrencia de DbContext
            comboBox3.SelectedItem = "(Seleccionar)";
            comboBox3.SelectedIndexChanged += ComboBox3_SelectedIndexChanged;
            //
            label11.Visible = false;
            button4.Visible = false;
            button5.Visible = false;
            button3.Visible = true;
        }
        private async void Button4_Click(object sender, EventArgs ev)
        {
            List<NotaDeEnvio> notas = await ObtenerListaABorrar();
            bool logrado = await UIExecutor.ExecuteAsync(
                _scope,
                async sp => {
                    var savingServices = sp.GetRequiredService<ISavingServices>();
                    var notaDeEnvioServices = sp.GetRequiredService<INotaDeEnvioServices>();
                    notaDeEnvioServices.DeleteNotas(notas);
                    return await savingServices.SaveAsync();
                },
                "No se pudieron eliminar las Notas de Envío",
                this
            );
            if (!logrado)
            {
                return;
            }
            //
            comboBox3.SelectedIndexChanged -= ComboBox3_SelectedIndexChanged;  // evita error de concurrencia de DbContext
            comboBox3.SelectedItem = "(Seleccionar)";
            comboBox3.SelectedIndexChanged += ComboBox3_SelectedIndexChanged;
            //
            label11.Visible = false;
            button4.Visible = false;
            button5.Visible = false;
            button3.Visible = true;
            await ActualizarNotas();
            ActualizarGrid1(_lstNotaDeEnvios);
        }
    }
}
