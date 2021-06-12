using linway_app.Models;
using linway_app.Models.Enums;
using System;
using System.Windows.Forms;
using static linway_app.Services.Delegates.DClientes;

namespace linway_app.Forms
{
    public partial class FormClientes : Form
    {
        public FormClientes()
        {
            InitializeComponent();
        }

        // agregar Cliente
        private void AgregarCliente_Click(object sender, EventArgs e)
        {
            if (textBox2.Text != "" && (radioButton1.Checked || radioButton2.Checked))
            {
                TipoR tipo = TipoR.Monotributo;
                if (radioButton2.Checked) tipo = TipoR.Inscripto;
                Cliente nuevoCliente = new Cliente {
                    Direccion = textBox18.Text != "" ? textBox2.Text + " - " + textBox18.Text : textBox2.Text,
                    CodigoPostal = textBox4.Text,
                    Telefono = textBox5.Text,
                    Nombre = textBox1.Text,
                    Cuit = textBox3.Text,
                    Tipo = tipo.ToString()
                };
                addCliente(nuevoCliente);
                button2.PerformClick();
            }
            else MessageBox.Show("Los campos Dirección y Responsable Inscr/Monotributo son obligatorios");
        }
        private void Limpiar_Click(object sender, EventArgs e)
        {
            label23.Text = "";
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            textBox6.Text = "";
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
        //private void KeyPress_SoloLetras(object sender, KeyPressEventArgs e)
        //{
        //    if (Char.IsLetter(e.KeyChar)) e.Handled = false;
        //    else if (Char.IsControl(e.KeyChar)) e.Handled = false;
        //    else if (Char.IsSeparator(e.KeyChar)) e.Handled = false;
        //    else e.Handled = true;
        //}
        //private void KeyPress_SoloNumeros(object sender, KeyPressEventArgs e)
        //{
        //    if (!char.IsNumber(e.KeyChar) && e.KeyChar != (char)Keys.Back)
        //    {
        //        e.Handled = true;
        //        return;
        //    }
        //}


        //  modificar cliente
        bool TodoOkModificarC()
        {
            return label23.Text != "No encontrado" && textBox10.Text != "" && textBox11.Text != ""
                && textBox23.Text != "" && textBox24.Text != "" && textBox25.Text != "";
        }
        private void DoIt(Cliente cliente)
        {
            if (cliente != null)
            {
                label23.Text = cliente.Direccion;
                textBox23.Text = cliente.Direccion;
                textBox24.Text = cliente.Telefono?.ToString();
                textBox25.Text = cliente.CodigoPostal?.ToString();
                textBox11.Text = cliente.Nombre;
                textBox10.Text = cliente.Cuit;
                if (cliente.Tipo == TipoR.Inscripto.ToString()) radioButton3.Checked = true;
                else radioButton4.Checked = true;
            }
            else
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
        private void TextBox14_TextChanged(object sender, EventArgs e)
        {
            if (textBox14.Text != "")
            {
                try { long.Parse(textBox14.Text); } catch { return; };
                Cliente cliente = getCliente(long.Parse(textBox14.Text));
                DoIt(cliente);
            }
            else
            {
                label23.Text = "";
                textBox11.Text = "";
                textBox10.Text = "";
                textBox23.Text = "";
                textBox24.Text = "";
                textBox25.Text = "";
                radioButton3.Checked = false;
                radioButton4.Checked = false;
            }
        }
        private void TextBox6_TextChanged(object sender, EventArgs e)
        {
            if (textBox6.Text != "")
            {
                Cliente cliente = getClientePorDireccion(textBox6.Text);
                DoIt(cliente);
            }
            else
            {
                label23.Text = "";
                textBox11.Text = "";
                textBox10.Text = "";
                textBox23.Text = "";
                textBox24.Text = "";
                textBox25.Text = "";
                radioButton3.Checked = false;
                radioButton4.Checked = false;
            }
        }
        private void Editar_Click(object sender, EventArgs e)
        {
            if (TodoOkModificarC())
            {
                Cliente cliente = getClientePorDireccionExacta(label23.Text);
                if (cliente == null) return;
                cliente.Direccion = textBox23.Text;
                cliente.Telefono = textBox24.Text;
                cliente.CodigoPostal = textBox25.Text;
                cliente.Nombre = textBox11.Text;
                cliente.Cuit = textBox10.Text;
                if (radioButton3.Checked)
                    cliente.Tipo = TipoR.Inscripto.ToString();
                else
                    cliente.Tipo = TipoR.Monotributo.ToString();
                editCliente(cliente);
                button8.PerformClick();
            }
            else MessageBox.Show("Verifique que los campos sean correctos");
        }


        //  Borrar clientes
        private void EliminarCliente_Click(object sender, EventArgs e)
        {
            if (cbSeguroBorrar.Checked)
            {
                var cliente = getClientePorDireccion(label47.Text);
                deleteCliente(cliente);
                cbSeguroBorrar.Checked = false;
                label47.Text = "";
                textBox22.Text = "";
            }
        }
        private void BorrarPorId_textBox_TextChanged(object sender, EventArgs e)
        {
            if (textBox22.Text != "")
            {
                try { long.Parse(textBox22.Text); } catch { return; };
                label47.Visible = true;
                Cliente cliente = getCliente(long.Parse(textBox22.Text));
                if (cliente == null)
                {
                    label47.Text = "No encontrado";
                    button23.Enabled = false;
                    return;
                }
                label47.Text = cliente.Direccion;
                button23.Enabled = true;
            }
            else
            {
                label47.Visible = false;
                label47.Text = "";
                button23.Enabled = false;
            }
        }
        private void BorrarPorDire_textBox_TextChanged(object sender, EventArgs e)
        {
            if (textBoxDireEnBorrar.Text != "")
            {
                label47.Visible = true;
                Cliente cliente = getClientePorDireccion(textBoxDireEnBorrar.Text);
                if (cliente == null)
                {
                    label47.Text = "No encontrado";
                    button23.Enabled = false;
                    return;
                }
                label47.Text = cliente.Direccion;
                button23.Enabled = true;
            }
            else
            {
                label47.Visible = false;
                label47.Text = "";
                button23.Enabled = false;
            }
        }
    }
}
