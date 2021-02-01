using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;


namespace linway_app
{
    public partial class FormVentas : Form
    {
        const string direccionVentas = @"Base de datos\VentaProductos.bin";
        const string direccionRegistro = @"Base de datos\RegistroVentas.bin";
        const string copiaDeSeguridad = "ventas.xlsx";
        const string copiaDeSeguridad2 = "registroVentas.xlsx";
        List<Venta> listaVentas = new List<Venta>();
        List<RegistroVenta> listaRegistro = new List<RegistroVenta>();
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

        public List<Venta> CargarVentas()
        {
            if (File.Exists(direccionVentas))
            {
                try
                {
                    Stream archivoVentas = File.OpenRead(direccionVentas);
                    BinaryFormatter traductor = new BinaryFormatter();
                    listaVentas = (List<Venta>) traductor.Deserialize(archivoVentas);
                    archivoVentas.Close();
                    dataGridView3.DataSource = listaVentas.ToArray();
                    dataGridView3.Columns[1].Width = 40;
                }
                catch (Exception e)
                {
                    MessageBox.Show("Error al leer las ventas: " + e.Message);
                }
            }
            else
            {
                MessageBox.Show("No se encontró el archivo Ventas en la carpeta Base de datos...");
            }
            return listaVentas;
        }

        public void CargarRegistros()
        {
            if (File.Exists(direccionRegistro))
            {
                try
                {
                    Stream archivo = File.OpenRead(direccionRegistro);
                    BinaryFormatter traductor = new BinaryFormatter();
                    listaRegistro = (List<RegistroVenta>)traductor.Deserialize(archivo);
                    archivo.Close();

                    if (listaRegistro.Count > 0)
                        new RegistroVenta(listaRegistro.ElementAt(listaRegistro.Count - 1).Id);
                    dataGridView1.DataSource = listaRegistro.ToArray();
                    dataGridView1.Columns[0].Width = 34;
                    dataGridView1.Columns[1].Width = 67;
                    label1.Text = "Registro de ventas (" + listaRegistro.Count.ToString() + ")";
                }
                catch (Exception e)
                {
                    MessageBox.Show("Error al leer el registro de ventas: " + e.Message);
                }
            }
            else
            {
                MessageBox.Show("No se encontró el archivo RegistroVentas en la carpeta Base de datos...");
            }
        }

        private void GuardarVentas()
        {
            try
            {
                Stream archivo = File.Create(direccionVentas);
                BinaryFormatter traductor3 = new BinaryFormatter();
                traductor3.Serialize(archivo, listaVentas);
                archivo.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show("Error al guardar las ventas:" + e.Message);
            }
        }

        private void GuardarRegistros()
        {
            try
            {
                Stream archivo = File.Create(direccionRegistro);
                BinaryFormatter traductor3 = new BinaryFormatter();
                traductor3.Serialize(archivo, listaRegistro);
                archivo.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show("Error al guardar registro de ventas: " + e.Message);
            }
        }

