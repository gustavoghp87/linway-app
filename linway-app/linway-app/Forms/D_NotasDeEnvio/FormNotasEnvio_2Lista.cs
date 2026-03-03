using Models;
using Models.Entities;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace linway_app.Forms
{
    public partial class FormNotasEnvio : Form
    {
        private void ActualizarGrid1(ICollection<NotaDeEnvio> lstNotadeEnvios)
        {
            if (lstNotadeEnvios == null)
            {
                return;
            }
            var grid = new List<ENotaDeEnvio>();
            foreach (NotaDeEnvio nota in lstNotadeEnvios)
            {
                grid.Add(Form1.Mapper.Map<ENotaDeEnvio>(nota));
            }
            dataGridView1.DataSource = grid;
            //foreach (var item in dataGridView1.Rows)   muy lento
            //{
            //    ((DataGridViewRow)item).Height = 35;
            //}
            dataGridView1.Columns[0].Width = 40;
            dataGridView1.Columns[1].Width = 70;
            dataGridView1.Columns[2].Width = 170;
            dataGridView1.Columns[3].Width = 320;
            dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;        // lento
            comboBox1.SelectedItem = "Todas ??";
            comboBox3.SelectedIndexChanged -= ComboBox3_SelectedIndexChanged;  // evita error de concurrencia de DbContext
            comboBox3.SelectedItem = "(Seleccionar)";
            comboBox3.SelectedIndexChanged += ComboBox3_SelectedIndexChanged;
        }
        private async void ComboBox1_SelectedIndexChanged(object sender, EventArgs ev)
        {
            var lFiltrada = new List<NotaDeEnvio>();
            //todas - hoy - impresas- no impresas
            if (comboBox1.SelectedItem.ToString() == "Hoy")
            {
                label2.Text = "";
                textBox1.Text = "";
                textBox1.Visible = false;
                foreach (NotaDeEnvio nota in _lstNotaDeEnvios)
                {
                    if (nota.Fecha == DateTime.Now.ToString(Constants.FormatoDeFecha))
                    {
                        lFiltrada.Add(nota);
                    }
                }
                ActualizarGrid1(lFiltrada);
            }
            else if (comboBox1.SelectedItem.ToString() == "Todas")
            {
                label2.Text = "";
                textBox1.Text = "";
                textBox1.Visible = false;
                await ActualizarNotas();
                ActualizarGrid1(_lstNotaDeEnvios);
            }
            else if (comboBox1.SelectedItem.ToString() == "Impresas")
            {
                label2.Text = "";
                textBox1.Text = "";
                textBox1.Visible = false;
                foreach (NotaDeEnvio nota in _lstNotaDeEnvios)
                {
                    if (nota.Impresa == 1)
                    {
                        lFiltrada.Add(nota);
                    }
                }
                ActualizarGrid1(lFiltrada);
            }
            else if (comboBox1.SelectedItem.ToString() == "No impresas")
            {
                label2.Text = "";
                textBox1.Text = "";
                textBox1.Visible = false;
                foreach (NotaDeEnvio nota in _lstNotaDeEnvios)
                {
                    if (nota.Impresa == 0)
                    {
                        lFiltrada.Add(nota);
                    }
                }
                ActualizarGrid1(lFiltrada);
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
        private async void FiltrarDatos(string texto, char x)
        {
            await ActualizarNotas();
            var lstFiltrada = new List<NotaDeEnvio>();
            foreach (NotaDeEnvio nota in _lstNotaDeEnvios)
            {
                if (x == 'c' && nota.Cliente != null && nota.Cliente.Direccion != null && nota.Cliente.Direccion.ToLower().Contains(texto.ToLower()))
                {
                    lstFiltrada.Add(nota);
                }
                else if (x == 'f' && nota.Fecha.Contains(texto))
                {
                    lstFiltrada.Add(nota);
                }
            }
            ActualizarGrid1(lstFiltrada);
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
