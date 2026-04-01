using Microsoft.Extensions.DependencyInjection;
using Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace linway_app.Forms
{
    public partial class FormRecibos : Form
    {
        //private void RadioButton1_CheckedChanged(object sender, EventArgs ev)
        //{
        //    textBox7.Enabled = radioButton4.Checked;
        //}
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
        private List<Recibo> ObtenerListaAImprimir()
        {
            string opcion = comboBox2ImprimirTipo.SelectedItem?.ToString();
            if (opcion == "No impresas")
            {
                return _lstRecibos.FindAll(r => r.Impreso == 0);
            }
            else if (opcion == "Hoy")
            {
                return _lstRecibos.FindAll(r => r.Fecha == DateTime.Now.ToString("yyy-MM-dd"));
            }
            else if (opcion == "Establecer rango" && textBox2ImprimirRangoDesde.Text != "")
            {
                var listaAImprimir = new List<Recibo>();
                try
                {
                    int rangoDesde = int.Parse(textBox2ImprimirRangoDesde.Text);
                    int rangoHasta = textBox3ImprimirRangoHasta.Text != "" ? int.Parse(textBox3ImprimirRangoHasta.Text) : rangoDesde;
                    return _lstRecibos.FindAll(r => r.Id >= rangoDesde && r.Id <= rangoHasta);
                }
                catch
                {
                    MessageBox.Show("Rango establecido incorrecto");
                }
            }
            return new List<Recibo>();
        }
        private void ComboBox2_SelectedIndexChanged(object sender, EventArgs ev)
        {
            string opcion = comboBox2ImprimirTipo.SelectedItem.ToString();
            if (opcion == "No impresas" || opcion == "Hoy")
            {
                textBox2ImprimirRangoDesde.Visible = false;
                textBox3ImprimirRangoHasta.Visible = false;
                label4.Visible = false;
                label5.Visible = false;
                textBox3ImprimirRangoHasta.Text = "";
                textBox2ImprimirRangoDesde.Text = "";
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
        private void Button1_Click(object sender, EventArgs ev)
        {
            List<Recibo> listaAImprimir = ObtenerListaAImprimir();
            if (listaAImprimir.Count > 20)
            {
                MessageBox.Show("Se imprimen hasta 20 por vez");
            }
            int i = 0;
            int abiertos = 0;
            foreach (Recibo recibo in listaAImprimir)
            {
                i++;
                if (i > 20)
                {
                    continue;
                }
                //var form = Program.LinwayServiceProvider.GetRequiredService<FormImprimirRecibo>();
                //form.Rellenar_Datos(recibo);
                //form.Show(this);
                abiertos++;
                var scope = Program.LinwayServiceProvider.CreateScope();
                var form = scope.ServiceProvider.GetRequiredService<FormImprimirRecibo>();
                form.Rellenar_Datos(recibo);
                form.FormClosing += async (s, e) =>
                {
                    scope.Dispose();
                    if (Interlocked.Decrement(ref abiertos) == 0)
                    {
                        _scope.Dispose();
                        _scope = Program.LinwayServiceProvider.CreateScope();  // renueva scope para que tome los cambios hechos en otros scopes
                        await Actualizar();
                        textBox2ImprimirRangoDesde.Visible = false;
                        textBox3ImprimirRangoHasta.Visible = false;
                        label4.Visible = false;
                        label5.Visible = false;
                        textBox3ImprimirRangoHasta.Text = "";
                        textBox2ImprimirRangoDesde.Text = "";
                        // con eventos asociados:
                        comboBox1FiltrarLista.SelectedItem = "Todas";
                        comboBox2ImprimirTipo.Text = "";
                    }
                };
                form.Show(this);
            }
            //Close();
        }
    }
}
