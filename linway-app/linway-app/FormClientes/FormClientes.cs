using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
//using System.ComponentModel;
//using System.Data;
using System.Drawing;
using System.Linq;
//using System.Text;
using System.Windows.Forms;


namespace linway_app
{
    public partial class FormClientes : Form
    {
        const string direccionClientes = @"Base de datos\ClientesLinway.bin";
        const string copiaDeSeguridad = "clientes.xlsx";
        private int codigoParaCliente;
        List<Cliente> listaClientes = new List<Cliente>();

        public FormClientes()
        {
            InitializeComponent();
        }

        private void FormClientes_Load(object sender, EventArgs e)
        {
            CargarClientes();
        }

        public List<Cliente> CargarClientes()
        {
            if (File.Exists(direccionClientes))
            {
                listaClientes.Clear();
                try
                {
                    Stream archivo = File.OpenRead(direccionClientes);
                    BinaryFormatter traductor = new BinaryFormatter();
                    listaClientes = (List<Cliente>) traductor.Deserialize(archivo);
                    archivo.Close();
                    codigoParaCliente = listaClientes.ElementAt(listaClientes.Count - 1).Numero + 1;
                }
                catch (Exception e)
                {
                    MessageBox.Show("Falló lectura de datos de cliente:" + e.Message);
                }
            }
            else
            {
                MessageBox.Show("No se encontró archivo de clientes en la carpeta Base de datos...");
            }
            
            return listaClientes;
        }

        private void GuardarClientes()
        {
            try
            {
                Stream archivoClientes = File.Create(direccionClientes);
                BinaryFormatter traductor = new BinaryFormatter();
                traductor.Serialize(archivoClientes, listaClientes);
                archivoClientes.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show("Error al guardar archivo de clientes:" + e.Message);
            }
        }

        public void AgregarClientes()
        {
            gbModificar.Enabled = false;
            gbBorrar.Enabled = false;
        }

        public void ModificarClientes()
        {
            gbAgregar.Enabled = false;
            gbBorrar.Enabled = false;
        }

        public void BorrarClientes()
        {
            gbModificar.Enabled = false;
            gbAgregar.Enabled = false;
        }

        private void bCopiaSeguridad_Click(object sender, EventArgs e)
        {
            CargarClientes();
            DialogResult dialogResult = MessageBox.Show("Esta acción reemplazará al actual Excel clientes.xlsx y demorará 15 segundos. ¿Confirmar?", "Exportar Clientes a Excel", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                bool success = new Exportar().ExportarAExcel(listaClientes);
                if (success)
                {
                    //MessageBox.Show("Exportado Clientes con éxito a Copias de seguridad");
                    bCopiaSeguridad.ForeColor = Color.Green;
                    bCopiaSeguridad.Enabled = false;
                    bCopiaSeguridad.Text = "Creacion exitosa";
                }
                else
                {
                    MessageBox.Show("Hubo un error al guardar los cambios.");
                }
            }
            else if (dialogResult == DialogResult.No)
            {
                return;
            }
        }

        public void ImportarClientes_Click(object sender, EventArgs e)    // conectarlo a botón
        {
            CargarClientes();
            DialogResult dialogResult = MessageBox.Show("Esta acción reemplazará definitivamente el listado actual de clientes por el contenido del Excel clientes.xlsx en la carpeta Copias de seguridad. ¿Confirmar?", "Importar Clientes desde Excel", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                listaClientes.Clear();
                try
                {
                    listaClientes = new Importar(copiaDeSeguridad).ImportarClientes();
                }
                catch
                {
                    listaClientes.Clear();
                    CargarClientes();
                    throw;
                }
                codigoParaCliente = listaClientes.Count + 1;
                GuardarClientes();
                MessageBox.Show("Terminado");
            }
            else
            {
                return;
            }
        }





        //                                                                    agregar Cliente

        public bool TodoOKagregarC()
        {
            bool correcto = false;
            if ((textBox1.Text != "") && (textBox2.Text != "") && (textBox3.Text != "") && (textBox4.Text != "") && (textBox5.Text != "") && (textBox18.Text != ""))
            {
                if ((radioButton1.Checked) || (radioButton2.Checked))
                {
                    correcto = true;
                }
            }
            return correcto;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (TodoOKagregarC())
            {
                string nombre = textBox1.Text;
                string direccion = textBox2.Text + " - " + textBox18.Text;
                string cuit = textBox3.Text;
                int telefono = int.Parse(textBox5.Text);
                int cp = int.Parse(textBox4.Text);
                TipoR tipo = TipoR.Monotributo;
                if (radioButton2.Checked)
                {
                    tipo = TipoR.Inscripto;
                }
                //CargarClientes();
                Cliente nuevoCliente = new Cliente(codigoParaCliente, direccion, cp, telefono, nombre, cuit, tipo);
                listaClientes.Add(nuevoCliente);
                //GuardarClientes();
                button2.PerformClick();
                
                //bool response = new DBconnection().AgregarClienteEnDB(direccion, cp, telefono, nombre, cuit, tipo);
                //if (response) Close();
            }
            else
            {
                MessageBox.Show("Revise que se hayan llenado los campos correctamente");
            }
        }

