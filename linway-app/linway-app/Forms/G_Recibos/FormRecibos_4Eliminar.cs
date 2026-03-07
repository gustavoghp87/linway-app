using linway_app.PresentationHelpers;
using linway_app.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
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
            if (comboBox3.SelectedItem == null)
            {
                return;
            }
            if (comboBox3.SelectedItem.ToString() == "Impresas" || comboBox3.SelectedItem.ToString() == "Todas" || comboBox3.SelectedItem.ToString() == "(Seleccionar)")
            {
                textBox4.Visible = false;
                textBox5.Visible = false;
                label13.Visible = false;
                label12.Visible = false;
                textBox4.Text = "";
                textBox5.Text = "";
            }
            else if (comboBox3.SelectedItem.ToString() == "Establecer rango")
            {
                textBox4.Visible = true;
                textBox5.Visible = true;
                label13.Visible = true;
                label12.Visible = true;
            }
            label10.Text = ObtenerListaABorrar().Count.ToString();
        }
        private void TextBox5_TextChanged(object sender, EventArgs ev)  // rango inferior
        {
            label10.Text = ObtenerListaABorrar().Count.ToString();
        }
        private void TextBox4_TextChanged(object sender, EventArgs ev)  // rango superior
        {
            label10.Text = ObtenerListaABorrar().Count.ToString();
        }

        private List<Recibo> ObtenerListaABorrar()
        {
            var listaABorrar = new List<Recibo>();
            if (comboBox3.SelectedItem == null)
            {
                return listaABorrar;
            }
            if (comboBox3.SelectedItem.ToString() == "Establecer rango" && textBox5.Text != "" && textBox4.Text != "")
            {
                try
                {
                    long menor = long.Parse(textBox5.Text);
                    long mayor = long.Parse(textBox4.Text);
                    if (menor <= mayor)
                    {
                        for (long i = menor; i <= mayor; i++)
                        {
                            var recibo = _lstRecibos.Find(x => x.Id == i);
                            if (recibo != null)
                            {
                                listaABorrar.Add(recibo);
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message, "Rango establecido incorrecto");
                    return new List<Recibo>();
                }
            }
            else if (comboBox3.SelectedItem.ToString() == "Todas")
            {
                foreach (Recibo recibo in _lstRecibos)
                {
                    if (recibo != null)
                    {
                        listaABorrar.Add(recibo);
                    }
                }
            }
            else if (comboBox3.SelectedItem.ToString() == "Impresas")
            {
                foreach (Recibo recibo in _lstRecibos)
                {
                    if (recibo != null && recibo.Impreso == 1)
                    {
                        listaABorrar.Add(recibo);
                    }
                }
            }
            return listaABorrar;
        }
        private void Button3_Click(object sender, EventArgs ev)      // Accept
        {
            if (comboBox3.SelectedItem == null || comboBox3.SelectedItem.ToString() == "(Seleccionar)")
            {
                return;
            }
            if (
                comboBox3.SelectedItem.ToString() != "Todas"
                && (comboBox3.SelectedItem.ToString() != "Establecer rango" && (textBox5.Text == "" || textBox4.Text == ""))
                && comboBox3.SelectedItem.ToString() != "Impresas"
            )
            {
                return;
            }
            if (label10.Text == "" || label10.Text == "0")
            {
                return;
            }
            MessageBox.Show("Confirme si desea borrar los recibos seleccionados");
            label11.Visible = true;
            button4.Visible = true;
            button5.Visible = true;
            button3.Visible = false;
        }
        private void Button5_Click(object sender, EventArgs ev)      // Cancel
        {
            comboBox3.SelectedItem = "(Seleccionar)";
            label11.Visible = false;
            button4.Visible = false;
            button5.Visible = false;
            button3.Visible = true;
        }
        private async void Button4_Click(object sender, EventArgs ev)      // Confirm
        {
            var detallesAEliminar = new List<DetalleRecibo>();
            var recibosAEliminar = ObtenerListaABorrar();
            foreach (Recibo recibo in recibosAEliminar)
            {
                if (recibo?.DetalleRecibos != null)
                {
                    foreach(DetalleRecibo detalle in recibo.DetalleRecibos)
                    {
                        detallesAEliminar.Add(detalle);
                    }
                }
            }
            bool logrado = await UIExecutor.ExecuteAsync(
                _scope,
                async sp =>
                {
                    var savingServices = sp.GetRequiredService<ISavingServices>();
                    var reciboService = sp.GetRequiredService<IReciboServices>();
                    var detalleReciboService = sp.GetRequiredService<IDetalleReciboServices>();
                    detalleReciboService.DeleteDetalles(detallesAEliminar);
                    reciboService.DeleteRecibos(recibosAEliminar);
                    bool guardado = await savingServices.SaveAsync();
                    if (!guardado)
                    {
                        savingServices.DiscardChanges();
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
            comboBox3.SelectedItem = "(Seleccionar)";
            label11.Visible = false;
            button4.Visible = false;
            button5.Visible = false;
            button3.Visible = true;
            lCantRecibos.Text = _lstRecibos.Count.ToString() + " recibos.";
            ActualizarGridRecibos(_lstRecibos);
        }
    }
}
