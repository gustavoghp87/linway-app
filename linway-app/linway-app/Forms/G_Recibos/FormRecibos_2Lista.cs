using Models;
using Models.Entities;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace linway_app.Forms
{
    public partial class FormRecibos : Form
    {
        private void ActualizarGridRecibos(ICollection<Recibo> lstRecibos)
        {
            if (lstRecibos == null)
            {
                return;
            }
            var grid1 = new List<ERecibo>();
            foreach (Recibo recibo in lstRecibos)
            {
                grid1.Add(Form1.Mapper.Map<ERecibo>(recibo));
            }
            dataGridView1.DataSource = grid1;
            dataGridView1.Columns[0].Width = 50;
            dataGridView1.Columns[1].Width = 50;
            dataGridView1.Columns[2].Width = 350;
            dataGridView1.Columns[3].Width = 80;
            lCantRecibos.Text = lstRecibos.Count.ToString() + " recibos.";
        }
        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs ev)
        {
            var lstRecibosFiltrados = new List<Recibo>();
            string opcion = comboBox1FiltrarLista.SelectedItem.ToString();
            if (opcion == "Hoy")
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
            else if (opcion == "Todas")
            {
                label2.Text = "";
                textBox1.Text = "";
                textBox1.Visible = false;
                ActualizarGridRecibos(_lstRecibos);
            }
            else if (opcion == "Impresas")
            {
                label2.Text = "";
                textBox1.Text = "";
                textBox1.Visible = false;
                foreach (Recibo recibo in _lstRecibos)
                {
                    if (recibo.Impreso == 1)
                    {
                        lstRecibosFiltrados.Add(recibo);
                    }
                }
                ActualizarGridRecibos(lstRecibosFiltrados);
            }
            else if (opcion == "No impresas")
            {
                label2.Text = "";
                textBox1.Text = "";
                textBox1.Visible = false;
                foreach (Recibo recibo in _lstRecibos)
                {
                    if (recibo.Impreso == 0)
                    {
                        lstRecibosFiltrados.Add(recibo);
                    }
                }
                ActualizarGridRecibos(lstRecibosFiltrados);
            }
            else if (opcion == "Cliente")
            {
                label2.Text = "Dirección:";
                textBox1.Text = "";
                textBox1.Visible = true;
            }
            else if (opcion == "Fecha")
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
            if (comboBox1FiltrarLista.SelectedItem.ToString() == "Cliente")
            {
                FiltrarDatos(textBox1.Text, 'c');
            }
            else if (comboBox1FiltrarLista.SelectedItem.ToString() == "Fecha")
            {
                FiltrarDatos(textBox1.Text, 'f');
            }
        }
    }
}
