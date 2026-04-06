using linway_app.PresentationHelpers;
using Models;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace linway_app.Forms
{
    public partial class FormRecibos : Form
    {
        private void ComboBox3_SelectedIndexChanged(object sender, EventArgs ev)
        {
            string opcion = comboBox3EliminarTipo.SelectedItem.ToString();
            if (opcion == "Impresas" || opcion == "Todas" || opcion == "(Seleccionar)")
            {
                textBox4EliminarRangoHasta.Visible = false;
                textBox5EliminarRangoDesde.Visible = false;
                label13.Visible = false;
                label12.Visible = false;
                textBox4EliminarRangoHasta.Text = "";
                textBox5EliminarRangoDesde.Text = "";
            }
            else if (opcion == "Establecer rango")
            {
                textBox4EliminarRangoHasta.Visible = true;
                textBox5EliminarRangoDesde.Visible = true;
                label13.Visible = true;
                label12.Visible = true;
            }
            label10EliminarCantidad.Text = ObtenerListaABorrar().Count.ToString();
        }
        private void TextBox5_TextChanged(object sender, EventArgs ev)  // rango inferior
        {
            label10EliminarCantidad.Text = ObtenerListaABorrar().Count.ToString();
        }
        private void TextBox4_TextChanged(object sender, EventArgs ev)  // rango superior
        {
            label10EliminarCantidad.Text = ObtenerListaABorrar().Count.ToString();
        }
        private List<Recibo> ObtenerListaABorrar()
        {
            string opcion = comboBox3EliminarTipo.SelectedItem?.ToString();
            if (opcion == "Establecer rango" && textBox5EliminarRangoDesde.Text != "")
            {
                try
                {
                    int rangoDesde = int.Parse(textBox5EliminarRangoDesde.Text);
                    int rangoHasta = textBox4EliminarRangoHasta.Text != "" ? int.Parse(textBox4EliminarRangoHasta.Text) : rangoDesde;
                    return _lstRecibos.FindAll(x => x.Id >= rangoDesde && x.Id <= rangoHasta);
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message, "Rango establecido incorrecto");
                    return new List<Recibo>();
                }
            }
            else if (opcion == "Todas")
            {
                return _lstRecibos;
            }
            else if (opcion == "Impresas")
            {
                return _lstRecibos.FindAll(x => x.Impreso == 1);
            }
            return new List<Recibo>();
        }
        private void Button3_Click(object sender, EventArgs ev)      // aceptar antes de confirmar
        {
            if (label10EliminarCantidad.Text == "" || label10EliminarCantidad.Text == "0")
            {
                return;
            }
            //MessageBox.Show("Confirme si desea borrar los recibos seleccionados");
            label11.Visible = true;
            button4.Visible = true;
            button5.Visible = true;
            button3.Visible = false;
        }
        private void Button5_Click(object sender, EventArgs ev)      // cancelar
        {
            comboBox3EliminarTipo.SelectedItem = "(Seleccionar)";
            label11.Visible = false;
            button4.Visible = false;
            button5.Visible = false;
            button3.Visible = true;
        }
        private async void Button4_Click(object sender, EventArgs ev)      // confirmar
        {
            var recibosAEliminar = ObtenerListaABorrar();
            if (recibosAEliminar.Count == 0)
            {
                return;
            }
            var detallesAEliminar = new List<DetalleRecibo>();
            foreach (Recibo recibo in recibosAEliminar)
            {
                if (recibo.DetalleRecibos != null)
                {
                    detallesAEliminar.AddRange(recibo.DetalleRecibos);
                }
            }
            bool logrado = await UIExecutor.ExecuteAsync(
                _scope,
                async sp =>
                {
                    var servicesContext = ServiceContext.Get(sp);
                    servicesContext.DetalleReciboServices.DeleteMany(detallesAEliminar);  // primero
                    servicesContext.ReciboServices.DeleteMany(recibosAEliminar);  // segundo
                    bool guardado = await servicesContext.SavingServices.SaveAsync();
                    if (!guardado)
                    {
                        servicesContext.SavingServices.DiscardChanges();
                        MessageBox.Show("No se hicieron cambios");
                    }
                    return guardado;
                },
                "No se pudieron eliminar los Recibos",
                this
            );
            if (!logrado)
            {
                return;
            }
            await Actualizar();
            comboBox3EliminarTipo.SelectedItem = "(Seleccionar)";
            label11.Visible = false;
            button4.Visible = false;
            button5.Visible = false;
            button3.Visible = true;
            label10EliminarCantidad.Text = "0";
            ActualizarGridRecibos(_lstRecibos);
            comboBox1FiltrarLista.SelectedItem = "Todas";
        }
    }
}
