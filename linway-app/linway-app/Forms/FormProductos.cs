using linway_app.Models;
using System;
using System.Windows.Forms;
using static linway_app.Services.Delegates.DProductos;

namespace linway_app.Forms
{
    public partial class FormProductos : Form
    {
        public FormProductos()
        {
            InitializeComponent();
        }
        private void Limpiar_Click(object sender, EventArgs e)
        {
            textBox21.Text = "";
            textBox6.Text = "";
            textBox7.Text = "";
            textBox8.Text = "";
            textBox9.Text = "";
            textBox2.Text = "";
            label19.Text = "";
            label46.Text = "";
            cbSeguroBorrar.Checked = false;
            radioButton1.Checked = false;
            radioButton2.Checked = false;
            radioButton3.Checked = false;
            radioButton4.Checked = false;
        }
        // private bool AlgunTipoSeleccionado()
        // {
        //     return (radioButton1.Checked | radioButton2.Checked | radioButton3.Checked | radioButton4.Checked);
        // }
        private bool TodoOKagregarP()
        {
            return (textBox6.Text != "" && textBox7.Text != "");
        }
        private void AgregarProducto_Click(object sender, EventArgs e)
        {
            if (TodoOKagregarP())
            {
                try { double.Parse(textBox7.Text); } catch { return; };
                Producto nuevoProducto = new Producto {
                    Nombre = textBox6.Text,
                    Precio = double.Parse(textBox7.Text),
                    Estado = "Activo"
                };
                addProducto(nuevoProducto);
                limpiarBtn.PerformClick();
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
        private void textBox8_TextChanged(object sender, EventArgs e)   // buscar por id
        {
            if (textBox8.Text != "")
            {
                try { long.Parse(textBox8.Text); } catch { return; };
                Producto producto = getProducto(long.Parse(textBox8.Text));
                if (producto != null)
                {
                    label19.Text = producto.Nombre;
                    textBox9.Text = producto.Precio.ToString();        // campo de edición
                }
                else
                {
                    label19.Text = "No encontrado";
                    textBox9.Text = "";
                }
            }
            else
            {
                label19.Text = "";
                textBox9.Text = "";
            }
        }
        private void textBox2_TextChanged(object sender, EventArgs e)  // buscar por nombre
        {
            if (textBox2.Text != "")
            {
                Producto producto = getProductoPorNombre(textBox2.Text);
                if (producto != null)
                {
                    label19.Text = producto.Nombre;
                    textBox9.Text = producto.Precio.ToString();        // campo de edición
                }
                else
                {
                    label19.Text = "No encontrado";
                    textBox9.Text = "";
                }
            }
            else
            {
                label19.Text = "";
                textBox9.Text = "";
            }
        }
        bool TodoOKmodificarP()
        {
            if (label19.Text == "No encontrado" || label19.Text == "" || textBox9.Text == "")
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        private void EditarProducto_Click(object sender, EventArgs e)
        {
            if (TodoOKmodificarP())
            {
                Producto producto = getProductoPorNombreExacto(label19.Text);
                if (producto == null) return;
                try { double.Parse(textBox9.Text); } catch { return; };
                producto.Precio = double.Parse(textBox9.Text);
                editProducto(producto);
                button6.PerformClick();
            }
            else
            {
                MessageBox.Show("Verifique que se hayan llenado los campos correctamente");
            }
        }



        //Borrar
        private void textBox21_Leave(object sender, EventArgs e)
        {}
        private void textBox21_TextChanged(object sender, EventArgs e)  // por id
        {
            if (textBox21.Text != "")
            {
                try { long.Parse(textBox21.Text); } catch { return; };
                Producto producto = getProducto(long.Parse(textBox21.Text));
                if (producto != null)
                {
                    label46.Text = producto.Nombre;
                    button22.Enabled = true;
                }
                else
                {
                    label46.Text = "No encontrado";
                    button22.Enabled = false;
                }
            }
            else
            {
                label46.Text = "";
                button22.Enabled = false;
            }
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                Producto producto = getProductoPorNombre(textBox1.Text);
                if (producto != null)
                {
                    label46.Text = producto.Nombre;
                    button22.Enabled = true;
                }
                else
                {
                    label46.Text = "No encontrado";
                    button22.Enabled = false;
                }
            }
            else
            {
                label46.Text = "";
                button22.Enabled = false;
            }
        }
        private void Eliminar_Click(object sender, EventArgs e)
        {
            if (cbSeguroBorrar.Checked)
            {
                Producto producto = getProductoPorNombreExacto(label46.Text);
                button22.Enabled = false;
                deleteProducto(producto);
                textBox21.Text = "";
                textBox1.Text = "";
                label46.Text = "";
                cbSeguroBorrar.Checked = false;
            }
            else
            {
                MessageBox.Show("Tilde si esta seguro para borrar el producto");
            }
        }

        private void SalirBtn_Click(object sender, EventArgs e)
        {
            Close();
        }







        // proyecto tipos de producto:

        private void SeleccionarTipo_CheckedChanged(object sender, EventArgs e)
        {
            comboBox1.Visible = true;
            RadioButton elegido = (RadioButton)sender;

            switch (elegido.Text)
            {
                case "Liquido":
                    //productoNuevo = new Liquido();
                    //comboBox1.DataSource = Enum.GetValues(typeof(TipoLiquido));
                    break;
                case "Polvo":
                    //productoNuevo = new Polvo();
                    comboBox1.Visible = true;
                    //comboBox1.DataSource = Enum.GetValues(typeof(TipoPolvo));
                    break;
                case "Unidades":
                    //productoNuevo = new Unidades();
                    comboBox1.Visible = false;
                    comboBox1.DataSource = null;
                    break;
                case "Otro":
                    //productoNuevo = new Otros();
                    comboBox1.Visible = false;
                    comboBox1.DataSource = null;
                    break;
            }
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //productoNuevo.DarTipoDeProducto((Enum)comboBox1.SelectedItem);
        }
        private void agregarPL_Click(object sender, EventArgs e)
        {
            if (TodoOKagregarP())
            {
                //GetProductos();
                //productoNuevo.Nombre = textBox6.Text;
                //productoNuevo.Precio = float.Parse(textBox7.Text);
                ////listaProductos.Add(productoNuevo);
                //GuardarProducto();
                //button4.PerformClick();
            }

        }
    }
}