        private void bCopiaSeguridad_Click(object sender, EventArgs e)
        {
            CargarVentas();
            CargarRegistros();
            DialogResult dialogResult = MessageBox.Show("Esta acción reemplazará al actual Excel ventas.xlsx y demorará 15 segundos. ¿Confirmar?", "Exportar Ventas a Excel", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                bool success = new Exportar().ExportarAExcel(listaVentas);
                if (!success)
                {
                    MessageBox.Show("Hubo un error al guardar los cambios.");
                }
            }

            DialogResult dialogResult2 = MessageBox.Show("Esta acción reemplazará al actual Excel registroVentas.xlsx y demorará 15 segundos. ¿Confirmar?", "Exportar Registro de Ventas a Excel", MessageBoxButtons.YesNo);
            if (dialogResult2 == DialogResult.Yes)
            {
                bool success = new Exportar().ExportarAExcel(listaRegistro);
                if (success)
                {
                    bCopiaSeguridad.ForeColor = Color.Green;
                    bCopiaSeguridad.Enabled = false;
                    bCopiaSeguridad.Text = "Creacion exitosa";
                    MessageBox.Show("Terminados ambos archivos Excel: ventas y registroVentas");
                }
                else
                {
                    MessageBox.Show("Hubo un error al guardar los cambios.");
                }
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
                listaVentas = new Importar(copiaDeSeguridad).ImportarVentas();
                if (listaVentas != null)
                {
                    GuardarVentas();
                }
                else
                {
                    MessageBox.Show("Falló Ventas; cancelado");
                }
                CargarVentas();

                listaRegistro.Clear();
                listaRegistro = new Importar(copiaDeSeguridad2).ImportarRegistroVentas();
                if (listaRegistro != null)
                {
                    GuardarRegistros();
                }
                else
                {
                    MessageBox.Show("Falló Registro de Ventas; cancelado");
                }
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
        private void actualizarListas_Click(object sender, EventArgs e)
        {
            CargarVentas();
            CargarRegistros();
        }

        //nueva venta
        private void nuevaVentaToolStripMenuItem_Click(object sender, EventArgs e)
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
            foreach (Producto pActual in listaProductos)
            {
                if (listaVentas.Exists(x => x.Producto.Equals(pActual.Nombre)))
                {
                    nuevaLista.Add(listaVentas.Find(x => x.Producto.Equals(pActual.Nombre)));
                }
            }
            listaVentas = nuevaLista;
        }

        private void cancelarClick_Click(object sender, EventArgs e)
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
                        if (pActual.Codigo == int.Parse(textBox12.Text))
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
        private bool esProducto(string nombre)
        {
            bool es = true;
            if ((nombre.Contains("pendiente")) || (nombre.Contains("favor")) || (nombre.Contains("actura")) || (nombre.Contains("evoluc")) || (nombre.Contains("cobrar") || (nombre.Contains("BONIFI"))))
            {
                es = false;
            }
            return es;
        }
        private void agregarVenta_Click(object sender, EventArgs e)
        {
            if (listaAgregarVentas.Count > 0)
            {
                CargarVentas();
                RegistroVenta nuevoRegistro = new RegistroVenta();
                nuevoRegistro.RecibirListaVentas(listaAgregarVentas, listaProductos);
                foreach (Venta vAgregar in listaAgregarVentas)
                {
                    if (esProducto(vAgregar.Producto))
                    {
                        bool existe = false;
                        foreach (Venta vActual in listaVentas)
                        {
                            if (vActual.Producto == vAgregar.Producto)
                            {
                                existe = true;
                                vActual.RealizarVenta(vAgregar.Cantidad);
                            }
                        }
                        if (!existe)
                        {
                            listaVentas.Add(vAgregar);
                        }
                    }
                }
                if (checkBox2.Checked)
                {
                    nuevoRegistro.Cliente = label20.Text;
                    FormReparto fr = new FormReparto();
                    fr.CargarAHojaDeReparto(label20.Text, comboBox1.Text, comboBox2.Text, listaAgregarVentas);
                    fr.Close();
                }
                ReordenarVentas();
                GuardarVentas();
                listaAgregarVentas.Clear();
                listaRegistro.Add(nuevoRegistro);
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
                Venta nVenta = new Venta(label28.Text);
                nVenta.RealizarVenta(int.Parse(textBox13.Text));
                listaAgregarVentas.Add(nVenta);
                dataGridView5.DataSource = listaAgregarVentas.ToArray();
            }
            label28.Text = "";
            textBox13.Text = "";
            textBox12.Text = "";
        }

        //enviar HDR
        private void checkBox2_CheckedChanged(object sender, EventArgs e)
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
            comboBox2.DataSource = fr.DarRepartos(comboBox1.Text);
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
                if (listaClientes.Exists(x => x.Numero == int.Parse(textBox19.Text)))
                {
                    label20.Text = (listaClientes.Find(x => x.Numero == int.Parse(textBox19.Text)).Direccion);
                }
                else
                {
                    label20.Text = "No encontrado";
                }
            }
        }

