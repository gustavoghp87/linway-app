﻿using linway_app.Models;
using linway_app.Models.Enums;
using linway_app.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace linway_app.Forms
{
    public partial class FormClientes : Form
    {
        private readonly IServicioCliente _servCliente;

        public FormClientes(IServicioCliente servCliente)
        {
            InitializeComponent();
            _servCliente = servCliente;
        }

        private void FormClientes_Load(object sender, EventArgs e) {}

        private List<Cliente> GetClientes()
        {
            List<Cliente> lstClientes = _servCliente.GetAll();
            return lstClientes;
        }
        private Cliente GetCliente(long id)
        {
            Cliente cliente = _servCliente.Get(id);
            return cliente;
        }
        private void GuardarCliente(Cliente cliente)
        {
            bool response = _servCliente.Add(cliente);
            if (!response) MessageBox.Show("Falló guardado en base de datos");
        }
        private void EditarCliente(Cliente cliente)
        {
            bool response = _servCliente.Edit(cliente);
            if (!response) MessageBox.Show("Falló guardado en base de datos");
        }
        private void EliminarCliente(Cliente cliente)
        {
            bool response = _servCliente.Delete(cliente);
            if (!response) MessageBox.Show("Falló guardado en base de datos");
        }

        private void bCopiaSeguridad_Click(object sender, EventArgs e)
        {
            //GetClientes();
            //DialogResult dialogResult = MessageBox.Show("Esta acción reemplazará al actual Excel clientes.xlsx y demorará 15 segundos. ¿Confirmar?", "Exportar Clientes a Excel", MessageBoxButtons.YesNo);
            //if (dialogResult == DialogResult.Yes)
            //{
            //    bool success = new Exportar().ExportarAExcel(listaClientes);
            //    if (success)
            //    {
            //        //MessageBox.Show("Exportado Clientes con éxito a Copias de seguridad");
            //        bCopiaSeguridad.ForeColor = Color.Green;
            //        bCopiaSeguridad.Enabled = false;
            //        bCopiaSeguridad.Text = "Creacion exitosa";
            //    }
            //    else
            //    {
            //        MessageBox.Show("Hubo un error al guardar los cambios.");
            //    }
            //}
            //else if (dialogResult == DialogResult.No)
            //{
            //    return;
            //}
        }

        public void ImportarClientes_Click(object sender, EventArgs e)
        {
            //listaClientes.Clear();
            //GetClientes();
            //DialogResult dialogResult = MessageBox.Show("Esta acción reemplazará definitivamente el listado actual de clientes por el contenido del Excel clientes.xlsx en la carpeta Copias de seguridad. ¿Confirmar?", "Importar Clientes desde Excel", MessageBoxButtons.YesNo);
            //if (dialogResult == DialogResult.Yes)
            //{
            //    listaClientes = new Importar("clientes").ImportarClientes();
            //    if (listaClientes != null)
            //    {
            //        GuardarClientes();
            //        MessageBox.Show("Terminado");
            //    }
            //    else
            //    {
            //        MessageBox.Show("Falló Clientes; cancelado");
            //    }
            //    GetClientes();
            //}
        }





        // agregar Cliente

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
        private void AgregarCliente_Click(object sender, EventArgs e)
        {
            if (TodoOKagregarC())
            {
                TipoR tipo = TipoR.Monotributo;
                if (radioButton2.Checked)
                {
                    tipo = TipoR.Inscripto;
                }
                Cliente nuevoCliente = new Cliente {
                    Direccion = textBox2.Text + " - " + textBox18.Text,
                    CodigoPostal = textBox4.Text,
                    Telefono = textBox5.Text,
                    Name = textBox1.Text,
                    Cuit = textBox3.Text,
                    Tipo = tipo.ToString()
                };
                GuardarCliente(nuevoCliente);
                button2.PerformClick();
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

        //private void keyPress_SoloLetras(object sender, KeyPressEventArgs e)
        //{
        //    if (Char.IsLetter(e.KeyChar))
        //    {
        //        e.Handled = false;
        //    }
        //    else if (Char.IsControl(e.KeyChar))
        //    {
        //        e.Handled = false;
        //    }
        //    else if (Char.IsSeparator(e.KeyChar))
        //    {
        //        e.Handled = false;
        //    }
        //    else
        //    {
        //        e.Handled = true;
        //    }
        //}
        //private void keyPress_SoloNumeros(object sender, KeyPressEventArgs e)
        //{
        //    if (!(char.IsNumber(e.KeyChar)) && (e.KeyChar != (char)Keys.Back))
        //    {
        //        e.Handled = true;
        //        return;
        //    }
        //}


        //  modificar cliente
        bool TodoOkModificarC()
        {
            bool correcto = false;
            if ((label23.Text != "No encontrado") && (textBox10.Text != "") && (textBox11.Text != "")
                && (textBox14.Text != "") && (textBox23.Text != "") && (textBox24.Text != "") && (textBox25.Text != "")
            )
            {
                correcto = true;
            }
            return correcto;
        }

        private void PrepararEditar_Leave(object sender, EventArgs e)
        {
            bool encontrado = false;
            if (textBox14.Text != "")
            {
                Cliente cliente = GetCliente(long.Parse(textBox14.Text));
                if (cliente == null) return;
                if (cliente.Id == int.Parse(textBox14.Text))
                {
                    encontrado = true;
                    label23.Text = cliente.Direccion;
                    textBox23.Text = cliente.Direccion;
                    textBox24.Text = cliente.Telefono.ToString();
                    textBox25.Text = cliente.CodigoPostal.ToString();
                    textBox11.Text = cliente.Name;
                    textBox10.Text = cliente.Cuit;
                    if (cliente.Tipo == TipoR.Inscripto.ToString())
                    {
                        radioButton3.Checked = true;
                    }
                    else
                    {
                        radioButton4.Checked = true;
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
        private void Editar_Click(object sender, EventArgs e)
        {
            if (TodoOkModificarC())
            {
                List<Cliente> lstClientes = GetClientes();
                foreach (Cliente cliente in lstClientes)
                {
                    if (cliente.Direccion.Equals(label23.Text))
                    {
                        cliente.Direccion = textBox23.Text;
                        cliente.Telefono = textBox24.Text;
                        cliente.CodigoPostal = textBox25.Text;
                        cliente.Name = textBox11.Text;
                        cliente.Cuit = textBox10.Text;
                        if (radioButton3.Checked)
                        {
                            cliente.Tipo = TipoR.Inscripto.ToString();
                        }
                        else
                        {
                            cliente.Tipo = TipoR.Monotributo.ToString();
                        }
                        EditarCliente(cliente);
                    }
                }
                button8.PerformClick();
            }
            else
            {
                MessageBox.Show("Verifique que los campos sean correctos");
            }
        }



        //  Borrar clientes

        private void MostrarClienteEliminar_Leave(object sender, EventArgs e)
        {
            if (textBox22.Text != "")
            {
                Cliente cliente = GetCliente(long.Parse(textBox22.Text));
                if (cliente == null)
                {
                    label47.Text = "No encontrado";
                    button23.Enabled = false;
                    return;
                }
                label47.Text = cliente.Direccion;
                button23.Enabled = true;
            }
        }

        private void EliminarCliente_Click(object sender, EventArgs e)
        {
            if (cbSeguroBorrar.Checked)
            {
                List<Cliente> lstClientes = GetClientes();
                var cliente = lstClientes.Find(x => x.Direccion == label47.Text);
                EliminarCliente(cliente);
                cbSeguroBorrar.Checked = false;
                label47.Text = "";
                textBox22.Text = "";
            }
        }

        private void SalirBtn_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}