        private void limpiar_Click(object sender, EventArgs e)
        {
            label23.Text = "";
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            textBox18.Text = "";
            textBox14.Text = "";
            textBox10.Text = "";
            textBox11.Text = "";
            textBox23.Text = "";
            textBox24.Text = "";
            textBox25.Text = "";
            radioButton1.Checked = false;
            radioButton2.Checked = false;
            radioButton3.Checked = false;
            radioButton4.Checked = false;
        }

        private void keyPress_SoloLetras(object sender, KeyPressEventArgs e)
        {
            if (Char.IsLetter(e.KeyChar))
            {
                e.Handled = false;
            }
            else if (Char.IsControl(e.KeyChar))
            {
                e.Handled = false;
            }
            else if (Char.IsSeparator(e.KeyChar))
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        private void keyPress_SoloNumeros(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsNumber(e.KeyChar)) && (e.KeyChar != (char)Keys.Back))
            {
                e.Handled = true;
                return;
            }
        }







        //                                     modificar cliente

        bool TodoOkModificarC()
        {
            bool correcto = false;
            if ((label23.Text != "No encontrado") && (textBox10.Text != "") && (textBox11.Text != "") && (textBox14.Text != "") && (textBox23.Text != "") && (textBox24.Text != "") && (textBox25.Text != ""))
            {
                correcto = true;
            }
            return correcto;
        }

        private void textBox14_Leave(object sender, EventArgs e)
        {
            bool encontrado = false;
            if (textBox14.Text != "")
            {
                foreach (Cliente cActual in listaClientes)
                {
                    if (cActual.Numero == int.Parse(textBox14.Text))
                    {
                        encontrado = true;
                        label23.Text = cActual.Direccion;
                        //
                        textBox23.Text = cActual.Direccion;
                        textBox24.Text = cActual.Telefono.ToString();
                        textBox25.Text = cActual.CodigoPostal.ToString();
                        //
                        textBox11.Text = cActual.Nombre;
                        textBox10.Text = cActual.CUIT;
                        if (cActual.Tipo == TipoR.Inscripto)
                        {
                            radioButton3.Checked = true;
                        }
                        else
                        {
                            radioButton4.Checked = true;
                        }
                    }
                }
            }
            if (!encontrado)
            {
                label23.Text = "No encontrado";
                textBox11.Text = "";
                textBox10.Text = "";
                textBox23.Text = "";
                textBox24.Text = "";
                textBox25.Text = "";
                radioButton3.Checked = false;
                radioButton4.Checked = false;
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (TodoOkModificarC())
            {
                //CargarClientes();
                foreach (Cliente cActual in listaClientes)
                {
                    if (cActual.Direccion.Equals(label23.Text))
                    {
                        cActual.Direccion = textBox23.Text;
                        cActual.Telefono = int.Parse(textBox24.Text);
                        cActual.CodigoPostal = int.Parse(textBox25.Text);
                        cActual.Nombre = textBox11.Text;
                        cActual.CUIT = textBox10.Text;
                        if (radioButton3.Checked)
                        {
                            cActual.Tipo = TipoR.Inscripto;
                        }
                        else
                        {
                            cActual.Tipo = TipoR.Monotributo;
                        }
                    }
                }
                //GuardarClientes();
                button8.PerformClick();
            }
            else
            {
                MessageBox.Show("Verifique que los campos sean correctos");
            }
        }







        //                                        Borrar clientes

        private void textBox22_Leave(object sender, EventArgs e)
        {
            if (textBox22.Text != "")
            {
                if (listaClientes.Exists(x => x.Numero == int.Parse(textBox22.Text)))
                {
                    label47.Text = listaClientes.Find(x => x.Numero == int.Parse(textBox22.Text)).Direccion;
                    button23.Enabled = true;
                }
                else
                {
                    label47.Text = "No encontrado";
                    button23.Enabled = false;
                }
            }
        }

        private void button23_Click(object sender, EventArgs e)
        {
            if (cbSeguroBorrar.Checked)
            {
                //CargarClientes();
                listaClientes.Remove(listaClientes.Find(x => x.Direccion == label47.Text));
                //GuardarClientes();
                cbSeguroBorrar.Checked = false;
                label47.Text = "";
                textBox22.Text = "";
            }
        }

        private void bSalir_Click(object sender, EventArgs e)
        {
            Close();
            GuardarClientes();
        }
    }
}
