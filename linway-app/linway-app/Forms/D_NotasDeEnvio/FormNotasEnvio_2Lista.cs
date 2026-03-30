using Models;
using Models.Entities;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace linway_app.Forms
{
    public partial class FormNotasEnvio : Form
    {
        private void ActualizarGrid1(ICollection<NotaDeEnvio> lstNotaDeEnvios)
        {
            var grid = new List<ENotaDeEnvio>();
            foreach (NotaDeEnvio nota in lstNotaDeEnvios)
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
            labelCantidadDeNotas.Text = lstNotaDeEnvios.Count.ToString() + " notas de envio.";
        }
        private void EventoCombobox1ListaModalidad()
        {
            string opcion = comboBox1ListaModalidad.SelectedItem?.ToString();  // todas - hoy - impresas - no impresas - fecha - cliente
            if (opcion == "Hoy")
            {
                label2FiltrarPorDireccionOFecha.Text = "";
                //
                textBox1ListaCampoClienteOFecha.Text = "";
                textBox1ListaCampoClienteOFecha.Visible = false;
                //
                ActualizarGrid1(_lstNotaDeEnvios.FindAll(n => n.Fecha == DateTime.Now.ToString(Constants.FormatoDeFecha)));
            }
            else if (opcion == "Todas")
            {
                label2FiltrarPorDireccionOFecha.Text = "";
                //
                textBox1ListaCampoClienteOFecha.Text = "";
                textBox1ListaCampoClienteOFecha.Visible = false;
                //
                ActualizarGrid1(_lstNotaDeEnvios);
            }
            else if (opcion == "Impresas")
            {
                label2FiltrarPorDireccionOFecha.Text = "";
                //
                textBox1ListaCampoClienteOFecha.Text = "";
                textBox1ListaCampoClienteOFecha.Visible = false;
                //
                ActualizarGrid1(_lstNotaDeEnvios.FindAll(n => n.Impresa == 1));
            }
            else if (opcion == "No impresas")
            {
                label2FiltrarPorDireccionOFecha.Text = "";
                //
                textBox1ListaCampoClienteOFecha.Text = "";
                textBox1ListaCampoClienteOFecha.Visible = false;
                //
                ActualizarGrid1(_lstNotaDeEnvios.FindAll(n => n.Impresa == 0));
            }
            else if (opcion == "Cliente")
            {
                label2FiltrarPorDireccionOFecha.Text = "Dirección:";
                //
                //textBox1ListaCampoClienteOFecha.Text = "";
                textBox1ListaCampoClienteOFecha.Visible = true;
            }
            else if (opcion == "Fecha")
            {
                label2FiltrarPorDireccionOFecha.Text = "Fecha:";
                //
                //textBox1ListaCampoClienteOFecha.Text = "";
                textBox1ListaCampoClienteOFecha.Visible = true;
            }
        }
        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs ev)
        {
            EventoCombobox1ListaModalidad();
        }
        private void TextBox1_TextChanged(object sender, EventArgs ev)
        {
            // filtrar datos
            string opcion = comboBox1ListaModalidad.SelectedItem.ToString();
            string texto = textBox1ListaCampoClienteOFecha.Text;
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
