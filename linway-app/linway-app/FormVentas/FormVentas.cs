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


namespace linway_app
{
    public partial class FormVentas : Form
    {
        const string direccionVentas = "VentaProductos.bin";
        const string copiaDeSeguridad = @"Copias de seguridad\VentaProductos.bin";
        List<Cliente> listaClientes = new List<Cliente>();
        List<Producto> listaProductos = new List<Producto>();
        List<Venta> listaVentas = new List<Venta>();
        List<Venta> listaAgregarVentas = new List<Venta>();

        public FormVentas()
        {
            InitializeComponent();
        }

        private void FormVentas_Load(object sender, EventArgs e)
        {
            CargarVentas();
            CargarRegistros();
        }

        public void ObtenerDatos(List<Cliente> clientes, List<Producto> productos)
        {
            listaClientes.AddRange(clientes);
            listaProductos.AddRange(productos);
        }

        private void GuardarVentas()
        {
            try
            {
                Stream archivoVentas = File.Create(direccionVentas);
                BinaryFormatter traductor3 = new BinaryFormatter();
                traductor3.Serialize(archivoVentas, listaVentas);
                archivoVentas.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show("Error al guardar las ventas:" + e.Message);
            }
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
                }
                catch (Exception e)
                {
                    GuardarVentas();
                    MessageBox.Show("Error al leer las ventas (interno). Se creó uno nuevo." + e.Message);
                }
            }
            else
            {
                GuardarVentas();
                try
                {
                    Stream archivoVentas = File.OpenRead(direccionVentas);
                    BinaryFormatter traductor = new BinaryFormatter();
                    listaVentas = (List<Venta>)traductor.Deserialize(archivoVentas);
                    archivoVentas.Close();
                    MessageBox.Show("Se creó VentaProductos.bin porque no existía");
                }
                catch (Exception e)
                {
                    GuardarVentas();
                    MessageBox.Show("Error al leer las ventas (interno). No existe VentaProductos.bin y no se pudo crear uno." + e.Message);
                }
            }
            dataGridView3.DataSource = listaVentas.ToArray();
            dataGridView3.Columns[1].Width = 40;
            return listaVentas;
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

        //crear copia seguridad
        private void crearCopiaSeguridad_Click(object sender, EventArgs e)
        {
            try
            {
                Stream archivoVentas = File.Create(copiaDeSeguridad);
                BinaryFormatter traductor3 = new BinaryFormatter();
                traductor3.Serialize(archivoVentas, listaVentas);
                archivoVentas.Close();
            }
            catch (Exception f)
            {
                MessageBox.Show("Error al crear copia de seguridad de ventas:" + f.Message);
            }
            try
            {
                Stream archivoRegistros = File.Create(copiaDeSeguridadRegistro);
                BinaryFormatter traductor4 = new BinaryFormatter();
                traductor4.Serialize(archivoRegistros, listaRegistro);
                archivoRegistros.Close();
                bSeguridad.Text = "Creacion exitosa";
                bSeguridad.Enabled = false;
            }
            catch (Exception g)
            {
                MessageBox.Show("Error al crear copia de seguridad de registro:" + g.Message);
            }
        }

        //exportar
        private void exportarAExcel_Click(object sender, EventArgs e)
        {
            ExportarDataGridViewExcel(dataGridView3);
        }

        private void ExportarDataGridViewExcel(DataGridView grd)
        {
            SaveFileDialog fichero = new SaveFileDialog();
            fichero.Filter = "Excel (*.xls)|*.xls";
            if (fichero.ShowDialog() == DialogResult.OK)
            {
                Microsoft.Office.Interop.Excel.Application aplicacion;
                Microsoft.Office.Interop.Excel.Workbook libros_trabajo;
                Microsoft.Office.Interop.Excel.Worksheet hoja_trabajo;
                Microsoft.Office.Interop.Excel.Range excelCellrange;
                aplicacion = new Microsoft.Office.Interop.Excel.Application();
                libros_trabajo = aplicacion.Workbooks.Add();
                hoja_trabajo =
                    (Microsoft.Office.Interop.Excel.Worksheet)libros_trabajo.Worksheets.get_Item(1);
                //Recorremos el DataGridView rellenando la hoja de trabajo
                for (int i = 0; i < grd.Rows.Count; i++)
                {
                    for (int j = 0; j < grd.Columns.Count; j++)
                    {
                        hoja_trabajo.Cells[i + 3, j + 1] = grd.Rows[i].Cells[j].Value.ToString();
                    }
                }
                //cabezera
                hoja_trabajo.Cells[1, 1] = "VENTAS";
                hoja_trabajo.Cells[1, 1].Font.Bold = true;
                hoja_trabajo.Cells[1, 1].Font.Underline = true;
                hoja_trabajo.Cells[1, 1].Font.Size = 11;
                hoja_trabajo.Cells[2, 1] = "Producto";
                hoja_trabajo.Cells[2, 2] = "Cantidad";
                //Establecer rango de celdas
                excelCellrange = hoja_trabajo.Range[hoja_trabajo.Cells[2, 1], hoja_trabajo.Cells[grd.Rows.Count + 2, grd.Columns.Count]];
                //Autoestablecer ancho de columnas
                excelCellrange.EntireColumn.AutoFit();
                //rellenar bordes
                Microsoft.Office.Interop.Excel.Borders border = excelCellrange.Borders;
                border.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                border.Weight = 2d;
                //guardar.
                libros_trabajo.SaveAs(fichero.FileName,
                    Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookNormal);
                libros_trabajo.Close(true);
                aplicacion.Quit();
            }
        }

