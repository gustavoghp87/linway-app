﻿using linway_app.Models;
using linway_app.Models.Entities;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using static linway_app.Services.Delegates.DCliente;
using static linway_app.Services.Delegates.DDetalleRecibo;
using static linway_app.Services.Delegates.DRecibo;

namespace linway_app.Forms
{
    public partial class FormRecibos : Form
    {
        private List<Recibo> _lstRecibos;
        private List<DetalleRecibo> _lstDetallesAAgregar;
        private decimal _subTo = 0;

        public FormRecibos()
        {
            _lstRecibos = new List<Recibo>();
            _lstDetallesAAgregar = new List<DetalleRecibo>();
            InitializeComponent();
        }
        private void FormRecibos_Load(object sender, EventArgs e)
        {
            Actualizar();
        }
        private void Actualizar()
        {
            CargarRecibos();
            ActualizarGridRecibos(_lstRecibos);
        }
        private void CargarRecibos()
        {
            _lstRecibos = getRecibos();
        }
        private void ActualizarGridRecibos(List<Recibo> lstRecibos)
        {
            if (lstRecibos != null)
            {
                List<ERecibo> grid1 = new List<ERecibo>();
                foreach (Recibo recibo in lstRecibos)
                {
                    grid1.Add(Form1._mapper.Map<ERecibo>(recibo));
                }
                dataGridView1.DataSource = grid1;
                dataGridView1.Columns[0].Width = 25;
                dataGridView1.Columns[1].Width = 38;
                dataGridView1.Columns[2].Width = 350;
                lCantRecibos.Text = lstRecibos.Count.ToString() + " recibos.";
            }
        }
        private void ActualizarGridDetalles()
        {
            if (_lstDetallesAAgregar != null)
            {
                List<EDetalleRecibo> grid2 = new List<EDetalleRecibo>();
                foreach (DetalleRecibo detalleRecibo in _lstDetallesAAgregar)
                {
                    grid2.Add(Form1._mapper.Map<EDetalleRecibo>(detalleRecibo));
                }
                dataGridView2.DataSource = grid2;
                dataGridView2.Columns[0].Width = 140;
            }
        }
        private void AbrirFormImprimirRecibo(Recibo recibo)
        {
            var form = Program.GetConfig().GetRequiredService<FormImprimirRecibo>();
            form.Rellenar_Datos(recibo);
            form.Show();
        }
        private void RadioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton4.Checked) textBox7.Enabled = true;
            else textBox7.Enabled = false;
        }
        private void SoloNumeros(object sender, KeyPressEventArgs e)
        {
            if (!char.IsNumber(e.KeyChar) && e.KeyChar != (char)Keys.Back)
            {
                e.Handled = true;
                return;
            }
        }
        private void TextBox8_KeyPress(object sender, KeyPressEventArgs e)
        {
            bool IsDec = false;
            int nroDec = 0;
            if (e.KeyChar == 8)
            {
                e.Handled = false;
                return;
            }
            for (int i = 0; i < textBox8.Text.Length; i++)
            {
                if (textBox8.Text[i] == ',')
                    IsDec = true;
                if (IsDec && nroDec++ >= 2)
                {
                    e.Handled = true;
                    return;
                }
            }
            if (e.KeyChar >= 48 && e.KeyChar <= 57)
                e.Handled = false;
            else if (e.KeyChar == 44)
                e.Handled = IsDec;
            else
                e.Handled = true;
        }
        void LimpiarCampos()
        {
            radioButton1.Checked = false;
            radioButton2.Checked = false;
            radioButton3.Checked = false;
            radioButton4.Checked = false;
            textBox7.Text = "";
            textBox8.Text = "";
        }
        private void Button8_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
            button6.Enabled = false;
            label18.Text = "0";
            MessageBox.Show("button8");
            //_lstDetallesAAgregar.Clear();
            ActualizarGridDetalles();
        }
        private bool AlgunDetSeleccionado()
        {
            bool seleccionado = false;
            if (radioButton1.Checked)
                seleccionado = true;
            if (radioButton2.Checked)
                seleccionado = true;
            if (radioButton3.Checked)
                seleccionado = true;
            if (radioButton4.Checked)
                seleccionado = true;
            return seleccionado;
        }


        // ____________ filtrar datos________________
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<Recibo> lstRecibosFiltrados = new List<Recibo>();
            //todas - hoy - impresas- no impresas

            if (comboBox1.SelectedItem.ToString() == "Hoy")
            {
                label2.Text = "";
                textBox1.Text = "";
                textBox1.Visible = false;
                foreach (Recibo recibo in _lstRecibos)
                {
                    if (recibo.Fecha == DateTime.Now.ToString("yyyy-MM-dd")) lstRecibosFiltrados.Add(recibo);
                }
                ActualizarGridRecibos(lstRecibosFiltrados);
            }

            if (comboBox1.SelectedItem.ToString() == "Todas")
            {
                label2.Text = "";
                textBox1.Text = "";
                textBox1.Visible = false;
                ActualizarGridRecibos(_lstRecibos);
            }

            if (comboBox1.SelectedItem.ToString() == "Impresas")
            {
                label2.Text = "";
                textBox1.Text = "";
                textBox1.Visible = false;
                foreach (Recibo recibo in _lstRecibos)
                {
                    if (recibo.Impreso == 1) lstRecibosFiltrados.Add(recibo);
                }
                ActualizarGridRecibos(lstRecibosFiltrados);
            }

            if (comboBox1.SelectedItem.ToString() == "No impresas")
            {
                label2.Text = "";
                textBox1.Text = "";
                textBox1.Visible = false;
                foreach (Recibo recibo in _lstRecibos)
                {
                    if (recibo.Impreso == 0) lstRecibosFiltrados.Add(recibo);
                }
                ActualizarGridRecibos(lstRecibosFiltrados);
            }

            if (comboBox1.SelectedItem.ToString() == "Cliente")
            {
                label2.Text = "Dirección:";
                textBox1.Text = "";
                textBox1.Visible = true;
            }

            if (comboBox1.SelectedItem.ToString() == "Fecha")
            {
                label2.Text = "Fecha:";
                textBox1.Text = "";
                textBox1.Visible = true;
            }
        }
        void FiltrarDatos(string texto, char x)
        {
            List<Recibo> lstFiltrados = new List<Recibo>();
            foreach (Recibo recibo in _lstRecibos)
            {
                if (x == 'c' && recibo.DireccionCliente.ToLower().Contains(texto.ToLower()))
                    lstFiltrados.Add(recibo);
                if (x == 'f' && recibo.Fecha.Contains(texto))
                    lstFiltrados.Add(recibo);
            }
            ActualizarGridRecibos(lstFiltrados);
        }
        private void TextBox1_TextChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem.ToString() == "Cliente")
                FiltrarDatos(textBox1.Text, 'c');
            if (comboBox1.SelectedItem.ToString() == "Fecha")
                FiltrarDatos(textBox1.Text, 'f');
        }

        
        //_____________grupo imprimir_______________
        private void Button1_Click(object sender, EventArgs e)
        {
            if (comboBox2.Text != "")
            {
                foreach (Recibo recibo in ObtenerListaAImprimir())
                {
                    AbrirFormImprimirRecibo(recibo);
                }
                Close();
            }
        }
        private void TextBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsNumber(e.KeyChar) && e.KeyChar != (char)Keys.Back)
            {
                e.Handled = true;
                return;
            }
        }
        private void TextBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsNumber(e.KeyChar) && e.KeyChar != (char)Keys.Back)
            {
                e.Handled = true;
                return;
            }
        }
        private void TextBox5_TextChanged(object sender, EventArgs e)
        {
            label10.Text = ObtenerListaABorrar().Count.ToString();
        }
        private void TextBox4_TextChanged(object sender, EventArgs e)
        {
            label10.Text = ObtenerListaABorrar().Count.ToString();
        }
        private List<Recibo> ObtenerListaAImprimir()
        {
            List<Recibo> listaAImprimir = new List<Recibo>();
            if (_lstRecibos == null) return listaAImprimir;

            if (comboBox2.SelectedItem.ToString() == "No impresas")
            {
                foreach (Recibo recibo in _lstRecibos)
                {
                    if (recibo.Impreso == 0) listaAImprimir.Add(recibo);
                }
            }

            if (comboBox2.SelectedItem.ToString() == "Hoy")
            {
                foreach (Recibo recibo in _lstRecibos)
                {
                    if (recibo.Fecha == DateTime.Now.ToString("yyy-MM-dd")) listaAImprimir.Add(recibo);
                }
            }

            if (textBox2.Text != "" && textBox3.Text != "" && comboBox2.SelectedItem.ToString() == "Establecer rango")
            {
                try
                {
                    long menor = long.Parse(textBox2.Text);
                    long mayor = long.Parse(textBox3.Text);
                    if (menor <= mayor)
                    {
                        for (long i = menor; i <= mayor; i++)
                        {
                            listaAImprimir.Add(_lstRecibos.Find(x => x.Id == i));
                        }
                    }
                }
                catch { MessageBox.Show("Rango establecido incorrecto"); }
            }
            return listaAImprimir;
        }
        private void ComboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (
                comboBox2.SelectedItem.ToString() == "No impresas"
                || (comboBox2.SelectedItem.ToString() == "Hoy"))
            {
                textBox2.Visible = false;
                textBox3.Visible = false;
                label4.Visible = false;
                label5.Visible = false;
                textBox3.Text = "";
                textBox2.Text = "";
            }
            if (comboBox2.SelectedItem.ToString() == "Establecer rango")
            {
                textBox2.Visible = true;
                textBox3.Visible = true;
                label4.Visible = true;
                label5.Visible = true;
            }
            label7.Text = ObtenerListaAImprimir().Count.ToString();
        }
        private void TextBox3_TextChanged(object sender, EventArgs e)
        {
            label7.Text = ObtenerListaAImprimir().Count.ToString();
        }
        private void TextBox2_TextChanged(object sender, EventArgs e)
        {
            label7.Text = ObtenerListaAImprimir().Count.ToString();
        }

        // AGREGAR RECIBO
        private void ClienteId_TextChanged(object sender, EventArgs e)
        {
            if (textBox6.Text != "")
            {
                try { long.Parse(textBox6.Text); } catch { return; };
                long id = long.Parse(textBox6.Text);
                Cliente cliente = getCliente(id);
                if (cliente != null)
                {
                    label15.Text = cliente.Direccion;
                    if (_lstDetallesAAgregar.Count != 0)
                    {
                        button6.Enabled = true;
                    }
                }
                else
                {
                    label15.Text = "No encontrado";
                    button6.Enabled = false;
                }
            }
            else
            {
                label15.Text = "";
                button6.Enabled = false;
            }
        }
        private void TextBox9_TextChanged(object sender, EventArgs e)
        {
            if (textBox9.Text != "")
            {
                Cliente cliente = getClientePorDireccion(textBox9.Text);
                if (cliente != null)
                {
                    label15.Text = cliente.Direccion;
                    if (_lstDetallesAAgregar.Count != 0)
                        button6.Enabled = true;
                }
                else
                {
                    label15.Text = "No encontrado";
                    button6.Enabled = false;
                }
            }
            else
            {
                label15.Text = "";
                button6.Enabled = false;
            }
        }
        private void AgregarDetalle_Click(object sender, EventArgs e)
        {
            if (textBox8.Text != "" && AlgunDetSeleccionado())
            {
                try { decimal.Parse(textBox8.Text); } catch { return; };
                decimal importe = decimal.Parse(textBox8.Text);
                DetalleRecibo nuevoDetalle = new DetalleRecibo();
                if (radioButton1.Checked)
                {
                    nuevoDetalle.Detalle = "Saldo a Favor";
                    nuevoDetalle.Importe = importe * -1;
                }
                if (radioButton2.Checked)
                {
                    nuevoDetalle.Detalle = "Desc. por devol.";
                    nuevoDetalle.Importe = importe * -1;
                }
                if (radioButton3.Checked)
                {
                    nuevoDetalle.Detalle = "Saldo pendiente";
                    nuevoDetalle.Importe = importe;
                }
                if (radioButton4.Checked)
                {
                    nuevoDetalle.Detalle = "Factura N° " + textBox7.Text;
                    nuevoDetalle.Importe = importe;
                }
                _lstDetallesAAgregar.Add(nuevoDetalle);
                ActualizarGridDetalles();

                _subTo = 0;
                foreach (DetalleRecibo recibo in _lstDetallesAAgregar)
                {
                    _subTo += recibo.Importe;
                }
                label18.Text = _subTo.ToString();
                LimpiarCampos();

                if (label15.Text != "" && label15.Text != "No encontrado")
                {
                    button6.Enabled = true;
                }
            }
            else MessageBox.Show("Complete correctamente los campos.");
        }
        private void Limpiar_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
            textBox6.Text = "";
            textBox8.Text = "";
            textBox9.Text = "";
            label15.Text = "";
            button6.Enabled = false;
            label18.Text = "0";
            _lstDetallesAAgregar.Clear();
            ActualizarGridDetalles();
        }
        private void CrearRecibo_Click(object sender, EventArgs e)
        {
            CargarRecibos();
            Cliente cliente = getClientePorDireccionExacta(label15.Text);
            if (cliente == null) return;

            Recibo nuevoRecibo = new Recibo();
            nuevoRecibo.ClienteId = cliente.Id;
            nuevoRecibo.DireccionCliente = label15.Text;
            nuevoRecibo.ImporteTotal = _subTo;
            nuevoRecibo.Impreso = 0;
            nuevoRecibo.Fecha = DateTime.Now.ToString("yyyy-MM-dd");
            long reciboId = addReciboReturnId(nuevoRecibo);
            if (reciboId == 0) { MessageBox.Show("Algo falló en el proceso"); return; }
            foreach (DetalleRecibo detalle in _lstDetallesAAgregar)
            {
                detalle.ReciboId = reciboId;
                addDetalleRecibo(detalle);
            }
            
            LimpiarCampos();
            textBox6.Text = "";
            textBox8.Text = "";
            label15.Text = "";
            button6.Enabled = false;
            label18.Text = "0";
            _lstDetallesAAgregar.Clear();
            Actualizar();
        }


        //BORRAR RECIBOS
        private void ComboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (
                comboBox3.SelectedItem.ToString() == "Impresas"
                || (comboBox3.SelectedItem.ToString() == "Todas"
                || comboBox3.SelectedItem.ToString() == "(Seleccionar)")
            )
            {
                textBox4.Visible = false;
                textBox5.Visible = false;
                label13.Visible = false;
                label12.Visible = false;
                textBox4.Text = "";
                textBox5.Text = "";
            }
            if (comboBox3.SelectedItem.ToString() == "Establecer rango")
            {
                textBox4.Visible = true;
                textBox5.Visible = true;
                label13.Visible = true;
                label12.Visible = true;
            }
            label10.Text = ObtenerListaABorrar().Count.ToString();
        }
        private List<Recibo> ObtenerListaABorrar()
        {
            List<Recibo> listaABorrar = new List<Recibo>();
            if (textBox5.Text != "" && textBox4.Text != "")
            {
                if (comboBox3.SelectedItem.ToString() == "Establecer rango")
                {
                    try
                    {
                        long menor = long.Parse(textBox5.Text);
                        long mayor = long.Parse(textBox4.Text);
                        if (menor <= mayor)
                        {
                            for (long i = menor; i <= mayor; i++) listaABorrar.Add(_lstRecibos.Find(x => x.Id == i));
                        }
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Rango establecido incorrecto");
                    }
                }
            }
            else if (comboBox3.SelectedItem.ToString() == "Todas")
            {
                listaABorrar.AddRange(_lstRecibos);
            }
            else if (comboBox3.SelectedItem.ToString() == "Impresas")
            {
                foreach (Recibo recibo in _lstRecibos)
                {
                    if (recibo.Impreso == 1) listaABorrar.Add(recibo);
                }
            }
            return listaABorrar;
        }
        private void Button3_Click(object sender, EventArgs e)
        {
            if (
                comboBox3.SelectedItem.ToString() == "Todas"
                || comboBox3.SelectedItem.ToString() == "Establecer rango"
                || comboBox3.SelectedItem.ToString() == "Impresas"
            )
            {
                MessageBox.Show("Confirme si desea borrar los recibos seleccionados");
                label11.Visible = true;
                button4.Visible = true;
                button5.Visible = true;
                button3.Visible = false;
            }
        }
        private void Button5_Click(object sender, EventArgs e)
        {
            comboBox3.SelectedItem = "(Seleccionar)";
            label11.Visible = false;
            button4.Visible = false;
            button5.Visible = false;
            button3.Visible = true;
        }
        private void Button4_Click(object sender, EventArgs e)
        {
            foreach (Recibo recibo in ObtenerListaABorrar())
            {
                foreach(DetalleRecibo detalle in recibo.DetalleRecibos)
                {
                    deleteDetalleRecibo(detalle);
                }
                deleteRecibo(recibo);
            }
            Actualizar();
            comboBox3.SelectedItem = "(Seleccionar)";
            label11.Visible = false;
            button4.Visible = false;
            button5.Visible = false;
            button3.Visible = true;
            lCantRecibos.Text = _lstRecibos.Count.ToString() + " recibos.";
            ActualizarGridRecibos(_lstRecibos);
        }

    }
}
