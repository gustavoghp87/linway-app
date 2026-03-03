using Microsoft.Extensions.DependencyInjection;
using Models;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace linway_app.Forms
{
    public partial class FormNotasEnvio : Form
    {
        private void Button1_Click(object sender, EventArgs ev)
        {
            if (comboBox2.Text == "")
            {
                return;
            }
            var lstAImprimir = ObtenerListaAImprimir();
            foreach (NotaDeEnvio nota in lstAImprimir)
            {
                var form = Program.LinwayServiceProvider.GetRequiredService<FormImprimirNota>();
                form.Rellenar_Datos(nota);
                form.Show();
                form.BringToFront();
            }
            SendToBack();
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
        private List<NotaDeEnvio> ObtenerListaAImprimir()
        {
            var listaAImprimir = new List<NotaDeEnvio>();
            if (comboBox2.SelectedItem.ToString() == "No impresas")
            {
                foreach (NotaDeEnvio nota in _lstNotaDeEnvios)
                {
                    if (nota.Impresa == 0)
                    {
                        listaAImprimir.Add(nota);
                    }
                }
            }
            else if (comboBox2.SelectedItem.ToString() == "Hoy")
            {
                foreach (NotaDeEnvio nota in _lstNotaDeEnvios)
                {
                    if (nota.Fecha == DateTime.Now.ToString(Constants.FormatoDeFecha))
                    {
                        listaAImprimir.Add(nota);
                    }
                }
            }
            else if (comboBox2.SelectedItem.ToString() == "Establecer rango" && textBox2.Text != "" && textBox3.Text != "")
            {
                try
                {
                    int j = int.Parse(textBox3.Text);
                    for (int i = int.Parse(textBox2.Text); i <= j; i++)
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
