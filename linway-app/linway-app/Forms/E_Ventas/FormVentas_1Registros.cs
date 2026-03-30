using linway_app.Services.FormServices;
using Models;
using Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace linway_app.Forms
{
    public partial class FormVentas : Form
    {
        private void ActualizarGrid1Registros(ICollection<RegistroVenta> lstRegistroVentas)
        {
            var grid = new List<ERegistroVenta>();
            lstRegistroVentas.ToList().ForEach(x =>
                grid.Add(Form1.Mapper.Map<ERegistroVenta>(x))
            );
            dataGridView1.DataSource = grid;
            dataGridView1.Columns[0].Width = 48;
            dataGridView1.Columns[1].Width = 240;
            label1.Text = "Registro de ventas (" + lstRegistroVentas.Count.ToString() + ")";
        }
        private void ComboBox3_SelectedIndexChanged(object sender, EventArgs ev)
        {
            var lFiltrada = new List<RegistroVenta>();
            string opcion = comboBox3.SelectedItem.ToString();
            if (opcion == "Hoy")
            {
                textBox2RegistrosFiltro.Text = "";
                textBox2RegistrosFiltro.Visible = false;
                foreach (RegistroVenta rActual in _lstRegistros)
                {
                    if (rActual.Fecha == DateTime.Now.ToString(Constants.FormatoDeFecha))
                    {
                        lFiltrada.Add(rActual);
                    }
                }
                ActualizarGrid1Registros(lFiltrada);
            }
            else if (opcion == "Todas")
            {
                textBox2RegistrosFiltro.Text = "";
                textBox2RegistrosFiltro.Visible = false;
                ActualizarGrid1Registros(_lstRegistros);
            }
            else
            {
                textBox2RegistrosFiltro.Text = "";
                textBox2RegistrosFiltro.Visible = true;
            }
        }
        private void TextBox2_TextChanged(object sender, EventArgs ev)
        {
            if (comboBox3.SelectedItem.ToString() == "Cliente")
            {
                FiltrarDatos(textBox2RegistrosFiltro.Text, 'c');
            }
            if (comboBox3.SelectedItem.ToString() == "Fecha")
            {
                FiltrarDatos(textBox2RegistrosFiltro.Text, 'f');
            }
        }
        private void FiltrarDatos(string texto, char x)
        {
            texto = texto.Trim().ToLower();
            var ListaFiltrada = new List<RegistroVenta>();
            foreach (RegistroVenta rActual in _lstRegistros)
            {
                string nombreCliente = rActual.NombreCliente.Trim().ToLower();
                if (x == 'c')
                {
                    if (nombreCliente.Contains(texto))
                    {
                        ListaFiltrada.Add(rActual);
                    }
                    else
                    {
                        List<string> nombres = Helpers.IgnorarTildes(nombreCliente);
                        if (nombres == null || nombres.Count == 0)
                        {
                            continue;
                        }
                        foreach (var nombre in nombres)
                        {
                            if (nombreCliente.Contains(nombre))
                            {
                                ListaFiltrada.Add(rActual);
                                break;
                            }
                        }
                    }
                }
                else if (x == 'f')
                {
                    if (rActual.Fecha.Contains(texto))
                    {
                        ListaFiltrada.Add(rActual);
                    }
                }
            }
            ActualizarGrid1Registros(ListaFiltrada);
        }
    }
}
