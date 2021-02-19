﻿using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;


namespace linway_app
{
    public partial class FormNotasEnvio : Form
    {
        const string direccion = @"Base de datos\NotasDeEnvio.bin";
        const string copiaDeSeguridad = "notas";
        int ultimaNota;
        int primeraNota = 0;
        List<NotaDeEnvio> notasEnvio = new List<NotaDeEnvio>();
        List<ProdVendido> listaPV = new List<ProdVendido>();
        readonly List<Producto> listaProductos = new List<Producto>();

        public FormNotasEnvio()
        {
            InitializeComponent();
        }

        private void FormNotas_Load(object sender, EventArgs e)
        {
            CargarNotas();
            dataGridView1.DataSource = notasEnvio.ToArray();
            dataGridView1.Columns[0].Width = 30;
            dataGridView1.Columns[1].Width = 50;
            dataGridView1.Columns[2].Width = 150;
            dataGridView1.Columns[4].Width = 40;
            dataGridView1.Columns[5].Width = 35;
            comboBox1.SelectedItem = "Todas";
            comboBox3.SelectedItem = "(Seleccionar)";
            dataGridView2.DataSource = listaPV.ToArray();
            dataGridView2.Columns[0].Width = 30;
            dataGridView2.Columns[2].Width = 40;
        }

