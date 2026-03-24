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
            dataGridView1.Columns[0].Width = 50;
            dataGridView1.Columns[1].Width = 70;
            dataGridView1.Columns[2].Width = 210;
            dataGridView1.Columns[3].Width = 380;
            dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;        // lento
            //
            comboBox1.SelectedIndexChanged -= ComboBox1_SelectedIndexChanged;  // evita error de concurrencia de DbContext
            comboBox1.SelectedItem = "Todas ??";
            comboBox1.SelectedIndexChanged += ComboBox1_SelectedIndexChanged;
            //
            comboBox3.SelectedIndexChanged -= ComboBox3_SelectedIndexChanged;  // evita error de concurrencia de DbContext
            comboBox3.SelectedItem = "(Seleccionar)";
            comboBox3.SelectedIndexChanged += ComboBox3_SelectedIndexChanged;
        }
        private async void ComboBox1_SelectedIndexChanged(object sender, EventArgs ev)
        {
            string opcion = comboBox1.SelectedItem.ToString();  // todas - hoy - impresas- no impresas
            var lFiltrada = new List<NotaDeEnvio>();
            if (opcion == "Hoy")
            {
                label2FiltrarPorDireccionOFecha.Text = "";
                //
                textBox1.TextChanged -= TextBox1_TextChanged;  // evita error de concurrencia de DbContext
                textBox1.Text = "";
                textBox1.Visible = false;
                textBox1.TextChanged += TextBox1_TextChanged;
                //
                foreach (NotaDeEnvio nota in _lstNotaDeEnvios)
                {
                    if (nota.Fecha == DateTime.Now.ToString(Constants.FormatoDeFecha))
                    {
                        lFiltrada.Add(nota);
                    }
                }
                ActualizarGrid1(lFiltrada);
            }
            else if (opcion == "Todas")
            {
                label2FiltrarPorDireccionOFecha.Text = "";
                //
                textBox1.TextChanged -= TextBox1_TextChanged;  // evita error de concurrencia de DbContext
                textBox1.Text = "";
                textBox1.Visible = false;
                textBox1.TextChanged += TextBox1_TextChanged;
                //
                await ActualizarNotas();
                ActualizarGrid1(_lstNotaDeEnvios);
            }
            else if (opcion == "Impresas")
            {
                label2FiltrarPorDireccionOFecha.Text = "";
                //
                textBox1.TextChanged -= TextBox1_TextChanged;  // evita error de concurrencia de DbContext
                textBox1.Text = "";
                textBox1.Visible = false;
                textBox1.TextChanged += TextBox1_TextChanged;
                //
                foreach (NotaDeEnvio nota in _lstNotaDeEnvios)
                {
                    if (nota.Impresa == 1)
                    {
                        lFiltrada.Add(nota);
                    }
                }
                ActualizarGrid1(lFiltrada);
            }
            else if (opcion == "No impresas")
            {
                label2FiltrarPorDireccionOFecha.Text = "";
                //
                textBox1.TextChanged -= TextBox1_TextChanged;  // evita error de concurrencia de DbContext
                textBox1.Text = "";
                textBox1.Visible = false;
                textBox1.TextChanged += TextBox1_TextChanged;
                //
                foreach (NotaDeEnvio nota in _lstNotaDeEnvios)
                {
                    if (nota.Impresa == 0)
                    {
                        lFiltrada.Add(nota);
                    }
                }
                ActualizarGrid1(lFiltrada);
            }
            else if (opcion == "Cliente")
            {
                label2FiltrarPorDireccionOFecha.Text = "Dirección:";
                //
                textBox1.TextChanged -= TextBox1_TextChanged;  // evita error de concurrencia de DbContext
                textBox1.Text = "";
                textBox1.Visible = true;
                textBox1.TextChanged += TextBox1_TextChanged;
                //
            }
            else if (opcion== "Fecha")
            {
                label2FiltrarPorDireccionOFecha.Text = "Fecha:";
                //
                textBox1.TextChanged -= TextBox1_TextChanged;  // evita error de concurrencia de DbContext
                textBox1.Text = "";
                textBox1.Visible = true;
                textBox1.TextChanged += TextBox1_TextChanged;
                //
            }
        }
        private async void TextBox1_TextChanged(object sender, EventArgs ev)
        {
            // filtrar datos
            string opcion = comboBox1.SelectedItem.ToString();
            string texto = textBox1.Text;
            char x;
            if (opcion == "Cliente")
            {
                x = 'c';
            }
            else if (opcion == "Fecha")
            {
                x = 'f';
            }
            else
            {
                return;
            }
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
    }
}