        //nueva venta
        private void nuevaVentaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LimpiarPantalla();
            gbNuevaVenta.Visible = true;
            listaAgregarVentas.Clear();
            dataGridView5.DataSource = listaAgregarVentas.ToArray();
            dataGridView5.Columns[1].Width = 40;
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
                foreach (Producto pActual in listaProductos)
                {
                    if (pActual.Codigo == int.Parse(textBox12.Text))
                    {
                        encontrado = true;
                        label28.Text = pActual.Nombre;
                    }
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
                                vActual.realizarVenta(vAgregar.Cantidad);
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
                    nuevoRegistro.cliente = label20.Text;
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
                nVenta.realizarVenta(int.Parse(textBox13.Text));
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
                                vActual.realizarVenta(pvActual.Cantidad);
                            }
                        }
                        if (!existe)
                        {
                            Venta nuevaVenta = new Venta(pvActual.Descripcion);
                            nuevaVenta.realizarVenta(pvActual.Cantidad);
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

        const string direccionRegistro = "RegistroVentas.bin";
        const string copiaDeSeguridadRegistro = @"Copias de seguridad\RegistroVentas.bin";
        private List<RegistroVenta> listaRegistro = new List<RegistroVenta>();

        private void verRegistroToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LimpiarPantalla();
            gbVerRegistro.Visible = true;
            dataGridView2.DataSource = new List<ProdVendido>().ToArray();
        }

        private void GuardarRegistros()
        {
            try
            {
                Stream archivoRegistros = File.Create(direccionRegistro);
                BinaryFormatter traductor3 = new BinaryFormatter();
                traductor3.Serialize(archivoRegistros, listaRegistro);
                archivoRegistros.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show("Error al guardar registro de ventas:" + e.Message);
            }
        }

        public void CargarRegistros()
        {
            if (File.Exists(direccionRegistro))
            {
                try
                {
                    Stream archivoRegistro = File.OpenRead(direccionRegistro);
                    BinaryFormatter traductor = new BinaryFormatter();
                    listaRegistro = (List<RegistroVenta>) traductor.Deserialize(archivoRegistro);
                    archivoRegistro.Close();
                    if (listaRegistro.Count > 0)
                        new RegistroVenta(listaRegistro.ElementAt(listaRegistro.Count - 1).id);
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
                MessageBox.Show("Se creará RegistroVentas.bin porque no existe");
                GuardarRegistros();
            }
        }

        //Deshacer una venta
        private void textBox1_Leave(object sender, EventArgs e)
        {
            bool encontrado = false;
            if (textBox1.Text != "")
            {
                foreach (RegistroVenta rActual in listaRegistro)
                {
                    if (rActual.id == uint.Parse(textBox1.Text))
                    {
                        encontrado = true;
                        labelFecha.Text = rActual.fecha;
                        labelCliente.Text = rActual.cliente;
                        dataGridView2.DataSource = rActual.obtenerPV().ToArray();
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
                RegistroVenta elRegistro = listaRegistro.Find(x => x.id == uint.Parse(textBox1.Text.ToString()));
                foreach (ProdVendido pvActual in elRegistro.obtenerPV())
                    if (esProducto(pvActual.Descripcion))
                        foreach (Venta vActual in listaVentas)
                            if (vActual.Producto.Equals(pvActual.Descripcion))
                                vActual.realizarVenta(pvActual.Cantidad * -1);
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
                    if (rActual.fecha == DateTime.Today.ToString().Substring(0, 10))
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
                    if (rActual.cliente.Contains(texto))
                    {
                        ListaFiltrada.Add(rActual);
                    }
                }
                if (x == 'f')
                {
                    if (rActual.fecha.Contains(texto))
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
            if (this.IntervaloCorrecto())
            {
                listaRegistro.RemoveAll(x => SeEncuentraEnIntervalo(x.id));
                GuardarRegistros();
                LimpiarPantalla();
                bActualizar.PerformClick();
            }
        }
    }
}
