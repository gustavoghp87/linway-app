using Microsoft.Extensions.DependencyInjection;
using Models;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace linway_app.Forms
{
    public partial class FormRecibos : Form
    {
        //private void RadioButton1_CheckedChanged(object sender, EventArgs ev)
        //{
        //    textBox7.Enabled = radioButton4.Checked;
        //}
        //_____________grupo imprimir_______________
        private void Button1_Click(object sender, EventArgs ev)
        {
            if (comboBox2.Text == "")
            {
                return;
            }
            int i = 0;
            foreach (Recibo recibo in ObtenerListaAImprimir())
            {
                if (recibo == null)
                {
                    continue;
                }
                i++;
                if (i < 21)
                {
                    var form = Program.LinwayServiceProvider.GetRequiredService<FormImprimirRecibo>();
                    form.Rellenar_Datos(recibo);
                    form.Show(this);
                }
            }
            Close();
        }
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
            var listaAImprimir = new List<Recibo>();
            if (_lstRecibos == null)
            {
                return listaAImprimir;
            }
            if (comboBox2.SelectedItem.ToString() == "No impresas")
            {
                foreach (Recibo recibo in _lstRecibos)
                {
                    if (recibo != null && recibo.Impreso == 0)
                    {
                        listaAImprimir.Add(recibo);
                    }
                }
            }
            else if (comboBox2.SelectedItem.ToString() == "Hoy")
            {
                foreach (Recibo recibo in _lstRecibos)
                {
                    if (recibo != null && recibo.Fecha == DateTime.Now.ToString("yyy-MM-dd"))
                    {
                        listaAImprimir.Add(recibo);
                    }
                }
            }
            else if (textBox2.Text != "" && textBox3.Text != "" && comboBox2.SelectedItem.ToString() == "Establecer rango")
            {
                try
                {
                    long menor = long.Parse(textBox2.Text);
                    long mayor = long.Parse(textBox3.Text);
                    if (menor <= mayor)
                    {
                        for (long i = menor; i <= mayor; i++)
                        {
                            Recibo recibo = _lstRecibos.Find(x => x.Id == i);
                            if (recibo == null)
                            {
                                continue;
                            }
                            listaAImprimir.Add(recibo);
                        }
                    }
                }
                catch {
                    MessageBox.Show("Rango establecido incorrecto");
                }
            }
            return listaAImprimir;
        }
        private void ComboBox2_SelectedIndexChanged(object sender, EventArgs ev)
        {
            if (comboBox2.SelectedItem.ToString() == "No impresas" || comboBox2.SelectedItem.ToString() == "Hoy")
            {
                textBox2.Visible = false;
                textBox3.Visible = false;
                label4.Visible = false;
                label5.Visible = false;
                textBox3.Text = "";
                textBox2.Text = "";
            }
            else if (comboBox2.SelectedItem.ToString() == "Establecer rango")
            {
                textBox2.Visible = true;
                textBox3.Visible = true;
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
    }
}
