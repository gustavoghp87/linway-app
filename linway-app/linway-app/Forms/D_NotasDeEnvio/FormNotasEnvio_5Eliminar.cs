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
                label13EliminarHasta.Visible = false;
                label12EliminarDesde.Visible = false;
                //
                textBox5EliminarHasta.TextChanged -= TextBox5_TextChanged;  // evita error de concurrencia de DbContext
                textBox5EliminarHasta.Visible = false;
                textBox5EliminarHasta.Text = "";
                textBox5EliminarHasta.TextChanged += TextBox5_TextChanged;
                //
                textBox4EliminarDesde.TextChanged -= TextBox4_TextChanged;  // evita error de concurrencia de DbContext
                textBox4EliminarDesde.Visible = false;
                textBox4EliminarDesde.Text = "";
                textBox4EliminarDesde.TextChanged += TextBox4_TextChanged;
            }
            else if (comboBox3.SelectedItem.ToString() == "Establecer rango")
            {
                textBox4EliminarDesde.TextChanged -= TextBox4_TextChanged;  // evita error de concurrencia de DbContext
                textBox4EliminarDesde.Visible = true;
                textBox4EliminarDesde.Text = "";
                textBox4EliminarDesde.TextChanged += TextBox4_TextChanged;
                //
                textBox5EliminarHasta.TextChanged -= TextBox5_TextChanged;  // evita error de concurrencia de DbContext
                textBox5EliminarHasta.Visible = true;
                textBox5EliminarHasta.TextChanged += TextBox5_TextChanged;
                //
                label12EliminarDesde.Visible = true;
                label13EliminarHasta.Visible = true;
            }
            var lista = await ObtenerListaABorrar();
            label10CantidadABorrar.Text = lista.Count.ToString();
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
            if (opcion == "Establecer rango" && textBox5EliminarHasta.Text != "" && textBox4EliminarDesde.Text != "")
            {
                try
                {
                    int j = int.Parse(textBox4EliminarDesde.Text);
                    for (int i = int.Parse(textBox5EliminarHasta.Text); i <= j; i++)
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
            label10CantidadABorrar.Text = lista.Count.ToString();
        }
        private async void TextBox4_TextChanged(object sender, EventArgs ev)
        {
            var lista = await ObtenerListaABorrar();
            label10CantidadABorrar.Text = lista.Count.ToString();
        }
        private void Button3_Click(object sender, EventArgs ev)
        {
            if ((new List<string>() { "Establecer rango", "Impresas", "Todas" }).Contains(comboBox3.SelectedItem.ToString()))
            {
                label11SerguroDeseaBorrar.Visible = true;
                button4EliminarConfirmacion.Visible = true;
                button5CancelarEliminarPorRangos.Visible = true;
                button3EliminarPrimero.Visible = false;
            }
        }
        private void Button5_Click(object sender, EventArgs ev)  // cancelar (en rangos)
        {
            label10CantidadABorrar.Text = "0";
            label11SerguroDeseaBorrar.Visible = false;
            button4EliminarConfirmacion.Visible = false;
            button5CancelarEliminarPorRangos.Visible = false;
            button3EliminarPrimero.Visible = true;
            label12EliminarDesde.Visible = false;
            label13EliminarHasta.Visible = false;
            //
            comboBox3.SelectedIndexChanged -= ComboBox3_SelectedIndexChanged;  // evita error de concurrencia de DbContext
            comboBox3.SelectedItem = "(Seleccionar)";
            comboBox3.SelectedIndexChanged += ComboBox3_SelectedIndexChanged;
            //
            textBox4EliminarDesde.TextChanged -= TextBox4_TextChanged;  // evita error de concurrencia de DbContext
            textBox4EliminarDesde.Visible = false;
            textBox4EliminarDesde.Text = "";
            textBox4EliminarDesde.TextChanged += TextBox4_TextChanged;
            //
            textBox5EliminarHasta.TextChanged -= TextBox5_TextChanged;  // evita error de concurrencia de DbContext
            textBox5EliminarHasta.Visible = false;
            textBox5EliminarHasta.Text = "";
            textBox5EliminarHasta.TextChanged += TextBox5_TextChanged;
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
            label11SerguroDeseaBorrar.Visible = false;
            button4EliminarConfirmacion.Visible = false;
            button5CancelarEliminarPorRangos.Visible = false;
            label13EliminarHasta.Visible = false;
            label12EliminarDesde.Visible = false;
            label10CantidadABorrar.Text = "0";
            button3EliminarPrimero.Visible = false;
            //
            textBox4EliminarDesde.TextChanged -= TextBox4_TextChanged;  // evita error de concurrencia de DbContext
            textBox4EliminarDesde.Visible = false;
            textBox4EliminarDesde.Text = "";
            textBox4EliminarDesde.TextChanged += TextBox4_TextChanged;
            //
            textBox5EliminarHasta.TextChanged -= TextBox5_TextChanged;  // evita error de concurrencia de DbContext
            textBox5EliminarHasta.Visible = false;
            textBox5EliminarHasta.Text = "";
            textBox5EliminarHasta.TextChanged += TextBox5_TextChanged;
            //
            await ActualizarNotas();
            ActualizarGrid1(_lstNotaDeEnvios);
        }
    }
}