        void CargarNotas()
        {
            if (File.Exists(direccion))
            {
                try
                {
                    Stream archivo = File.OpenRead(direccion);
                    BinaryFormatter traductor = new BinaryFormatter();
                    notasEnvio = (List<NotaDeEnvio>) traductor.Deserialize(archivo);
                    archivo.Close();
                    
                    foreach (NotaDeEnvio nActual in notasEnvio)
                    {
                        if (primeraNota == 0)
                        {
                            primeraNota = nActual.Codigo;
                        }
                        ultimaNota = nActual.Codigo;
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show("Error al leer las notas de envío: " + e.Message);
                }
            }
            else
            {
                MessageBox.Show("No se encontró el archivo Notas de Envío en la carpeta Base de datos...");
            }

            lCantNotas.Text = notasEnvio.Count.ToString() + " notas de envio.";
        }

        void GuardarNotas()
        {
            try
            {
                Stream archivo = File.Create(direccion);
                BinaryFormatter traductor = new BinaryFormatter();
                traductor.Serialize(archivo, notasEnvio);
                archivo.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show("Error al guardar las notas de envío:" + e.Message);
            }
        }

        private void bCopiaSeguridad_Click(object sender, EventArgs e)         // quitar diálogo, llevar a Exportar
        {
            CargarNotas();
            DialogResult dialogResult = MessageBox.Show("Esta acción reemplazará al actual Excel notas.xlsx y demorará 15 segundos. ¿Confirmar?", "Exportar Notas de Envío a Excel", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                bool success = new Exportar().ExportarAExcel(notasEnvio);
                if (success)
                {
                    bCopiaSeguridad.ForeColor = Color.Green;
                    bCopiaSeguridad.Enabled = false;
                    bCopiaSeguridad.Text = "Creacion exitosa";
                }
                else
                {
                    MessageBox.Show("Hubo un error al guardar los cambios.");
                }
            }
        }

        public void ImportarNotasDeEnvio_Click(object sender, EventArgs e)
        {
            notasEnvio.Clear();
            CargarNotas();
            DialogResult dialogResult = MessageBox.Show("Esta acción reemplazará definitivamente el listado actual de notas de envío por el contenido del Excel notas.xlsx en la carpeta Copias de seguridad. ¿Confirmar?", "Importar Notas de Envío desde Excel", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                notasEnvio = new Importar(copiaDeSeguridad).ImportarNotas();
                if (notasEnvio != null)
                {
                    GuardarNotas();
                    MessageBox.Show("Terminado");
                }
                else
                {
                    MessageBox.Show("Falló Notas; cancelado");
                }
                CargarNotas();
            }
        }

        public void RecibirProductos(List<Producto> productos)
        {
            listaProductos.AddRange(productos);
        }


        //_________________________FILTRAR DATOS_________________________________
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<NotaDeEnvio> lFiltrada = new List<NotaDeEnvio>();
            //todas - hoy - impresas- no impresas
            if (comboBox1.SelectedItem.ToString() == "Hoy") // cambio dps de este, 21/01/2021
            {
                label2.Text = "";
                textBox1.Text = "";
                textBox1.Visible = false;
                foreach (NotaDeEnvio nActual in notasEnvio)
                {
                    if (nActual.Fecha == DateTime.Today.ToString().Substring(0, 10))
                    {
                        lFiltrada.Add(nActual);
                    }
                }
                dataGridView1.DataSource = lFiltrada.ToArray();
            }

            if (comboBox1.SelectedItem.ToString() == "Todas")
            {
                label2.Text = "";
                textBox1.Text = "";
                textBox1.Visible = false;
                dataGridView1.DataSource = notasEnvio.ToArray();
            }

            if (comboBox1.SelectedItem.ToString() == "Impresas")
            {
                label2.Text = "";
                textBox1.Text = "";
                textBox1.Visible = false;
                foreach (NotaDeEnvio nActual in notasEnvio)
                {
                    if (nActual.Impresa)
                    {
                        lFiltrada.Add(nActual);
                    }
                }
                dataGridView1.DataSource = lFiltrada.ToArray();
            }

            if (comboBox1.SelectedItem.ToString() == "No impresas")
            {
                label2.Text = "";
                textBox1.Text = "";
                textBox1.Visible = false;
                foreach (NotaDeEnvio nActual in notasEnvio)
                {
                    if (!nActual.Impresa)
                    {
                        lFiltrada.Add(nActual);
                    }
                }
                dataGridView1.DataSource = lFiltrada.ToArray();
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
            List<NotaDeEnvio> ListaFiltrada = new List<NotaDeEnvio>();

            foreach (NotaDeEnvio nActual in notasEnvio)
            {
                if (x == 'c')
                {
                    if (nActual.Cliente.Contains(texto))
                    {
                        ListaFiltrada.Add(nActual);
                    }
                }

                if (x == 'f')
                {
                    if (nActual.Fecha.Contains(texto))
                    {
                        ListaFiltrada.Add(nActual);
                    }
                }

            }
            dataGridView1.DataSource = ListaFiltrada.ToArray();
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

        //_______________GRUPO IMPRIMIR ________________________________________
        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox2.Text != "")
            {
                foreach (NotaDeEnvio nActual in ObtenerListaAImprimir())
                {
                    try
                    {
                        FormImprimirNota vistaPrevia = new FormImprimirNota();
                        vistaPrevia.Rellenar_Datos(nActual);
                        vistaPrevia.Show();
                    }
                    catch (Exception g)
                    {
                        MessageBox.Show("Error al generar vista previa, rellenar los datos o mostrar la vista previa:" + g.Message);
                    }
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

        public List<NotaDeEnvio> ObtenerListaAImprimir()
        {
            List<NotaDeEnvio> listaAImprimir = new List<NotaDeEnvio>();

            try
            {
                if (comboBox2.SelectedItem.ToString() == "No impresas")
                {
                    foreach (NotaDeEnvio nActual in notasEnvio)
                    {
                        if (!nActual.Impresa)
                        {
                            listaAImprimir.Add(nActual);
                        }
                    }
                }

                if (comboBox2.SelectedItem.ToString() == "Hoy")
                {
                    foreach (NotaDeEnvio nActual in notasEnvio)
                    {
                        if (nActual.Fecha == DateTime.Today.ToString().Substring(0, 10))
                        {
                            listaAImprimir.Add(nActual);
                        }
                    }
                }

                if (textBox2.Text != "" && textBox3.Text != "")
                {
                    if (comboBox2.SelectedItem.ToString() == "Establecer rango")
                    {
                        if ((int.Parse(textBox2.Text) <= int.Parse(textBox3.Text)) && (int.Parse(textBox3.Text) <= ultimaNota) && (int.Parse(textBox2.Text) >= primeraNota))
                        {
                            int j = int.Parse(textBox3.Text);
                            for (int i = int.Parse(textBox2.Text); i <= j; i++)
                            {
                                listaAImprimir.Add(notasEnvio.Find(x => x.Codigo == i));
                            }
                        }
                        else
                        {
                            MessageBox.Show("Rango establecido incorrecto");
                        }
                    }
                }
                return listaAImprimir;
            }
            catch (Exception excep)
            {
                MessageBox.Show("Error en la lista de notas de envío: " + excep.Message);
                return null;
            }

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((comboBox2.SelectedItem.ToString() == "No impresas") || ((comboBox2.SelectedItem.ToString() == "Hoy")))
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

        //Actualizar y Cerrar
        private void button2_Click(object sender, EventArgs e)
        {
            CargarNotas();
            dataGridView1.DataSource = notasEnvio.ToArray();
        }

        private void FormNotas_FormClosing(object sender, FormClosingEventArgs e)
        {
            CargarNotas();
            GuardarNotas();
        }

        //_________________________GRUPO BORRAR___________________________________
        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((comboBox3.SelectedItem.ToString() == "Impresas") || ((comboBox3.SelectedItem.ToString() == "Todas") || (comboBox3.SelectedItem.ToString() == "(Seleccionar)")))
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

        public List<NotaDeEnvio> ObtenerListaABorrar()
        {
            List<NotaDeEnvio> listaABorrar = new List<NotaDeEnvio>();

            if (textBox5.Text != "" && textBox4.Text != "")
            {
                if (comboBox3.SelectedItem.ToString() == "Establecer rango")
                {
                    if ((int.Parse(textBox5.Text) <= int.Parse(textBox4.Text)) && (int.Parse(textBox4.Text) <= ultimaNota) && (int.Parse(textBox5.Text) >= primeraNota))
                    {
                        int j = int.Parse(textBox4.Text);
                        for (int i = int.Parse(textBox5.Text); i <= j; i++)
                        {
                            listaABorrar.Add(notasEnvio.Find(x => x.Codigo == i));
                        }
                    }

                    else
                    {
                        MessageBox.Show("Rango establecido incorrecto");
                    }
                }
            }

            if (comboBox3.SelectedItem.ToString() == "Todas")
            {
                listaABorrar.AddRange(notasEnvio);
            }

            if (comboBox3.SelectedItem.ToString() == "Impresas")
            {
                foreach (NotaDeEnvio nActual in notasEnvio)
                {
                    if (nActual.Impresa)
                    {
                        listaABorrar.Add(nActual);
                    }
                }
            }

            return listaABorrar;
        }

        private void textBox5_Leave(object sender, EventArgs e)
        {
            label10.Text = ObtenerListaABorrar().Count.ToString();
        }

        private void textBox4_Leave(object sender, EventArgs e)
        {
            label10.Text = ObtenerListaABorrar().Count.ToString();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if ((comboBox3.SelectedItem.ToString() == "Todas") || (comboBox3.SelectedItem.ToString() == "Establecer rango") || (comboBox3.SelectedItem.ToString() == "Impresas"))
            {
                MessageBox.Show("Confirme si desea borrar las notas de envio seleccionadas");
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
            CargarNotas();
            foreach (NotaDeEnvio nActual in ObtenerListaABorrar())
            {
                notasEnvio.Remove(nActual);
            }
            comboBox3.SelectedItem = "(Seleccionar)";
            label11.Visible = false;
            button4.Visible = false;
            button5.Visible = false;
            button3.Visible = true;
            GuardarNotas();
            dataGridView1.DataSource = notasEnvio.ToArray();
            lCantNotas.Text = notasEnvio.Count.ToString() + " notas de envio.";
        }

        //enviar a hoja de reparto
        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((label16.Text != "") && (label16.Text != "No encontrado"))
            {
                button6.Enabled = true;
            }
            else
            {
                button6.Enabled = false;
            }
            FormReparto fr = new FormReparto();
            comboBox5.DataSource = fr.DarRepartos(comboBox4.Text);
            comboBox5.DisplayMember = "Nombre";
            comboBox5.ValueMember = "Nombre";
            fr.Close();
        }

        private void textBox6_Leave(object sender, EventArgs e)
        {
            if (textBox6.Text != "")
            {
                if (notasEnvio.Exists(x => x.Codigo == int.Parse(textBox6.Text)))
                {
                    label16.Text = notasEnvio.Find(x => x.Codigo == int.Parse(textBox6.Text)).Cliente;
                    if (comboBox5.Text != "")
                    {
                        button6.Enabled = true;
                    }
                    else
                    {
                        button6.Enabled = false;
                    }
                }
                else
                {
                    label16.Text = "No encontrado";
                    button6.Enabled = false;
                }
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            comboBox5.Text = "";
            comboBox4.Text = "";
            textBox6.Text = "";
            button6.Enabled = false;
            label16.Text = "";
        }

        private void button6_Click(object sender, EventArgs e)
        {
            FormReparto fr = new FormReparto();
            fr.CargarAHojaDeReparto2(label16.Text, comboBox4.Text, comboBox5.Text, notasEnvio.Find(x => x.Codigo == int.Parse(textBox6.Text)).Productos);
            fr.Close();
            comboBox5.Text = "";
            comboBox4.Text = "";
            textBox6.Text = "";
            button6.Enabled = false;
            label16.Text = "";
        }

        //Modificar nota de envio
        private void textBox7_Leave(object sender, EventArgs e)
        {
            if (textBox7.Text != "")
            {
                if (notasEnvio.Exists(x => x.Codigo == int.Parse(textBox7.Text)))
                {
                    listaPV = notasEnvio.Find(x => x.Codigo == int.Parse(textBox7.Text)).Productos;
                    label18.Text = notasEnvio.Find(x => x.Codigo == int.Parse(textBox7.Text)).Cliente;
                    dataGridView2.DataSource = listaPV.ToArray();
                    float impTotal = 0;
                    foreach (ProdVendido pvActual in listaPV)
                    {
                        impTotal += pvActual.Subtotal;
                    }
                    label20.Text = impTotal.ToString();
                    button10.Enabled = true;
                }
                else
                {
                    label18.Text = "No encontrado";
                    listaPV.Clear();
                    dataGridView2.DataSource = listaPV.ToArray();
                    label20.Text = "0";
                    button10.Enabled = false;
                }
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            if (listaPV.Count != 0)
            {
                CargarNotas();
                notasEnvio.Find(x => x.Codigo == int.Parse(textBox7.Text)).Modificar(listaPV);
                GuardarNotas();
                textBox7.Text = "";
                button10.Enabled = false;
                label18.Text = "";
                label20.Text = "0";
                listaPV.Clear();
                CargarNotas();
                dataGridView2.DataSource = listaPV.ToArray();
                dataGridView1.DataSource = notasEnvio.ToArray();
            }
            else
            {
                MessageBox.Show("La nota de envío debe tener al menos un producto");
            }
        }

        //Quitar
        private void textBox8_TextChanged(object sender, EventArgs e)
        {
            if (textBox8.Text != "")
            {
                if (listaPV.Exists(x => x.Descripcion.Contains(textBox8.Text)))
                {
                    label22.Text = listaPV.Find(x => x.Descripcion.Contains(textBox8.Text)).Descripcion;
                    button8.Enabled = true;
                    button10.Enabled = true;
                }
                else
                {
                    label22.Text = "No encontrado";
                    button8.Enabled = false;
                    button10.Enabled = false;
                }
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if ((label18.Text != "") && (label18.Text != "No encontrado") && (textBox7.Text != ""))
            {
                listaPV.Remove(listaPV.Find(x => x.Descripcion == label22.Text));
                float impTotal = 0;
                foreach (ProdVendido pvActual in listaPV)
                {
                    impTotal += pvActual.Subtotal;
                }
                label20.Text = impTotal.ToString();
                textBox8.Text = "";
                label22.Text = "";
                button8.Enabled = false;
                dataGridView2.DataSource = listaPV.ToArray();
            }
        }

        //agregar
        private void textBox9_Leave(object sender, EventArgs e)
        {
            if (textBox9.Text != "")
            {
                if (listaProductos.Exists(x => x.Codigo == int.Parse(textBox9.Text)))
                {
                    label25.Text = listaProductos.Find(x => x.Codigo == int.Parse(textBox9.Text)).Nombre;
                    if (textBox10.Text != "")
                    {
                        button9.Enabled = true;
                    }
                    if (label25.Text.Contains("actura"))
                    {
                        textBox11.Visible = true;
                    }
                    else
                    {
                        textBox11.Visible = false;
                    }
                }
                else
                {
                    label25.Text = "No encontrado";
                    button9.Enabled = true;
                }
            }
        }

        private void textBox10_Leave(object sender, EventArgs e)
        {
            if ((textBox9.Text != "") && (label25.Text != "No encontrado"))
            {
                if (textBox10.Text != "")
                {
                    label26.Text = (listaProductos.Find(x => x.Nombre == label25.Text).Precio * int.Parse(textBox10.Text)).ToString();
                    button9.Enabled = true;
                }
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            ProdVendido nuevoPV = new ProdVendido();

            if (label25.Text.Contains("pendiente"))
            {
                nuevoPV.CargarPV(label25.Text, 1, float.Parse(label26.Text));
            }
            else if ((label25.Text.Contains("favor")) || (label25.Text.Contains("devoluci")) || (label25.Text.Contains("BONIF")))
            {
                nuevoPV.CargarPV(label25.Text, 1, float.Parse(label26.Text) * -1);
            }
            else if ((label25.Text.Contains("actura")))
            {
                nuevoPV.CargarPV(label25.Text + textBox11.Text, 1, float.Parse(label26.Text));
            }
            else
            {
                nuevoPV.CargarPV(label25.Text, int.Parse(textBox10.Text), float.Parse(label26.Text));
            }

            listaPV.Add(nuevoPV);
            dataGridView2.DataSource = listaPV.ToArray();
            float impTotal = 0;
            foreach (ProdVendido pvActual in listaPV)
            {
                impTotal += pvActual.Subtotal;
            }
            label20.Text = impTotal.ToString();
            label25.Text = "";
            label26.Text = "";
            textBox9.Text = "";
            textBox10.Text = "";
            textBox11.Text = "";
            textBox11.Visible = false;
            button9.Enabled = false;
        }

        //Excel exportar
        private void bExportar_Click(object sender, EventArgs e)
        {
            // Anulado
        }
    }
}
