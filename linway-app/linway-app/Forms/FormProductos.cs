using linway_app.Models;
using linway_app.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace linway_app.Forms
{
    public partial class FormProductos : Form
    {
        private readonly IServicioProducto _servProducto;
        public FormProductos(IServicioProducto servProducto)
        {
            InitializeComponent();
            _servProducto = servProducto;
        }

        private void FormProductos_Load(object sender, EventArgs e) {}

        public List<Producto> GetProductos()
        {
            List<Producto> lstProductos = _servProducto.GetAll();
            Console.WriteLine(lstProductos);
            return lstProductos;
        }
        private void GuardarProducto(Producto nuevoProducto)
        {
            bool response = _servProducto.Add(nuevoProducto);
            if (!response) MessageBox.Show("Algo falló al guardar en la base de datos");
        }
        private void EditarProducto(Producto prodEditar)
        {
            bool response = _servProducto.Edit(prodEditar);
            if (!response) MessageBox.Show("Algo falló al editar en la base de datos");
        }
        private void EliminarProducto(Producto producto)
        {
            bool response = _servProducto.Delete(producto);
            if (!response) MessageBox.Show("Algo falló eliminando el producto de la base de datos");
        }


        private void CrearCopiaSeguridad_Click(object sender, EventArgs e)
        {
            GetProductos();
            DialogResult dialogResult = MessageBox.Show("Esta acción reemplazará al actual Excel productos.xlsx y demorará 15 segundos. ¿Confirmar?", "Exportar Productos a Excel", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                //bool success = new Exportar().ExportarAExcel(listaProductos);
                //if (success)
                //{
                //    bCopiaSeguridad.ForeColor = Color.Green;
                //    bCopiaSeguridad.Enabled = false;
                //    bCopiaSeguridad.Text = "Creacion exitosa";
                //}
                //else
                //{
                //    MessageBox.Show("Hubo un error al guardar los cambios.");
                //}
            }
            else if (dialogResult == DialogResult.No)
            {
                return;
            }
        }

        private void ImportarBtn(object sender, EventArgs e)
        {
            GetProductos();
            DialogResult dialogResult = MessageBox.Show("Esta acción reemplazará definitivamente el listado actual de productos por el contenido del Excel productos.xlsx en la carpeta Copias de seguridad. ¿Confirmar?", "Importar Productos desde Excel", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                //listaProductos = new Importar("productos").ImportarProductos();
                //if (listaProductos != null)
                //{
                //    GuardarProductos();
                //    MessageBox.Show("Terminado");
                //}
                //else
                //{
                //    MessageBox.Show("Falló Productos; cancelado");
                //}
                //CargarProductos();
            }
        }

        private void Limpiar_Click(object sender, EventArgs e)
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

        // private bool AlgunTipoSeleccionado()
        // {
        //     return (radioButton1.Checked | radioButton2.Checked | radioButton3.Checked | radioButton4.Checked);
        // }
        
        private bool TodoOKagregarP()
        {
            return ((textBox6.Text != "") && (textBox7.Text != ""));
        }

        private void AgregarProducto_Click(object sender, EventArgs e)
        {
            if (TodoOKagregarP())
            {
                Producto nuevoProducto = new Producto {
                    Nombre = textBox6.Text,
                    Precio = float.Parse(textBox7.Text)
                };
                GuardarProducto(nuevoProducto);
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

        private void textBox8_Leave(object sender, EventArgs e)
        {
            List<Producto> lstProductos = GetProductos();
            bool encontrado = false;
            if (textBox8.Text != "")
            {
                foreach (Producto producto in lstProductos)
                {
                    if (producto.Id == int.Parse(textBox8.Text))
                    {
                        encontrado = true;
                        label18.Text = producto.Nombre;
                        label19.Text = "" + producto.Precio;
                        textBox9.Text = producto.Precio.ToString();        // campo de edición
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

        bool TodoOKmodificarP()
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

        private void EditarProducto_Click(object sender, EventArgs e)
        {
            if (TodoOKmodificarP())
            {
                List<Producto> lstProductos = GetProductos();
                foreach (Producto prodEditar in lstProductos)
                {
                    if (prodEditar.Nombre.Equals(label18.Text))
                    {
                        prodEditar.Precio = float.Parse(textBox9.Text);
                        EditarProducto(prodEditar);
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
                List<Producto> lstProductos = GetProductos();
                if (lstProductos.Exists(x => x.Id == int.Parse(textBox21.Text)))
                {
                    label46.Text = lstProductos.Find(x => x.Id == int.Parse(textBox21.Text)).Nombre;
                    button22.Enabled = true;
                }
                else
                {
                    label46.Text = "No encontrado";
                    button22.Enabled = false;
                }
            }
        }

        private void Eliminar_Click(object sender, EventArgs e)
        {
            if (cbSeguroBorrar.Checked)
            {
                List<Producto> lstProductos = GetProductos();
                Producto producto = lstProductos.Find(x => x.Nombre.Equals(label46.Text));
                button22.Enabled = false;
                EliminarProducto(producto);
                textBox21.Text = "";
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




        private void SeleccionarTipo_CheckedChanged(object sender, EventArgs e)
        {
            //comboBox1.Visible = true;
            //RadioButton elegido = (RadioButton) sender;

            //switch (elegido.Text)
            //{
            //    case "Liquido":
            //        productoNuevo = new Liquido();
            //        comboBox1.DataSource = Enum.GetValues(typeof(TipoLiquido));
            //        break;
            //    case "Polvo":
            //        productoNuevo = new Polvo();
            //        comboBox1.Visible = true;
            //        comboBox1.DataSource = Enum.GetValues(typeof(TipoPolvo));
            //        break;
            //    case "Unidades":
            //        productoNuevo = new Unidades();
            //        comboBox1.Visible = false;
            //        comboBox1.DataSource = null;
            //        break;
            //    case "Otro":
            //        productoNuevo = new Otros();
            //        comboBox1.Visible = false;
            //        comboBox1.DataSource = null;
            //        break;
            //}
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