using Microsoft.Extensions.DependencyInjection;
using Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;

namespace linway_app.Forms
{
    public partial class FormNotasEnvio : Form
    {
        private void TextBox2_KeyPress(object sender, KeyPressEventArgs ev)
        {
            if (!char.IsNumber(ev.KeyChar) && ev.KeyChar != (char)Keys.Back)
            {
                ev.Handled = true;
            }
        }
        private void TextBox3_KeyPress(object sender, KeyPressEventArgs ev)
        {
            if (!char.IsNumber(ev.KeyChar) && ev.KeyChar != (char)Keys.Back)
            {
                ev.Handled = true;
            }
        }
        private List<NotaDeEnvio> ObtenerListaAImprimir()
        {
            string opcion = comboBox2TipoImprimir.SelectedItem?.ToString();
            if (opcion == "No impresas")
            {
                return _lstNotaDeEnvios.FindAll(x => x.Impresa == 0);
            }
            else if (opcion == "Hoy")
            {
                return _lstNotaDeEnvios.FindAll(x => x.Fecha == DateTime.Now.ToString(Constants.FormatoDeFecha));
            }
            else if (opcion == "Establecer rango" && textBox2ImprimirRangoDesde.Text != "")
            {
                try
                {
                    int rangoDesde = int.Parse(textBox2ImprimirRangoDesde.Text);
                    int rangoHasta = textBox3ImprimirRangoHasta.Text != "" ? int.Parse(textBox3ImprimirRangoHasta.Text) : rangoDesde;
                    return _lstNotaDeEnvios.FindAll(x => x.Id >= rangoDesde && x.Id <= rangoHasta);
                }
                catch {
                    MessageBox.Show("Rango establecido incorrecto");
                    return new List<NotaDeEnvio>();
                }
            }
            return new List<NotaDeEnvio>();
        }
        private void ComboBox2_SelectedIndexChanged(object sender, EventArgs ev)
        {
            string opcion = comboBox2TipoImprimir.SelectedItem?.ToString();
            if (opcion == "No impresas" || opcion == "Hoy")
            {
                textBox2ImprimirRangoDesde.Visible = false;
                textBox2ImprimirRangoDesde.Text = "";
                textBox3ImprimirRangoHasta.Visible = false;
                textBox3ImprimirRangoHasta.Text = "";
                label4.Visible = false;
                label5.Visible = false;
            }
            else if (opcion == "Establecer rango")
            {
                textBox2ImprimirRangoDesde.Visible = true;
                textBox3ImprimirRangoHasta.Visible = true;
                label4.Visible = true;
                label5.Visible = true;
            }
            label7.Text = ObtenerListaAImprimir().Count.ToString();
        }
        private void TextBox3_TextChanged(object sender, EventArgs ev)
        {
            label7.Text = ObtenerListaAImprimir().Count.ToString();
        }
        private void TextBox2_TextChanged(object sender, EventArgs ev)
        {
            label7.Text = ObtenerListaAImprimir().Count.ToString();
        }
        private void Button1Imprimir_Click(object sender, EventArgs ev)
        {
            if (comboBox2TipoImprimir.Text == "")
            {
                return;
            }
            var lstAImprimir = ObtenerListaAImprimir();
            int abiertos = 0;
            foreach (NotaDeEnvio nota in lstAImprimir)
            {
                abiertos++;
                var scope = Program.LinwayServiceProvider.CreateScope();
                var form = scope.ServiceProvider.GetRequiredService<FormImprimirNota>();
                form.Rellenar_Datos(nota);
                form.FormClosing += async (s, e) =>
                {
                    scope.Dispose();
                    if (Interlocked.Decrement(ref abiertos) == 0)
                    {
                        _scope.Dispose();
                        _scope = Program.LinwayServiceProvider.CreateScope();  // renueva scope para que tome los cambios hechos en otros scopes
                        await ActualizarNotas();
                        EventoCombobox1ListaModalidad();
                    }
                };
                form.Show(this);
            }
        }
    }
}
