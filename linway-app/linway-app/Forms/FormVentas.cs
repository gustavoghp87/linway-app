using linway_app.Models;
using linway_app.Excel;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace linway_app.Forms
{
    public partial class FormVentas : Form
    {
        private int idNuevoRegistro;
        List<Venta> listaVentas = new List<Venta>();
        List<RegistroVenta> listaRegistros = new List<RegistroVenta>();
        readonly List<Cliente> listaClientes = new List<Cliente>();
        readonly List<Producto> listaProductos = new List<Producto>();
        readonly List<Venta> listaAgregarVentas = new List<Venta>();

        public FormVentas()
        {
            InitializeComponent();
        }

        private void FormVentas_Load(object sender, EventArgs e)
        {
            CargarVentas();
            CargarRegistros();
        }

        public void CargarVentas()
        {
            //listaVentas = GetData.GetSales();
            if (listaVentas == null) return;
            dataGridView3.DataSource = listaVentas.ToArray();
            dataGridView3.Columns[1].Width = 40;
        }

        public void CargarRegistros()
        {
            //listaRegistros = GetData.GetSalesRecord();
            //if (listaRegistros == null) return;
            //idNuevoRegistro = listaRegistros.ElementAt(listaRegistros.Count - 1).Id + 1;   // Last()    ???
            //dataGridView1.DataSource = listaRegistros.ToArray();
            //dataGridView1.Columns[0].Width = 34;
            //dataGridView1.Columns[1].Width = 67;
            //label1.Text = "Registro de ventas (" + listaRegistros.Count.ToString() + ")";
        }

        private void GuardarVentas()
        {
            //SetData.SetSales(listaVentas);
        }

        private void GuardarRegistros()
        {
            //SetData.SetSalesRecord(listaRegistros);
        }

        private void bCopiaSeguridad_Click(object sender, EventArgs e)
        {
            CargarVentas();
            CargarRegistros();
            DialogResult dialogResult = MessageBox.Show("Esta acción reemplazará al actual Excel ventas.xlsx y demorará 15 segundos. ¿Confirmar?", "Exportar Ventas a Excel", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                //bool success = new Exportar().ExportarAExcel(listaVentas);
                //if (!success)
                //{
                //    MessageBox.Show("Hubo un error al guardar los cambios.");
                //}
            }

            DialogResult dialogResult2 = MessageBox.Show("Esta acción reemplazará al actual Excel registroVentas.xlsx y demorará 15 segundos. ¿Confirmar?", "Exportar Registro de Ventas a Excel", MessageBoxButtons.YesNo);
            if (dialogResult2 == DialogResult.Yes)
            {
                //bool success = new Exportar().ExportarAExcel(listaRegistros);
                //if (success)
                //{
                //    bCopiaSeguridad.ForeColor = Color.Green;
                //    bCopiaSeguridad.Enabled = false;
                //    bCopiaSeguridad.Text = "Creacion exitosa";
                //    MessageBox.Show("Terminados ambos archivos Excel: ventas y registroVentas");
                //}
                //else
                //{
                //    MessageBox.Show("Hubo un error al guardar los cambios.");
                //}
            }
        }

        private void ImportarBtn_Click(object sender, EventArgs e)
        {
            CargarVentas();
            CargarRegistros();
            DialogResult dialogResult = MessageBox.Show("Esta acción reemplazará definitivamente el listado actual de ventas por el contenido del Excel ventas.xlsx, y el registro de ventas por el contenido del Excel registroVentas.xlsx, en la carpeta Copias de seguridad. ¿Confirmar?", "Importar Ventas y Registro de Ventas desde Excel", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                listaVentas.Clear();
                //listaVentas = new Importar("ventas").ImportarVentas();
                //if (listaVentas != null)
                //{
                //    GuardarVentas();
                //}
                //else
                //{
                //    MessageBox.Show("Falló Ventas; cancelado");
                //}
                //CargarVentas();

                //listaRegistros.Clear();
                //listaRegistros = new Importar("registroVentas").ImportarRegistroVentas();
                //if (listaRegistros != null)
                //{
                //    GuardarRegistros();
                //}
                //else
                //{
                //    MessageBox.Show("Falló Registro de Ventas; cancelado");
                //}
                CargarRegistros();
            }
        }

        private void exportarAExcel_Click(object sender, EventArgs e)
        {
        //   anulado
        }

        public void ObtenerDatos(List<Cliente> clientes, List<Producto> productos)
        {
            listaClientes.AddRange(clientes);
            listaProductos.AddRange(productos);
        }

        public void LimpiarPantalla()
        {
            gbNuevaVenta.Visible = false;
            gbVerRegistro.Visible = false;
            groupBox7.Visible = false;
            label28.Text = "";
            label20.Text = "";
            textBox19.Text = "";
            checkBox2.Checked = false;
            cbSeguro.Checked = false;
            textBox12.Text = "";
            textBox13.Text = "";
            textBox1.Text = "";
            labelFecha.Text = "";
            labelTotal.Text = "";
            labelCliente.Text = "";
            cbSeguroBorrar.Checked = false;
            gbBorrarReg.Visible = false;
            tbDesde.Text = "";
            tbHasta.Text = "";
            checkBox1.Checked = false;
        }

        //Actualizar
        private void ActualizarListas_Click(object sender, EventArgs e)
        {
            CargarVentas();
            CargarRegistros();
        }

        //nueva venta
        private void NuevaVenta_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LimpiarPantalla();
            gbNuevaVenta.Visible = true;
            listaAgregarVentas.Clear();
            dataGridView5.DataSource = listaAgregarVentas.ToArray();
            dataGridView5.Columns[1].Width = 60;
        }

        private void ReordenarVentas()
        {
            List<Venta> nuevaLista = new List<Venta>();
            foreach (Producto producto in listaProductos)
            {
                var venta = listaVentas.Find(x => x.ProductoId.Equals(producto.Id));
                if (venta != null)
                    nuevaLista.Add(venta);
            }
            listaVentas = nuevaLista;
        }

        private void CancelarClick_Click(object sender, EventArgs e)
        {
            LimpiarPantalla();
        }

        private void SoloNumero_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsNumber(e.KeyChar)) && (e.KeyChar != (char)Keys.Back))
            {
                e.Handled = true;
                return;
            }
        }

        private void SoloNumeroYNegativo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && (!char.IsDigit(e.KeyChar))
         && (e.KeyChar != '-'))
                e.Handled = true;

            if (e.KeyChar == '-' && (sender as TextBox).Text.Length > 0)
                e.Handled = true;
        }

        private void textBox12_Leave(object sender, EventArgs e)
        {
            bool encontrado = false;
            if (textBox12.Text != "")
            {
                try
                {
                    foreach (Producto pActual in listaProductos)
                    {
                        if (pActual.Id == int.Parse(textBox12.Text))
                        {
                            encontrado = true;
                            label28.Text = pActual.Nombre;
                        }
                    }
                }
                catch
                {
                    return;
                }
            }
            if (!encontrado)
            {
                label28.Text = "No encontrado";
            }
        }
        private bool EsProducto(long productoId)
        {
            bool es = true;
            string nombreProd = listaProductos.Find(x => x.Id == productoId).Nombre;
            if ((nombreProd.Contains("pendiente")) || (nombreProd.Contains("favor")) || (nombreProd.Contains("actura"))
                 || (nombreProd.Contains("evoluc")) || (nombreProd.Contains("cobrar") || (nombreProd.Contains("BONIFI")))
            )
            {
                es = false;
            }
            return es;
        }
        private void AgregarVenta_Click(object sender, EventArgs e)
        {
            if (listaAgregarVentas.Count > 0)
            {
                CargarVentas();
                // RegistroVenta nuevoRegistro = new RegistroVenta(idNuevoRegistro);
                RegistroVenta nuevoRegistro = new RegistroVenta { Id = idNuevoRegistro };
                idNuevoRegistro++;
                //nuevoRegistro.RecibirListaVentas(listaAgregarVentas, listaProductos);
                //foreach (Venta venta in listaAgregarVentas)
                //{
                //    if (EsProducto(venta.ProductoId))
                //    {
                //        bool existe = false;
                //        foreach (Venta ventaActual in listaVentas)
                //        {
                //            if (ventaActual.ProductoId == venta.ProductoId)
                //            {
                //                existe = true;
                //                ventaActual.Cantidad += venta.Cantidad;
                //            }
                //        }
                //        if (!existe)
                //        {
                //            listaVentas.Add(venta);
                //        }
                //    }
                //}
                if (checkBox2.Checked)
                {
                    nuevoRegistro.NombreCliente = label20.Text;
                    FormReparto fr = new FormReparto();
                    fr.CargarAHojaDeReparto(label20.Text, comboBox1.Text, comboBox2.Text, listaAgregarVentas);
                    fr.Close();
                }
                ReordenarVentas();
                GuardarVentas();
                listaAgregarVentas.Clear();
                listaRegistros.Add(nuevoRegistro);
                GuardarRegistros();
                bActualizar.PerformClick();
                LimpiarPantalla();
            }
            else
            {
                MessageBox.Show("No se han ingresado productos");
            }
        }

        private void button18_Click(object sender, EventArgs e)
        {
            listaAgregarVentas.Clear();
            dataGridView5.DataSource = listaAgregarVentas.ToArray();
            label28.Text = "";
            textBox13.Text = "";
            textBox12.Text = "";
        }

        private void button17_Click(object sender, EventArgs e)
        {
            if ((label28.Text != "No encontrado") && (textBox13.Text != "") && (textBox12.Text != ""))
            {
                Producto prod = listaProductos.Find(x => x.Nombre == label28.Text);
                if (prod == null) return;
                // Venta nuevaVenta = new Venta(prod.Id, int.Parse(textBox13.Text));
                Venta nuevaVenta = new Venta { ProductoId = prod.Id, Cantidad = int.Parse(textBox13.Text) };
                listaAgregarVentas.Add(nuevaVenta);
                dataGridView5.DataSource = listaAgregarVentas.ToArray();
            }
            label28.Text = "";
            textBox13.Text = "";
            textBox12.Text = "";
        }

        //enviar HDR
        private void CheckBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                label6.Visible = true;
                label7.Visible = true;
                comboBox2.Visible = true;
                comboBox1.Visible = true;
                label15.Visible = true;
                label20.Text = "";
                textBox19.Visible = true;
            }
            else
            {
                label6.Visible = false;
                label7.Visible = false;
                comboBox2.Visible = false;
                comboBox1.Visible = false;
                label15.Visible = false;
                label20.Text = "";
                textBox19.Visible = false;
                textBox19.Text = "";
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            FormReparto fr = new FormReparto();
            //comboBox2.DataSource = fr.DarRepartos(comboBox1.Text);
            comboBox2.DisplayMember = "Nombre";
            comboBox2.ValueMember = "Nombre";
            fr.Close();
        }

        private void textBox19_Leave(object sender, EventArgs e)
        {
            if (textBox19.Text == "")
            {
                label20.Text = "No encontrado";
            }
            else
            {
                if (listaClientes.Exists(x => x.Id == int.Parse(textBox19.Text)))
                {
                    label20.Text = (listaClientes.Find(x => x.Id == int.Parse(textBox19.Text)).Direccion);
                }
                else
                {
                    label20.Text = "No encontrado";
                }
            }
        }

        //reiniciar ventas
        private void ReiniciarVentas_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LimpiarPantalla();
            groupBox7.Visible = true;
        }

        private void ReiniciarVentas_Click(object sender, EventArgs e)
        {
            if (cbSeguroBorrar.Checked)
            {
                listaVentas.Clear();
                GuardarVentas();
                LimpiarPantalla();
                CargarVentas();
            }
            else
            {
                MessageBox.Show("Seleccione si esta seguro para borrar la lista de ventas");
            }
        }




        ///////////////////////////////REGISTRO DE VENTAS//////////////////////////////////////

        private void VerRegistro_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LimpiarPantalla();
            gbVerRegistro.Visible = true;
            dataGridView2.DataSource = new List<ProdVendido>().ToArray();
        }

        
        //Deshacer una venta
        private void TextBox1_Leave(object sender, EventArgs e)
        {
            bool encontrado = false;
            if (textBox1.Text != "")
            {
                foreach (RegistroVenta rActual in listaRegistros)
                {
                    if (rActual.Id == uint.Parse(textBox1.Text))
                    {
                        encontrado = true;
                        labelFecha.Text = rActual.Fecha;
                        labelCliente.Text = rActual.NombreCliente;
                        //dataGridView2.DataSource = rActual.ProductosVendidos.ToArray();
                        //labelTotal.Text = "Total: $" + rActual.ObtenerTotal().ToString();
                        bDeshacerVenta.Enabled = true;
                    }
                }
            }
            if (!encontrado)
            {
                labelCliente.Text = "NO SE ENCONTRO REGISTRO";
                labelFecha.Text = "XX/XX/XXXX";
                labelTotal.Text = "Total: $XXXX";
                dataGridView2.DataSource = new List<ProdVendido>().ToArray();
                bDeshacerVenta.Enabled = false;
            }
        }

        private void bDeshacerVenta_Click(object sender, EventArgs e)
        {
            if (cbSeguro.Checked)
            {
                RegistroVenta registro = listaRegistros.Find(x => x.Id == int.Parse(textBox1.Text.ToString()));
                //foreach (ProdVendido prodVendido in registro.ProductosVendidos)
                //    if (EsProducto(prodVendido.ProductoId))
                //        foreach (Venta venta in listaVentas)
                //            if (venta.ProductoId.Equals(prodVendido.ProductoId))
                //                venta.Cantidad -= prodVendido.Cantidad;
                GuardarVentas();
                listaRegistros.Remove(registro);
                GuardarRegistros();
                bActualizar.PerformClick();
                LimpiarPantalla();
            }
            else
            {
                MessageBox.Show("Debe confirmar que esta seguro de deshacer este registro.");
            }
        }

        //////Filtrar datos. 
        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<RegistroVenta> lFiltrada = new List<RegistroVenta>();
            if (comboBox3.SelectedItem.ToString() == "Hoy")
            {
                textBox2.Text = "";
                textBox2.Visible = false;
                foreach (RegistroVenta rActual in listaRegistros)
                {
                    if (rActual.Fecha == DateTime.Today.ToString().Substring(0, 10))
                    {
                        lFiltrada.Add(rActual);
                    }
                }
                dataGridView1.DataSource = lFiltrada.ToArray();
            }
            else if (comboBox3.SelectedItem.ToString() == "Todas")
            {
                textBox2.Text = "";
                textBox2.Visible = false;
                dataGridView1.DataSource = listaRegistros.ToArray();
            }
            else
            {
                textBox2.Text = "";
                textBox2.Visible = true;
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (comboBox3.SelectedItem.ToString() == "Cliente")
            {
                FiltrarDatos(textBox2.Text, 'c');
            }
            if (comboBox3.SelectedItem.ToString() == "Fecha")
            {
                FiltrarDatos(textBox2.Text, 'f');
            }
        }

        void FiltrarDatos(string texto, char x)
        {
            List<RegistroVenta> ListaFiltrada = new List<RegistroVenta>();
            foreach (RegistroVenta rActual in listaRegistros)
            {
                if (x == 'c')
                {
                    if (rActual.NombreCliente.Contains(texto))
                    {
                        ListaFiltrada.Add(rActual);
                    }
                }
                if (x == 'f')
                {
                    if (rActual.Fecha.Contains(texto))
                    {
                        ListaFiltrada.Add(rActual);
                    }
                }
            }
            dataGridView1.DataSource = ListaFiltrada.ToArray();
        }

        //Borrar registro de ventas
        private void borrarRegistrosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LimpiarPantalla();
            gbBorrarReg.Visible = true;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (bBorrarRegVentas.Enabled)
                bBorrarRegVentas.Enabled = false;
            else
                bBorrarRegVentas.Enabled = true;
        }

        private bool IntervaloCorrecto()
        {
            bool todoOk = false;
            if ((tbDesde.Text != "") & (tbHasta.Text != ""))
            {
                int primero = int.Parse(tbDesde.Text);
                int segundo = int.Parse(tbHasta.Text);
                todoOk = (primero <= segundo);
                if (!todoOk)
                    MessageBox.Show("intervalo incorrecto.");
            }
            else
            {
                MessageBox.Show("Falta llenar algunos campos");
            }
            return todoOk;
        }

        private bool SeEncuentraEnIntervalo(int id)
        {
            int primero = int.Parse(tbDesde.Text);
            int segundo = int.Parse(tbHasta.Text);
            return ((primero <= id) & (segundo >= id));
        }

        private void bBorrarRegVentas_Click(object sender, EventArgs e)
        {
            List<RegistroVenta> registrosABorrar = new List<RegistroVenta>();
            if (IntervaloCorrecto())
            {
                //listaRegistros.RemoveAll(x => SeEncuentraEnIntervalo(x.Id));
                GuardarRegistros();
                LimpiarPantalla();
                bActualizar.PerformClick();
            }
        }
    }
}
