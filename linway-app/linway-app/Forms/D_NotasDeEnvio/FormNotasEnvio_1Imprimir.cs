using Microsoft.Extensions.DependencyInjection;
using Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
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
            var listaAImprimir = new List<NotaDeEnvio>();
            string opcion = comboBox2TipoImprimir.SelectedItem.ToString();
            if (opcion == "No impresas")
            {
                foreach (NotaDeEnvio nota in _lstNotaDeEnvios)
                {
                    if (nota.Impresa == 0)
                    {
                        listaAImprimir.Add(nota);
                    }
                }
            }
            else if (opcion == "Hoy")
            {
                foreach (NotaDeEnvio nota in _lstNotaDeEnvios)
                {
                    if (nota.Fecha == DateTime.Now.ToString(Constants.FormatoDeFecha))
                    {
                        listaAImprimir.Add(nota);
                    }
                }
            }
            else if (opcion == "Establecer rango" && textBox2ImprimirRangoDesde.Text != "")
            {
                try
                {
                    int rangoDesde = int.Parse(textBox2ImprimirRangoDesde.Text);
                    int rangoHasta = textBox3ImprimirRangoHasta.Text != "" ? int.Parse(textBox3ImprimirRangoHasta.Text) : rangoDesde;
                    for (int i = rangoDesde; i <= rangoHasta; i++)
                    {
                        NotaDeEnvio nota = _lstNotaDeEnvios.Find(x => x.Id == i);
                        if (nota != null)
                        {
                            listaAImprimir.Add(nota);
                        }
                    }
                }
                catch {
                    MessageBox.Show("Rango establecido incorrecto");
                    return new List<NotaDeEnvio>();
                }
            }
            return listaAImprimir;
        }
        private void ComboBox2_SelectedIndexChanged(object sender, EventArgs ev)
        {
            string opcion = comboBox2TipoImprimir.SelectedItem.ToString();
            if (opcion == "No impresas" || opcion == "Hoy")
            {
                textBox2ImprimirRangoDesde.TextChanged -= TextBox2_TextChanged;  // evita error de concurrencia de DbContext
                textBox2ImprimirRangoDesde.Visible = false;
                textBox2ImprimirRangoDesde.Text = "";
                textBox2ImprimirRangoDesde.TextChanged += TextBox2_TextChanged;
                //
                textBox3ImprimirRangoHasta.TextChanged -= TextBox3_TextChanged;  // evita error de concurrencia de DbContext
                textBox3ImprimirRangoHasta.Visible = false;
                textBox3ImprimirRangoHasta.Text = "";
                textBox3ImprimirRangoHasta.TextChanged += TextBox3_TextChanged;
                //
                label4.Visible = false;
                label5.Visible = false;
            }
            else if (opcion == "Establecer rango")
            {
                textBox2ImprimirRangoDesde.TextChanged -= TextBox2_TextChanged;  // evita error de concurrencia de DbContext
                textBox2ImprimirRangoDesde.Visible = true;
                textBox2ImprimirRangoDesde.TextChanged += TextBox2_TextChanged;
                //
                textBox3ImprimirRangoHasta.TextChanged -= TextBox3_TextChanged;  // evita error de concurrencia de DbContext
                textBox3ImprimirRangoHasta.Visible = true;
                textBox3ImprimirRangoHasta.TextChanged += TextBox3_TextChanged;
                //
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
                        ActualizarGrid1(_lstNotaDeEnvios);
                    }
                };
                form.Show(this);
            }
        }
    }
}
