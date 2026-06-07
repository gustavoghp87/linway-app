using AppLinway.PresentationHelpers;
using AppServices.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace AppLinway.Forms
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
            string opcion = comboBox3EliminarModalidad.SelectedItem?.ToString();
            if (opcion == "Establecer rango" && textBox5EliminarDesde.Text != "")
            {
                if (!long.TryParse(textBox5EliminarDesde.Text, out long rangoDesde))
                {
                    MessageBox.Show("Rango establecido incorrecto");
                    return new List<NotaDeEnvio>();
                }
                long rangoHasta = rangoDesde;
                if (!string.IsNullOrWhiteSpace(textBox4EliminarHasta.Text))
                {
                    long.TryParse(textBox4EliminarHasta.Text, out rangoHasta);
                }
                return _lstNotaDeEnvios.Where(ne => ne != null && ne.Id >= rangoDesde && ne.Id <= rangoHasta).ToList();
            }
            else if (opcion == "Todas")
            {
                return _lstNotaDeEnvios.Where(ne => ne != null).ToList();
            }
            else if (opcion == "Impresas")
            {
                return _lstNotaDeEnvios.Where(ne => ne != null && ne.Impresa == 1).ToList();
            }
            return new List<NotaDeEnvio>();
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
        private void CheckBox2EliminarIncluirRegistros_CheckedChanged(object sender, EventArgs e)
        {
            checkBox1EliminarRestarDeVentas.Visible = checkBox2EliminarIncluirRegistros.Checked;
            checkBox1EliminarRestarDeVentas.Checked = false;
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
            List<NotaDeEnvio> notasAEliminar = ObtenerListaABorrar();
            bool eliminarDeLosRepartos = checkBox1EliminarIncluirRepartos.Checked;
            bool eliminarRegistros = checkBox2EliminarIncluirRegistros.Checked;
            bool restarDeVentas = checkBox1EliminarRestarDeVentas.Checked;
            var resultado = await UIExecutor.ExecuteAsync(
                _scope,
                async sp => {
                    var useCase = _scope.ServiceProvider.GetRequiredService<IEliminarNotaDeEnvioUseCase>();
                    return await useCase.ExecuteAsync(notasAEliminar, eliminarDeLosRepartos, eliminarRegistros, restarDeVentas);
                },
                "No se pudieron eliminar las Notas de Envío",
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
            checkBox1EliminarIncluirRepartos.Checked = false;
            checkBox2EliminarIncluirRegistros.Checked = false;
            checkBox1EliminarRestarDeVentas.Checked = false;
            //
            await ActualizarNotas();
            EventoCombobox1ListaModalidad();
        }
    }
}
