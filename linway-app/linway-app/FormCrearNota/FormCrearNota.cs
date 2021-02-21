using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;


namespace linway_app
{
    public partial class FormCrearNota : Form
    {
        const string direccionNotas = @"Base de datos\NotasDeEnvio.bin";
        int codigoParaNotaEnvio;
        List<NotaDeEnvio> listaNotas = new List<NotaDeEnvio>();
        readonly List<ProdVendido> listaPV = new List<ProdVendido>();
        readonly List<Producto> listaProductos = new List<Producto>();
        readonly List<Cliente> listaClientes = new List<Cliente>();
        
        public FormCrearNota()
        {
            InitializeComponent();
        }

        private void CargarNotas()
        {
            if (File.Exists(direccionNotas))
            {
                try
                {
                    Stream archivo = File.OpenRead(direccionNotas);
                    BinaryFormatter traductor = new BinaryFormatter();
                    listaNotas = (List<NotaDeEnvio>) traductor.Deserialize(archivo);
                    archivo.Close();
                }
                catch (Exception e)
                {
                    MessageBox.Show("Error al leer las notas de envío:" + e.Message);
                }
            }
            else
            {
                MessageBox.Show("No se encontró el archivo Notas de Envío en la carpeta Base de datos...");
            }
            if (listaNotas.Count>1) codigoParaNotaEnvio = listaNotas.ElementAt(listaNotas.Count - 1).Codigo + 1;
        }

        private void GuardarNotas()
        {
            try
            {
                Stream archivo = File.Create(direccionNotas);
                BinaryFormatter traductor = new BinaryFormatter();
                traductor.Serialize(archivo, listaNotas);
                archivo.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show("Error al guardar notas de envío:" + e.Message);
            }
        }

        private void FormCrearNota_Load(object sender, EventArgs e)
        {
            listaPV.Clear();
            dataGridView4.DataSource = listaPV.ToArray();
            dataGridView4.Columns[0].Width = 35;
            dataGridView4.Columns[2].Width = 60;
        }

        public void cargarProductosYClientes(List<Producto> productos, List<Cliente> clientes)
        {
            listaProductos.AddRange(productos);
            listaClientes.AddRange(clientes);
        }

