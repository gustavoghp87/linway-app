using Models;
using Models.Enums;
using System;
using System.Windows.Forms;
using static linway_app.Services.Delegates.DProducto;

namespace linway_app.Forms
{
    public partial class FormProductos : Form
    {
        private string _tipo;
        private string _subTipo;
        private string _tipoMod;
        private string _subTipoMod;
        private bool _liberado;
        private bool _liberado2;
        public FormProductos()
        {
            InitializeComponent();
            ActiveControl = textBox2;
            _tipo = TipoProducto.Líquido.ToString();
            _subTipo = "";
            _tipoMod = "";
            _subTipoMod = "";
            _liberado = false;
            _liberado2 = false;
        }
        private void Limpiar_Click(object sender, EventArgs ev)
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
            comboBox1.Visible = false;
            comboBox2.Visible = false;
            comboBox3.Visible = false;
        }

        // AGREGAR PRODUCTO
        private void SeleccionarTipo_CheckedChanged(object sender, EventArgs ev)
        {
            comboBox1.Visible = true;
            var elegido = (RadioButton)sender;
            switch (elegido.Text)
            {
                case "Líquido":
                    _tipo = TipoProducto.Líquido.ToString();
                    comboBox1.DataSource = Enum.GetValues(typeof(TipoLiquido));
                    break;
                case "Polvo":
                    _tipo = TipoProducto.Polvo.ToString();
                    comboBox1.Visible = true;
                    comboBox1.DataSource = Enum.GetValues(typeof(TipoPolvo));
                    break;
                case "Unidad":
                    _tipo = TipoProducto.Unidad.ToString();
                    comboBox1.Visible = false;
                    comboBox1.DataSource = null;
                    break;
                case "Saldo":
                    _tipo = TipoProducto.Saldo.ToString();
                    comboBox1.Visible = true;
                    comboBox1.DataSource = Enum.GetValues(typeof(TipoSaldo));
                    break;
            }
        }
        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs ev)
        {
            if (comboBox1.SelectedItem != null) _subTipo = comboBox1.SelectedItem.ToString();
            else _subTipo = "";
        }
        private bool TodoOKagregarP()
        {
            return textBox6.Text != "" && textBox7.Text != ""
                && (radioButton1.Checked || radioButton2.Checked || radioButton3.Checked || radioButton4.Checked);
        }
        private void AgregarProducto_Click(object sender, EventArgs ev)
        {
            if (!TodoOKagregarP())
            {
                MessageBox.Show("Verifique los campos.");
                return;
            }
            try { decimal.Parse(textBox7.Text); } catch { return; };
            var nuevoProducto = new Producto
            {
                Nombre = textBox6.Text,
                Precio = decimal.Parse(textBox7.Text),
                Tipo = _tipo.ToString()
            };
            if (_subTipo != "") nuevoProducto.SubTipo = _subTipo;
            addProducto(nuevoProducto);
            limpiarBtn.PerformClick();
        }
        private void SoloImporte_KeyPress(object sender, KeyPressEventArgs ev)
        {
            if (ev.KeyChar == 8)
            {
                ev.Handled = false;
                return;
            }
            bool IsDec = false;
            int nroDec = 0;
            for (int i = 0; i < textBox7.Text.Length; i++)
            {
                if (textBox7.Text[i] == ',') IsDec = true;
                if (IsDec && nroDec++ >= 2)
                {
                    ev.Handled = true;
                    return;
                }
            }
            if (ev.KeyChar >= 48 && ev.KeyChar <= 57) ev.Handled = false;
            else if (ev.KeyChar == 44) ev.Handled = IsDec;
            else ev.Handled = true;
        }

        // MODIFICAR PRODUCTO
        private void SoloNumeros_KeyPress(object sender, KeyPressEventArgs ev)
        {
            if (!char.IsNumber(ev.KeyChar) && ev.KeyChar != (char)Keys.Back) ev.Handled = true;
        }
        private void CargarDatosAModificar(Producto producto)
        {
            if (producto != null)
            {
                _liberado = false;
                _liberado2 = false;
                label19.Text = producto.Nombre;
                textBox9.Text = producto.Precio.ToString();
                _tipoMod = producto.Tipo;
                if (producto.SubTipo != null)
                {
                    _subTipoMod = producto.SubTipo;
                    comboBox2.Visible = true;
                }
                else _subTipoMod = "";
                comboBox3.Visible = true;
                comboBox3.DataSource = Enum.GetValues(typeof(TipoProducto));
            }
            else
            {
                label19.Text = "No encontrado";
                textBox9.Text = "";
                _subTipoMod = "";
                comboBox3.Visible = false;
                comboBox2.Visible = false;
            }
        }
        private void ComboBox3_SelectedIndexChanged(object sender, EventArgs ev)
        {
            var tipoElegido = (ComboBox)sender;
            if (_tipoMod == null) return;
            if (_tipoMod != "" && !_liberado)
            {
                tipoElegido.Text = _tipoMod.ToString();
                _tipoMod = "";
            }
            switch (tipoElegido.Text)
            {
                case "Líquido":
                    _tipoMod = TipoProducto.Líquido.ToString();
                    comboBox2.Visible = true;
                    comboBox2.DataSource = Enum.GetValues(typeof(TipoLiquido));
                    break;
                case "Polvo":
                    _tipoMod = TipoProducto.Polvo.ToString();
                    comboBox2.Visible = true;
                    comboBox2.DataSource = Enum.GetValues(typeof(TipoPolvo));
                    break;
                case "Unidad":
                    _tipoMod = TipoProducto.Unidad.ToString();
                    comboBox2.Visible = false;
                    comboBox2.DataSource = null;
                    break;
                case "Saldo":
                    _tipoMod = TipoProducto.Saldo.ToString();
                    comboBox2.Visible = true;
                    comboBox2.DataSource = Enum.GetValues(typeof(TipoSaldo));
                    break;
            }
            comboBox2.Text = _subTipoMod;
            _liberado = true;
        }
        private void ComboBox2_SelectedIndexChanged(object sender, EventArgs ev)
        {
            if (_subTipoMod == null) return;
            var tipoElegido = (ComboBox)sender;
            if (_subTipoMod != "" && !_liberado2)
            {
                tipoElegido.Text = _subTipoMod;
                _liberado2 = true;
            }
        }
        private void TextBox8_TextChanged(object sender, EventArgs ev)   // buscar por id
        {
            if (textBox8.Text != "")
            {
                try { long.Parse(textBox8.Text); } catch { return; };
                Producto producto = getProducto(long.Parse(textBox8.Text));
                CargarDatosAModificar(producto);
            }
            else
            {
                label19.Text = "";
                textBox9.Text = "";
                _tipoMod = "";
                _subTipoMod = "";
                comboBox3.Visible = false;
                comboBox2.Visible = false;
            }
        }
        private void TextBox2_TextChanged(object sender, EventArgs ev)  // buscar por nombre
        {
            if (textBox2.Text != "")
            {
                Producto producto = getProductoPorNombre(textBox2.Text);
                CargarDatosAModificar(producto);
            }
            else
            {
                label19.Text = "";
                textBox9.Text = "";
                _tipoMod = "";
                _subTipoMod = "";
                comboBox3.Visible = false;
                comboBox2.Visible = false;
            }
        }
        bool TodoOKmodificarP()
        {
            bool subtipoVisibleButEmpty = comboBox2.Visible && comboBox2.Text == "";
            return label19.Text != "No encontrado" && label19.Text != "" && textBox9.Text != "" && !subtipoVisibleButEmpty;
        }
        private void EditarProducto_Click(object sender, EventArgs ev)
        {
            if (!TodoOKmodificarP())
            {
                MessageBox.Show("Verifique que se hayan llenado los campos correctamente");
                return;
            }
            Producto producto = getProductoPorNombreExacto(label19.Text);
            if (producto == null) return;
            try { decimal.Parse(textBox9.Text); } catch { return; };
            producto.Precio = decimal.Parse(textBox9.Text);
            producto.Tipo = comboBox3.Text;
            producto.SubTipo = comboBox2.Visible && comboBox2.Text != "" ? comboBox2.Text : "";
            editProducto(producto);
            button6.PerformClick();
        }



        //Borrar
        private void TextBox21_TextChanged(object sender, EventArgs ev)  // por id
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
        private void TextBox1_TextChanged(object sender, EventArgs ev)
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
        private void Eliminar_Click(object sender, EventArgs ev)
        {
            if (!cbSeguroBorrar.Checked)
            {
                MessageBox.Show("Tilde si esta seguro para borrar el producto");
                return;
            }
            Producto producto = getProductoPorNombreExacto(label46.Text);
            button22.Enabled = false;
            deleteProducto(producto);
            textBox21.Text = "";
            textBox1.Text = "";
            label46.Text = "";
            cbSeguroBorrar.Checked = false; 
        }
        private void SalirBtn_Click(object sender, EventArgs ev)
        {
            Close();
        }
    }
}