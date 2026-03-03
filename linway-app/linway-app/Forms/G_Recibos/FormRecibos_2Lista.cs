using Models;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace linway_app.Forms
{
    public partial class FormRecibos : Form
    {
        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs ev)
        {
            var lstRecibosFiltrados = new List<Recibo>();
            if (comboBox1.SelectedItem.ToString() == "Hoy")
            {
                label2.Text = "";
                textBox1.Text = "";
                textBox1.Visible = false;
                foreach (Recibo recibo in _lstRecibos)
                {
                    if (recibo.Fecha == DateTime.Now.ToString(Constants.FormatoDeFecha))
                    {
                        lstRecibosFiltrados.Add(recibo);
                    }
                }
                ActualizarGridRecibos(lstRecibosFiltrados);
            }
            else if (comboBox1.SelectedItem.ToString() == "Todas")
            {
                label2.Text = "";
                textBox1.Text = "";
                textBox1.Visible = false;
                ActualizarGridRecibos(_lstRecibos);
            }
            else if (comboBox1.SelectedItem.ToString() == "Impresas")
            {
                label2.Text = "";
                textBox1.Text = "";
                textBox1.Visible = false;
                foreach (Recibo recibo in _lstRecibos)
                {
                    if (recibo.Impreso == 1) {
                        lstRecibosFiltrados.Add(recibo);
                    }
                }
                ActualizarGridRecibos(lstRecibosFiltrados);
            }
            else if (comboBox1.SelectedItem.ToString() == "No impresas")
            {
                label2.Text = "";
                textBox1.Text = "";
                textBox1.Visible = false;
                foreach (Recibo recibo in _lstRecibos)
                {
                    if (recibo.Impreso == 0) {
                        lstRecibosFiltrados.Add(recibo);
                    }
                }
                ActualizarGridRecibos(lstRecibosFiltrados);
            }
            else if (comboBox1.SelectedItem.ToString() == "Cliente")
            {
                label2.Text = "Dirección:";
                textBox1.Text = "";
                textBox1.Visible = true;
            }
            else if (comboBox1.SelectedItem.ToString() == "Fecha")
            {
                label2.Text = "Fecha:";
                textBox1.Text = "";
                textBox1.Visible = true;
            }
        }
        private void FiltrarDatos(string texto, char x)
        {
            var lstFiltrados = new List<Recibo>();
            foreach (Recibo recibo in _lstRecibos)
            {
                if (x == 'c' && recibo.DireccionCliente.ToLower().Contains(texto.ToLower()))
                {
                    lstFiltrados.Add(recibo);
                }
                else if (x == 'f' && recibo.Fecha.Contains(texto))
                {
                    lstFiltrados.Add(recibo);
                }
            }
            ActualizarGridRecibos(lstFiltrados);
        }
        private void TextBox1_TextChanged(object sender, EventArgs ev)
        {
            if (comboBox1.SelectedItem.ToString() == "Cliente")
            {
                FiltrarDatos(textBox1.Text, 'c');
            }
            else if (comboBox1.SelectedItem.ToString() == "Fecha")
            {
                FiltrarDatos(textBox1.Text, 'f');
            }
        }
    }
}