        private void button16_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void soloNumero_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsNumber(e.KeyChar)) && (e.KeyChar != (char)Keys.Back))
            {
                e.Handled = true;
                return;
            }
        }

        private void textBox15_Leave(object sender, EventArgs e)
        {
            bool encontrado = false;
            if (textBox15.Text != "")
            {
                foreach (Cliente cActual in listaClientes)
                {
                    if (cActual.Numero == int.Parse(textBox15.Text))
                    {
                        encontrado = true;
                        label36.Text = cActual.Direccion;
                    }
                }
            }
            if (!encontrado)
            {
                label36.Text = "No encontrado";
            }

        }

        private void textBox16_Leave(object sender, EventArgs e)
        {
            bool encontrado = false;
            if (textBox16.Text != "")
            {
                foreach (Producto pActual in listaProductos)
                {
                    if (pActual.Codigo == int.Parse(textBox16.Text))
                    {
                        encontrado = true;
                        label38.Text = pActual.Nombre;
                    }
                }
                if (label38.Text.Contains("actura"))
                {
                    textBox20.Visible = true;
                }
                else
                {
                    textBox20.Visible = false;
                }
            }
            if (!encontrado)
            {
                label38.Text = "No encontrado";
            }
        }

        private void textBox17_Leave(object sender, EventArgs e)
        {
            if ((label38.Text != "No encontrado") && (textBox16.Text != "") && (textBox17.Text != ""))
            {
                foreach (Producto pActual in listaProductos)
                {
                    if (pActual.Nombre.Equals(label38.Text))
                    {
                        label40.Text = (pActual.Precio * int.Parse(textBox17.Text)).ToString();
                    }
                }
            }
        }

        private void button14_Click(object sender, EventArgs e)
        {
            listaPV.Clear();
            dataGridView4.DataSource = listaPV.ToArray();
            label42.Text = "";
        }

        private void button13_Click(object sender, EventArgs e)
        {
            if ((label38.Text != "No encontrado") && (textBox16.Text != "") && (textBox17.Text != "") && (label40.Text != ""))
            {
                ProdVendido nuevoPV = new ProdVendido();
                if (label38.Text.Contains("pendiente"))
                {
                    nuevoPV.CargarPV(label38.Text, 1, float.Parse(label40.Text));
                }
                else if ((label38.Text.Contains("favor")) || (label38.Text.Contains("devoluci")) || (label38.Text.Contains("BONIF")))
                {
                    nuevoPV.CargarPV(label38.Text, 1, float.Parse(label40.Text) * -1);
                }
                else if ((label38.Text.Contains("actura")))
                {
                    nuevoPV.CargarPV(label38.Text + textBox20.Text, 1, float.Parse(label40.Text));
                }
                else
                {
                    nuevoPV.CargarPV(label38.Text, int.Parse(textBox17.Text), float.Parse(label40.Text));
                }
                listaPV.Add(nuevoPV);
                float impTotal = 0;
                foreach (ProdVendido vActual in listaPV)
                {
                    impTotal += vActual.Subtotal;
                }
                textBox20.Text = "";
                textBox20.Visible = false;
                label42.Text = impTotal.ToString();
                dataGridView4.DataSource = listaPV.ToArray();
                textBox16.Text = "";
                textBox17.Text = "";
                label38.Text = "";
                label40.Text = "";
            }
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox3.Checked)
            {
                label33.Visible = true;
                label34.Visible = true;
                comboBox4.Visible = true;
                comboBox3.Visible = true;
            }
            else
            {
                label33.Visible = false;
                label34.Visible = false;
                comboBox4.Visible = false;
                comboBox3.Visible = false;
            }
        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            FormReparto fr = new FormReparto();
            comboBox3.DataSource = fr.DarRepartos(comboBox4.Text);
            comboBox3.DisplayMember = "Nombre";
            comboBox3.ValueMember = "Nombre";
            fr.Close();
        }

        //private bool EsProducto(string nombre)
        //{
        //    bool es = true;
        //    if ((nombre.Contains("pendiente")) || (nombre.Contains("favor")) || (nombre.Contains("actura")) || (nombre.Contains("evoluc")) || (nombre.Contains("cobrar") || (nombre.Contains("BONIFI"))))
        //    {
        //        es = false;
        //    }
        //    return es;
        //}

        private void button15_Click(object sender, EventArgs e)
        {
            if ((label36.Text != "No encontrado") && (textBox15.Text != ""))
            {
                CargarNotas();
                NotaDeEnvio nuevaNota = new NotaDeEnvio(codigoParaNotaEnvio, label36.Text, listaPV, false);
                if (label36.Text.Contains("–")) label36.Text = label36.Text.Replace("–", "-");
                // MessageBox.Show(codigoParaNotaEnvio + label36.Text + listaPV[0].Descripcion);
                //Agregar a ventas

                if (checkBox4.Checked)
                {
                    //FormVentas formVentas = new FormVentas();
                    //formVentas.RecibirProductosVendidos(listaPV, label36.Text);
                    FormVentas newFormVentas = new FormVentas();
                    newFormVentas.CargarVentas();
                    newFormVentas.CargarRegistros();
                    newFormVentas.RecibirProductosVendidos(listaPV, label36.Text);
                }

                if (checkBox1.Checked)
                {
                    FormImprimirNota formimprimir = new FormImprimirNota();
                    formimprimir.Rellenar_Datos(nuevaNota);
                    formimprimir.Show();
                }

                if (checkBox3.Checked)
                {
                    FormReparto fr = new FormReparto();
                    //MessageBox.Show(label36.Text + " " + comboBox4.Text + " " + comboBox3.Text + " " + listaPV);
                    fr.CargarAHojaDeReparto2(label36.Text, comboBox4.Text, comboBox3.Text, listaPV);
                    fr.Close();
                }

                listaNotas.Add(nuevaNota);
                GuardarNotas();
                Close();
            }
            else
            {
                MessageBox.Show("Verifique los campos");
            }
        }
    }
}
