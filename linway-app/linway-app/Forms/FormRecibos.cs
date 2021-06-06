﻿using linway_app.Models;
using linway_app.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace linway_app.Forms
{
    public partial class FormRecibos : Form
    {
        private List<Recibo> _lstRecibos = new List<Recibo>();
        private List<DetalleRecibo> _lstDetallesAAgregar = new List<DetalleRecibo>();
        private List<Cliente> _lstClientes = new List<Cliente>();
        private readonly IServicioCliente _servCliente;
        private readonly IServicioRecibo _servRecibo;
        private readonly IServicioDetalleRecibo _servDetalleRecibo;
        private double _subTo = 0;

        public FormRecibos(IServicioCliente servCliente, IServicioRecibo servRecibo, IServicioDetalleRecibo servDetalleRecibo)
        {
            InitializeComponent();
            _servCliente = servCliente;
            _servRecibo = servRecibo;
            _servDetalleRecibo = servDetalleRecibo;
        }
        private void FormRecibos_Load(object sender, EventArgs e)
        {
            Actualizar();
        }
        private void Actualizar()
        {
            CargarRecibos();
            CargarClientes();
            ActualizarGridRecibos(_lstRecibos);
        }
        private void CargarRecibos()
        {
            _lstRecibos = _servRecibo.GetAll();
        }
        private void CargarClientes()
        {
            _lstClientes = _servCliente.GetAll();
        }
        private void ActualizarGridRecibos(List<Recibo> recibos)
        {
            if (recibos != null)
            {
                List<Recibo> grid1 = new List<Recibo>();
                foreach (Recibo recibo in recibos)
                {
                    grid1.Add(new Recibo
                    {
                        Id = recibo.Id,
                        Fecha = recibo.Fecha,
                        DireccionCliente = recibo.DireccionCliente,
                        ImporteTotal = recibo.ImporteTotal,
                        Impresa = recibo.Impresa
                        //== 0 ? "No" : "Sí"
                    });
                }
                dataGridView1.DataSource = grid1.ToArray();
            }
            lCantRecibos.Text = recibos.Count.ToString() + " recibos.";
        }
        private void ActualizarGridDetalles()
        {
            if (_lstDetallesAAgregar != null)
            {
                dataGridView2.DataSource = _lstDetallesAAgregar.ToArray();
                dataGridView2.Columns[1].Width = 55;
            }
        }
        private long GuardarRecibo(Recibo recibo)
        {
            bool response = _servRecibo.Add(recibo);
            if (!response) MessageBox.Show("Algo falló al guardar el Recibo en la base de datos");
            var response1 = _servRecibo.GetAll();
            var last = response1[response1.Count - 1];
            return last.Id;
        }
        private void EliminarRecibo(Recibo recibo)
        {
            bool response = _servRecibo.Delete(recibo);
            if (!response) MessageBox.Show("Algo falló al eliminar el Recibo de la base de datos");
        }
        private void AgregarDetalle(DetalleRecibo detalle)
        {
            bool response = _servDetalleRecibo.Add(detalle);
            if (!response) MessageBox.Show("Algo falló al guardar el Detalle de Recibo en la base de datos");
        }
        private void AbrirFormImprimirRecibo(Recibo recibo)
        {
            var form = Program.GetConfig().GetRequiredService<FormImprimirRecibo>();
            form.Rellenar_Datos(recibo);
            form.Show();
        }
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton4.Checked)
            {
                textBox7.Enabled = true;
            }
            else
            {
                textBox7.Enabled = false;
            }
        }
        private void SoloNumeros(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsNumber(e.KeyChar)) && (e.KeyChar != (char)Keys.Back))
            {
                e.Handled = true;
                return;
            }
        }
        private void textBox8_KeyPress(object sender, KeyPressEventArgs e)
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
        private void button8_Click(object sender, EventArgs e)
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
        private void bCopiaSeguridad_Click(object sender, EventArgs e)
        {
        }
        private void Importar_Click(object sender, EventArgs e)
        {
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
                    if (recibo.Fecha == DateTime.Now.ToString("yyyy-MM-dd"))
                    {
                        lstRecibosFiltrados.Add(recibo);
                    }
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
                    if (recibo.Impresa == 1)
                    {
                        lstRecibosFiltrados.Add(recibo);
                    }
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
                    if (recibo.Impresa == 0)
                    {
                        lstRecibosFiltrados.Add(recibo);
                    }
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
                if (x == 'c')
                {
                    if (recibo.DireccionCliente.ToLower().Contains(texto.ToLower()))
                    {
                        lstFiltrados.Add(recibo);
                    }
                }
                if (x == 'f')
                {
                    if (recibo.Fecha.Contains(texto))
                    {
                        lstFiltrados.Add(recibo);
                    }
                }
            }
            ActualizarGridRecibos(lstFiltrados);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem.ToString() == "Cliente")
            {
                FiltrarDatos(textBox1.Text, 'c');
            }
            if (comboBox1.SelectedItem.ToString() == "Fecha")
            {
                FiltrarDatos(textBox1.Text, 'f');
            }
        }

        
        //_____________grupo imprimir_______________
        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox2.Text != "")
            {
                foreach (Recibo recibo in ObtenerListaAImprimir())
                {
                    AbrirFormImprimirRecibo(recibo);
                }
            }
        }
        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsNumber(e.KeyChar)) && (e.KeyChar != (char)Keys.Back))
            {
                e.Handled = true;
                return;
            }
        }
        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsNumber(e.KeyChar)) && (e.KeyChar != (char)Keys.Back))
            {
                e.Handled = true;
                return;
            }
        }
        private void textBox5_Leave(object sender, EventArgs e)
        {
            label10.Text = ObtenerListaABorrar().Count.ToString();
        }
        private void textBox4_Leave(object sender, EventArgs e)
        {
            label10.Text = ObtenerListaABorrar().Count.ToString();
        }

        private List<Recibo> ObtenerListaAImprimir()
        {
            List<Recibo> listaAImprimir = new List<Recibo>();

            if (comboBox2.SelectedItem.ToString() == "No impresas")
            {
                foreach (Recibo recibo in _lstRecibos)
                {
                    if (recibo.Impresa == 0)
                    {
                        listaAImprimir.Add(recibo);
                    }
                }
            }

            if (comboBox2.SelectedItem.ToString() == "Hoy")
            {
                foreach (Recibo recibo in _lstRecibos)
                {
                    if (recibo.Fecha == DateTime.Now.ToString("yyy-MM-dd"))
                    {
                        listaAImprimir.Add(recibo);
                    }
                }
            }

            if (textBox2.Text != "" && textBox3.Text != "")
            {
                if (comboBox2.SelectedItem.ToString() == "Establecer rango")
                {
                    try
                    {
                        long menor = long.Parse(textBox2.Text);
                        long mayor = long.Parse(textBox3.Text);
                        if (menor <= mayor)
                        {
                            for (long i = menor; i <= mayor; i++)
                            {
                                try
                                {
                                    listaAImprimir.Add(_lstRecibos.Find(x => x.Id == i));
                                }
                                catch {}
                            }
                        }
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Rango establecido incorrecto");
                    }
                }
            }
            return listaAImprimir;
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox2.SelectedItem.ToString() == "No impresas"
                || (comboBox2.SelectedItem.ToString() == "Hoy")
            )
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
        private void textBox3_Leave(object sender, EventArgs e)
        {
            label7.Text = ObtenerListaAImprimir().Count.ToString();
        }
        private void textBox2_Leave(object sender, EventArgs e)
        {
            label7.Text = ObtenerListaAImprimir().Count.ToString();
        }


        //BORRAR RECIBOS
        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox3.SelectedItem.ToString() == "Impresas"
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
                            for (long i = menor; i <= mayor; i++)
                            {
                                listaABorrar.Add(_lstRecibos.Find(x => x.Id == i));
                            }
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
                    if (recibo.Impresa == 1)
                    {
                        listaABorrar.Add(recibo);
                    }
                }
            }
            return listaABorrar;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (comboBox3.SelectedItem.ToString() == "Todas"
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
        private void button5_Click(object sender, EventArgs e)
        {
            comboBox3.SelectedItem = "(Seleccionar)";
            label11.Visible = false;
            button4.Visible = false;
            button5.Visible = false;
            button3.Visible = true;
        }
        private void button4_Click(object sender, EventArgs e)
        {
            foreach (Recibo recibo in ObtenerListaABorrar())
            {
                EliminarRecibo(recibo);
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

        // NUEVO RECIBO
        private void ClienteId_Leave(object sender, EventArgs e)
        {
            if (textBox6.Text != "")
            {
                try
                {
                    long id = long.Parse(textBox6.Text);
                    if (_lstClientes.Exists(x => x.Id == id))
                    {
                        label15.Text = _lstClientes.Find(x => x.Id == id).Direccion;
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
                catch (Exception)
                {
                    MessageBox.Show("No se encontró");
                }
            }

        }

        private void AgregarDetalle_Click(object sender, EventArgs e)
        {
            if (textBox8.Text != "" && AlgunDetSeleccionado())
            {
                double importe = double.Parse(textBox8.Text);
                DetalleRecibo nuevoDetalle = new DetalleRecibo
                {
                    ReciboId = 1
                };

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

                if ((label15.Text != "") && (label15.Text != "No encontrado"))
                {
                    button6.Enabled = true;
                }
            }
            else
            {
                MessageBox.Show("Complete correctamente los campos.");
            }
        }

        private void Limpiar_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
            textBox6.Text = "";
            label15.Text = "";
            button6.Enabled = false;
            label18.Text = "0";
            _lstDetallesAAgregar.Clear();
            ActualizarGridDetalles();
        }

        private void AnyadirRecibo_Click(object sender, EventArgs e)
        {
            CargarRecibos();
            Cliente cliente = _lstClientes.Find(x => x.Id == long.Parse(textBox6.Text));
            if (cliente == null) return;

            Recibo nuevoRecibo = new Recibo
            {
                ClienteId = cliente.Id,
                DireccionCliente = label15.Text,
                DetalleRecibos = _lstDetallesAAgregar,
                ImporteTotal = _subTo,
                Impresa = 0,
                Fecha = DateTime.Now.ToString("yyyy-MM-dd")
            };
            long reciboId = GuardarRecibo(nuevoRecibo);
            foreach (DetalleRecibo detalle in _lstDetallesAAgregar)
            {
                detalle.ReciboId = reciboId;
                AgregarDetalle(detalle);
            }
            
            LimpiarCampos();
            textBox6.Text = "";
            label15.Text = "";
            button6.Enabled = false;
            label18.Text = "0";
            _lstDetallesAAgregar.Clear();
            Actualizar();
        }
    }
}
