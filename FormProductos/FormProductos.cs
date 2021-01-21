using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;


namespace WindowsFormsApplication1
{
    public partial class FormProductos : Form
    {
        public FormProductos()
        {
            InitializeComponent();
        }

        const string direccionProductos = "ProductosLinway.bin";
        const string copiaDeSeguridad = @"Copias de seguridad\ProductosLinway.bin";
        List<Producto> listaProductos = new List<Producto>();
        private int ultimoProducto;

        private void GuardarProductos()
        {
            Stream archivoProductos = File.Create(direccionProductos);
            BinaryFormatter traductor2 = new BinaryFormatter();
            traductor2.Serialize(archivoProductos, listaProductos);
            archivoProductos.Close();
        }

        public List<Producto> cargarProductos()
        {
            if (File.Exists(direccionProductos))
            {
                Stream archivoProductos = File.OpenRead(direccionProductos);
                BinaryFormatter traductor = new BinaryFormatter();
                listaProductos = (List<Producto>)traductor.Deserialize(archivoProductos);
                archivoProductos.Close();
                ultimoProducto = listaProductos.ElementAt(listaProductos.Count - 1).Codigo + 1;
            }
            return listaProductos;
        }

        public void agregarProductos()
        {
            gbModificar.Enabled = false;
            gbBorrar.Enabled = false;
        }
        public void modificarProductos()
        {
            gbAgregar.Enabled = false;
            gbBorrar.Enabled = false;
        }
        public void borrarProductos()
        {
            gbAgregar.Enabled = false;
            gbModificar.Enabled = false;
        }

        private void FormProductos_Load(object sender, EventArgs e)
        {
            cargarProductos();
        }

        //Agregar
        private void limpiar_Click(object sender, EventArgs e)
        {
            textBox21.Text = "";
            textBox6.Text = "";
            textBox7.Text = "";
            textBox8.Text = "";
            textBox9.Text = "";
            label18.Text = "";
            label19.Text = "";
            label46.Text = "";
            cbSeguroBorrar.Checked = false;
            radioButton1.Checked = false;
            radioButton2.Checked = false;
            radioButton3.Checked = false;
            radioButton4.Checked = false;
        }

        private bool algunTipoSeleccionado()
        {
            return (radioButton1.Checked | radioButton2.Checked | radioButton3.Checked | radioButton4.Checked);
        }
        private bool todoOKagregarP()
        {
            return ((textBox6.Text != "") && (textBox7.Text != ""));
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (todoOKagregarP())
            {
                string nombre = textBox6.Text;
                float precio = float.Parse(textBox7.Text);
                cargarProductos();
                Producto nuevoProducto = new Producto(ultimoProducto, nombre, precio);
                listaProductos.Add(nuevoProducto);
                GuardarProductos();
                button4.PerformClick();
            }
            else
            {
                MessageBox.Show("Verifique los campos.");
            }
        }

        private void SoloImporte_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 8)
            {
                e.Handled = false;
                return;
            }
            bool IsDec = false;
            int nroDec = 0;

            for (int i = 0; i < textBox7.Text.Length; i++)
            {
                if (textBox7.Text[i] == ',')
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
                e.Handled = (IsDec) ? true : false;
            else
                e.Handled = true;
        }

        //Modificar

        private void SoloNumeros_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsNumber(e.KeyChar)) && (e.KeyChar != (char)Keys.Back))
            {
                e.Handled = true;
                return;
            }
        }

        private void textBox8_Leave(object sender, EventArgs e)
        {
            bool encontrado = false;
            if (textBox8.Text != "")
            {
                foreach (Producto pActual in listaProductos)
                {
                    if (pActual.Codigo == int.Parse(textBox8.Text))
                    {
                        encontrado = true;
                        label18.Text = pActual.Nombre;
                        label19.Text = "" + pActual.Precio;
                        textBox9.Text = pActual.Precio.ToString();
                    }
                }
            }
            if (!encontrado)
            {
                label18.Text = "No encontrado";
                label19.Text = "No encontrado";
                textBox9.Text = "";
            }
        }

        bool todoOKmodificarP()
        {
            if ((label18.Text != "No encontrado") && (textBox8.Text != "") && (textBox9.Text != ""))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (todoOKmodificarP())
            {
                cargarProductos();
                foreach (Producto pActual in listaProductos)
                {
                    if (pActual.Nombre.Equals(label18.Text))
                    {
                        pActual.Precio = float.Parse(textBox9.Text);
                        GuardarProductos();
                        button6.PerformClick();
                    }
                }
            }
            else
            {
                MessageBox.Show("Verifique que se hayan llenado los campos correctamente");
            }
        }

        //Borrar
        private void textBox21_Leave(object sender, EventArgs e)
        {
            if (textBox21.Text != "")
            {
                if (listaProductos.Exists(x => x.Codigo == int.Parse(textBox21.Text)))
                {
                    label46.Text = listaProductos.Find(x => x.Codigo == int.Parse(textBox21.Text)).Nombre;
                    button22.Enabled = true;
                }
                else
                {
                    label46.Text = "No encontrado";
                    button22.Enabled = false;
                }
            }
        }

        private void button22_Click(object sender, EventArgs e)
        {
            if (cbSeguroBorrar.Checked)
            {
                cargarProductos();
                listaProductos.Remove(listaProductos.Find(x => x.Nombre.Equals(label46.Text)));
                button22.Enabled = false;
                GuardarProductos();
                textBox21.Text = "";
                cbSeguroBorrar.Checked = false;
            }
            else
            {
                MessageBox.Show("Tilde si esta seguro para borrar el producto");
            }
        }

        //Copia Seguridad
        private void crearCopiaSeguridad_Click(object sender, EventArgs e)
        {
            Stream archivoProductos = File.Create(copiaDeSeguridad);
            BinaryFormatter traductor2 = new BinaryFormatter();
            traductor2.Serialize(archivoProductos, listaProductos);
            archivoProductos.Close();
            bCopiaSeguridad.Enabled = false;
            bCopiaSeguridad.Text = "Creacion exitosa";
        }

        private void bSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }



        // Agregar - 2
        ProductoL productoNuevo;

        private void seleccionarTipo_CheckedChanged(object sender, EventArgs e)
        {
            
            RadioButton elegido = (RadioButton)sender;
            switch (elegido.Text)
            {
                case "Liquido":
                    productoNuevo = new Liquido();
                    comboBox1.Visible = true;
                    comboBox1.DataSource = Enum.GetValues(typeof(TipoLiquido));
                    break;
                case "Polvo":
                    productoNuevo = new Polvo();
                    comboBox1.Visible = true;
                    comboBox1.DataSource = Enum.GetValues(typeof(TipoPolvo));
                    break;
                case "Unidades":
                    productoNuevo = new Unidades();
                    comboBox1.Visible = false;
                    comboBox1.DataSource = null;
                    break;
                case "Otro":
                    productoNuevo = new Otros();
                    comboBox1.Visible = false;
                    comboBox1.DataSource = null;
                    break;
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            productoNuevo.darTipoDeProducto((Enum)comboBox1.SelectedItem);
        }

        private void agregarPL_Click(object sender, EventArgs e)
        {
            if (todoOKagregarP())
            {
                cargarProductos();
                productoNuevo.Nombre = textBox6.Text;
                productoNuevo.Precio = float.Parse(textBox7.Text);
                //listaProductos.Add(productoNuevo);
                GuardarProductos();
                button4.PerformClick();
            }

        }
    }
}