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
using System.Threading;


namespace WindowsFormsApplication1
{    
    public partial class Form1 : Form
    {
        List<Cliente> listaClientes = new List<Cliente>();
        List<Producto> listaProductos = new List<Producto>();
       
        public Form1()
        {
            InitializeComponent();
        }

       
        void Actualizar()
        {
            listaClientes = new FormClientes().cargarClientes();
            listaProductos = new FormProductos().cargarProductos();
            dataGridView1.DataSource = listaClientes.ToArray();
            dataGridView2.DataSource = listaProductos.ToArray();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Actualizar();
            dataGridView2.Columns[0].Width = 20;
            dataGridView2.Columns[2].Width = 35;
            dataGridView1.Columns[0].Width = 30;
            dataGridView1.Columns[1].Width = 200;
            dataGridView1.Columns[2].Width = 30;
            dataGridView1.Columns[3].Width = 60;
            dataGridView1.Columns[5].Width = 70;
            dataGridView1.Columns[6].Width = 65;
        }

        private void botonActualizar_Click(object sender, EventArgs e)
        {
            Actualizar();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
        }

        // MENUES .
        private void verClientesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormClientes fClientes = new FormClientes();
            fClientes.agregarClientes();
            fClientes.Show();
        }

        private void agregarProductoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormProductos fAgregar = new FormProductos();
            fAgregar.agregarProductos();
            fAgregar.Show();  
        }      

        private void modificarProductoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormProductos fModificar = new FormProductos();
            fModificar.modificarProductos();
            fModificar.Show();
        }

        private void modificarClienteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormClientes fModificar = new FormClientes();
            fModificar.modificarClientes();
            fModificar.Show();
        }

        private void crearNotaDeEnvíoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormCrearNota crearNotaEnvio = new FormCrearNota();
            crearNotaEnvio.cargarProductosYClientes(listaProductos, listaClientes);
            crearNotaEnvio.Show();
        }

        private void verNotasDeEnvíoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            formNotas ventanaNotas = new formNotas();
            ventanaNotas.recibirProductos(listaProductos);
            ventanaNotas.Show();
        }

        private void borrarProductoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormProductos fBorrar = new FormProductos();
            fBorrar.borrarProductos();
            fBorrar.Show();
        }

        private void borrarClienteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormClientes fBorrar = new FormClientes();
            fBorrar.borrarClientes();
            fBorrar.Show();
        }

        private void verHojasDeRepartoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            formReparto fHDR = new formReparto();
            fHDR.Show();
        }

        private void verRecibosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormRecibos recibos = new FormRecibos();
            recibos.cargarClientes(listaClientes);
            recibos.Show();
        }

        private void verVentasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormVentas fVentas = new FormVentas();
            fVentas.obtenerDatos(listaClientes, listaProductos);
            fVentas.Show();
        }

        //BUSCAR CLIENTE
        void FiltrarDatosC(string texto)
        {
            List<Cliente> ListaFiltradaC = new List<Cliente>();
            Cliente[] ArrayC = new Cliente[listaClientes.Count];
            listaClientes.CopyTo(ArrayC);
            for (int i = 0; i < listaClientes.Count; i++)
            {
                string Actual = ArrayC[i].Direccion;
                if (Actual.Contains(texto))
                {
                    ListaFiltradaC.Add(ArrayC[i]);
                }
            }
            dataGridView1.DataSource = ListaFiltradaC.ToArray();
        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {
            FiltrarDatosC(BuscadorClientes.Text);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (BuscadorClientes.Visible)
            {
                button5.Text = ">>";
                label11.Visible = false;
                BuscadorClientes.Visible = false;
                BuscadorClientes.Text = "";
            }
            else
            {
                button5.Text = "<<";
                label11.Visible = true;
                BuscadorClientes.Visible = true;
                BuscadorClientes.Text = "";
            }
        }

        //BUSCAR PRODUCTO
        void FiltrarDatosP(string texto)
        {
            List<Producto> ListaFiltradaP = new List<Producto>();
            Producto[] ArrayP = new Producto[listaProductos.Count];
            listaProductos.CopyTo(ArrayP);
            for (int i = 0; i < listaProductos.Count; i++)
            {
                string Actual = ArrayP[i].Nombre;
                if (Actual.Contains(texto))
                {
                    ListaFiltradaP.Add(ArrayP[i]);
                }
            }
            dataGridView2.DataSource = ListaFiltradaP.ToArray();
        }
        private void button10_Click(object sender, EventArgs e)
        {
            if (BuscadorProductos.Visible)
            {
                button10.Text = ">>";
                label26.Visible = false;
                BuscadorProductos.Visible = false;
                BuscadorProductos.Text = "";
            }
            else
            {
                button10.Text = "<<";
                label26.Visible = true;
                BuscadorProductos.Visible = true;
                BuscadorClientes.Text = "";
            }
        }
        private void BuscadorProductos_TextChanged(object sender, EventArgs e)
        {
            FiltrarDatosP(BuscadorProductos.Text);
        }

      
        //////////////////////////// EXPORTAR ///////////////////////////////////////
        private void exportarProductosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExportarDataGridViewExcel(dataGridView2,'p');
        }

        private void exToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExportarDataGridViewExcel(dataGridView1, 'c');
        }

        private void ExportarDataGridViewExcel(DataGridView grd, char laLista)
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
                for (int i = 0; i < grd.Rows.Count ; i++)
                {
                    for (int j = 0; j < grd.Columns.Count; j++)
                    {
                        hoja_trabajo.Cells[i + 3, j + 1] = grd.Rows[i].Cells[j].Value.ToString();
                    }
                }
              
                //cabezera
                if (laLista == 'p')
                {
                    hoja_trabajo.Cells[1, 2] = "PRODUCTOS";
                    hoja_trabajo.Cells[1, 2].Font.Bold = true;
                    hoja_trabajo.Cells[1, 2].Font.Underline = true;
                    hoja_trabajo.Cells[1, 2].Font.Size = 11;
                    hoja_trabajo.Cells[2, 1] = "Codigo";
                    hoja_trabajo.Cells[2, 2] = "Producto";
                    hoja_trabajo.Cells[2, 3] = "p/Unidad";
                }
                if (laLista == 'c')
                {
                    hoja_trabajo.Cells[1, 1] = "Clientes";
                    hoja_trabajo.Cells[1, 1].Font.Bold = true;
                    hoja_trabajo.Cells[1, 1].Font.Underline = true;
                    hoja_trabajo.Cells[1, 1].Font.Size = 11;
                    hoja_trabajo.Cells[2, 1] = "Codigo";
                    hoja_trabajo.Cells[2, 2] = "Direccion - Localidad";
                    hoja_trabajo.Cells[2, 3] = "CP";
                    hoja_trabajo.Cells[2, 4] = "Telefono";
                    hoja_trabajo.Cells[2, 5] = "Nombre y Apellido";
                    hoja_trabajo.Cells[2, 6] = "CUIT";
                    hoja_trabajo.Cells[2, 7] = "Tipo";
                }
                //Establecer rango de celdas
                excelCellrange = hoja_trabajo.Range[hoja_trabajo.Cells[2, 1], hoja_trabajo.Cells[grd.Rows.Count+2, grd.Columns.Count]];
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

    
        // _________________________IMPORTAR____________________
        private void importarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new Importar().importarExcel(dataGridView1, "ORDEN DEL SISTEMA LINWAY");
            reemplazarImportadosALista(dataGridView1);
        }

        private void reemplazarImportadosALista(DataGridView grd)
        {
            listaClientes.Clear();
            for (int i = 0; i < grd.Rows.Count-1; i++)
            {
                int Numero = Int32.Parse(grd.Rows[i].Cells[0].Value.ToString());
                string Direccion = grd.Rows[i].Cells[1].Value.ToString();
                int CodigoPostal = Int32.Parse(grd.Rows[i].Cells[2].Value.ToString());
                int Telefono = Int32.Parse(grd.Rows[i].Cells[3].Value.ToString());
                string Nombre = grd.Rows[i].Cells[4].Value.ToString();
                string CUIT = grd.Rows[i].Cells[5].Value.ToString();
                TipoR Tipo;

                if (grd.Rows[i].Cells[6].Value.ToString() == "Inscripto")
                {
                    Tipo = TipoR.Inscripto;
                }
                else
                {
                    Tipo = TipoR.Monotributo;
                }
             
                Cliente nuevoCliente = new Cliente(Numero,Direccion,CodigoPostal,Telefono,Nombre,CUIT,Tipo);
                listaClientes.Add(nuevoCliente);
            }
            //GuardarClientes();
            Actualizar();
        }

    }
}