        //reiniciar ventas
        private void reiniciarVentasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LimpiarPantalla();
            groupBox7.Visible = true;
        }

        private void reiniciarVentas_Click(object sender, EventArgs e)
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

        //recibir ventas
        public void RecibirProductosVendidos(List<ProdVendido> listaPV, String client)
        {
            try
            {
                RegistroVenta nuevoRegistro = new RegistroVenta();
                nuevoRegistro.RecibirProdVendidos(listaPV, client);
                foreach (ProdVendido pvActual in listaPV)
                {
                    if (esProducto(pvActual.Descripcion))
                    {
                        bool existe = false;
                        foreach (Venta vActual in listaVentas)
                        {
                            if (vActual.Producto.Equals(pvActual.Descripcion))
                            {
                                existe = true;
                                vActual.RealizarVenta(pvActual.Cantidad);
                            }
                        }
                        if (!existe)
                        {
                            Venta nuevaVenta = new Venta(pvActual.Descripcion);
                            nuevaVenta.RealizarVenta(pvActual.Cantidad);
                            listaVentas.Add(nuevaVenta);
                        }
                    }
                }
                GuardarVentas();
                listaRegistro.Add(nuevoRegistro);
                GuardarRegistros();
            }
            catch (Exception h)
            {
                MessageBox.Show("Tercera parte: " + h.Message);
            }
        }






        ///////////////////////////////REGISTRO DE VENTAS//////////////////////////////////////

        private void verRegistroToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LimpiarPantalla();
            gbVerRegistro.Visible = true;
            dataGridView2.DataSource = new List<ProdVendido>().ToArray();
        }

        
        //Deshacer una venta
        private void textBox1_Leave(object sender, EventArgs e)
        {
            bool encontrado = false;
            if (textBox1.Text != "")
            {
                foreach (RegistroVenta rActual in listaRegistro)
                {
                    if (rActual.Id == uint.Parse(textBox1.Text))
                    {
                        encontrado = true;
                        labelFecha.Text = rActual.Fecha;
                        labelCliente.Text = rActual.Cliente;
                        dataGridView2.DataSource = rActual.ObtenerPV().ToArray();
                        labelTotal.Text = "Total: $" + rActual.ObtenerTotal().ToString();
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
                RegistroVenta elRegistro = listaRegistro.Find(x => x.Id == uint.Parse(textBox1.Text.ToString()));
                foreach (ProdVendido pvActual in elRegistro.ObtenerPV())
                    if (esProducto(pvActual.Descripcion))
                        foreach (Venta vActual in listaVentas)
                            if (vActual.Producto.Equals(pvActual.Descripcion))
                                vActual.RealizarVenta(pvActual.Cantidad * -1);
                GuardarVentas();
                listaRegistro.Remove(elRegistro);
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
                foreach (RegistroVenta rActual in listaRegistro)
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
                dataGridView1.DataSource = listaRegistro.ToArray();
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
            foreach (RegistroVenta rActual in listaRegistro)
            {
                if (x == 'c')
                {
                    if (rActual.Cliente.Contains(texto))
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

        private bool SeEncuentraEnIntervalo(uint id)
        {
            uint primero = uint.Parse(tbDesde.Text);
            uint segundo = uint.Parse(tbHasta.Text);
            return ((primero <= id) & (segundo >= id));
        }

        private void bBorrarRegVentas_Click(object sender, EventArgs e)
        {
            List<RegistroVenta> registrosABorrar = new List<RegistroVenta>();
            if (IntervaloCorrecto())
            {
                listaRegistro.RemoveAll(x => SeEncuentraEnIntervalo(x.Id));
                GuardarRegistros();
                LimpiarPantalla();
                bActualizar.PerformClick();
            }
        }
    }
}
