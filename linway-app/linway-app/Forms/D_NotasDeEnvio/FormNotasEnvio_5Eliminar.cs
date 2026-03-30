using linway_app.PresentationHelpers;
using linway_app.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace linway_app.Forms
{
    public partial class FormNotasEnvio : Form
    {
        private async void ComboBox3_SelectedIndexChanged(object sender, EventArgs ev)
        {
            if ((new List<string>() { "Impresas", "(Seleccionar)", "Todas" }).Contains(comboBox3EliminarModalidad.SelectedItem.ToString()))
            {
                label13EliminarHasta.Visible = false;
                label12EliminarDesde.Visible = false;
                textBox5EliminarDesde.Visible = false;
                textBox5EliminarDesde.Text = "";
                textBox4EliminarHasta.Visible = false;
                textBox4EliminarHasta.Text = "";
            }
            else if (comboBox3EliminarModalidad.SelectedItem.ToString() == "Establecer rango")
            {
                textBox4EliminarHasta.Visible = true;
                textBox4EliminarHasta.Text = "";
                textBox5EliminarDesde.Visible = true;
                label12EliminarDesde.Visible = true;
                label13EliminarHasta.Visible = true;
            }
            var lista = ObtenerListaABorrar();
            label10CantidadABorrar.Text = lista.Count.ToString();
        }
        private List<NotaDeEnvio> ObtenerListaABorrar()
        {
            var listaABorrar = new List<NotaDeEnvio>();
            string opcion = comboBox3EliminarModalidad.SelectedItem.ToString();
            if (opcion == "Establecer rango" && textBox5EliminarDesde.Text != "")
            {
                try
                {
                    int rangoDesde = int.Parse(textBox5EliminarDesde.Text);
                    int rangoHasta = textBox4EliminarHasta.Text != "" ? int.Parse(textBox4EliminarHasta.Text) : rangoDesde;
                    for (int i = rangoDesde; i <= rangoHasta; i++)
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
                listaABorrar = _lstNotaDeEnvios.Where(ne => ne != null).ToList();
            }
            else if (opcion == "Impresas")
            {
                listaABorrar.AddRange(_lstNotaDeEnvios.Where(ne => ne != null && ne.Impresa == 1).ToList());
            }
            return listaABorrar;
        }
        private void TextBox5_TextChanged(object sender, EventArgs ev)
        {
            var lista = ObtenerListaABorrar();
            label10CantidadABorrar.Text = lista.Count.ToString();
        }
        private void TextBox4_TextChanged(object sender, EventArgs ev)
        {
            var lista = ObtenerListaABorrar();
            label10CantidadABorrar.Text = lista.Count.ToString();
        }
        private void Button3_Click(object sender, EventArgs ev)
        {
            if ((new List<string>() { "Establecer rango", "Impresas", "Todas" }).Contains(comboBox3EliminarModalidad.SelectedItem.ToString()))
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
            comboBox3EliminarModalidad.SelectedItem = "(Seleccionar)";
            textBox4EliminarHasta.Visible = false;
            textBox4EliminarHasta.Text = "";
            textBox5EliminarDesde.Visible = false;
            textBox5EliminarDesde.Text = "";
        }
        private async void Button4_Click(object sender, EventArgs ev)  // eliminar nota de envío
        {
            List<NotaDeEnvio> notas = ObtenerListaABorrar();
            bool logrado = await UIExecutor.ExecuteAsync(
                _scope,
                async sp => {
                    var savingServices = sp.GetRequiredService<ISavingServices>();
                    var notaDeEnvioServices = sp.GetRequiredService<INotaDeEnvioServices>();
                    var prodVendidoServices = sp.GetRequiredService<IProdVendidoServices>();
                    List<ProdVendido> prodVendidos = notas.SelectMany(n => n.ProdVendidos).ToList();
                    foreach (var prodVendido in prodVendidos)
                    {
                        prodVendido.NotaDeEnvioId = null;
                    }
                    prodVendidoServices.EditOrDeleteMany(prodVendidos);
                    notaDeEnvioServices.DeleteMany(notas);
                    bool guardado = await savingServices.SaveAsync();
                    if (!guardado)
                    {
                        savingServices.DiscardChanges();
                        MessageBox.Show("No se hicieron cambios");
                    }
                    return guardado;
                },
                "No se pudieron eliminar las Notas de Envío",
                this
            );
            if (!logrado)
            {
                return;
            }
            comboBox3EliminarModalidad.SelectedItem = "(Seleccionar)";
            label11SerguroDeseaBorrar.Visible = false;
            button4EliminarConfirmacion.Visible = false;
            button5CancelarEliminarPorRangos.Visible = false;
            label13EliminarHasta.Visible = false;
            label12EliminarDesde.Visible = false;
            label10CantidadABorrar.Text = "0";
            button3EliminarPrimero.Visible = true;
            textBox4EliminarHasta.Visible = false;
            textBox4EliminarHasta.Text = "";
            textBox5EliminarDesde.Visible = false;
            textBox5EliminarDesde.Text = "";
            //
            await ActualizarNotas();
            EventoCombobox1ListaModalidad();
        }
    }
}
