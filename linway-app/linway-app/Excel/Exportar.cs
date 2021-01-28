using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing;
using System.IO;


namespace linway_app
{
    class Exportar
    {
        readonly Microsoft.Office.Interop.Excel.Application aplicacion;
        readonly Microsoft.Office.Interop.Excel.Workbook libros_trabajo;
        readonly Microsoft.Office.Interop.Excel.Worksheet hoja_trabajo;
        Microsoft.Office.Interop.Excel.Range excelCellrange;

        public Exportar() {
            aplicacion = new Microsoft.Office.Interop.Excel.Application();
            libros_trabajo = aplicacion.Workbooks.Add();
            hoja_trabajo = (Microsoft.Office.Interop.Excel.Worksheet)libros_trabajo.Worksheets.get_Item(1);
        }

        //public bool ExportarAExcel(DataGridView grd, char tipo)
        //{
        //    SaveFileDialog fichero = new SaveFileDialog();
        //    fichero.Filter = "Excel (*.xls)|*.xls";
        //    if (fichero.ShowDialog() == DialogResult.OK)
        //    {
        //        Microsoft.Office.Interop.Excel.Application aplicacion;
        //        Microsoft.Office.Interop.Excel.Workbook libros_trabajo;
        //        Microsoft.Office.Interop.Excel.Worksheet hoja_trabajo;
        //        Microsoft.Office.Interop.Excel.Range excelCellrange;
        //        aplicacion = new Microsoft.Office.Interop.Excel.Application();
        //        libros_trabajo = aplicacion.Workbooks.Add();
        //        hoja_trabajo = (Microsoft.Office.Interop.Excel.Worksheet) libros_trabajo.Worksheets.get_Item(1);
        //        //Recorremos el DataGridView rellenando la hoja de trabajo
        //        for (int i = 0; i < grd.Rows.Count; i++)
        //        {
        //            for (int j = 0; j < grd.Columns.Count; j++)
        //            {
        //                hoja_trabajo.Cells[i + 3, j + 1] = grd.Rows[i].Cells[j].Value.ToString();
        //            }
        //        }

        //        if (tipo == 'p')
        //        {
        //            hoja_trabajo.Cells[1, 2] = "PRODUCTOS";
        //            hoja_trabajo.Cells[1, 2].Font.Bold = true;
        //            hoja_trabajo.Cells[1, 2].Font.Underline = true;
        //            hoja_trabajo.Cells[1, 2].Font.Size = 11;
        //            hoja_trabajo.Cells[2, 1] = "Codigo";
        //            hoja_trabajo.Cells[2, 2] = "Producto";
        //            hoja_trabajo.Cells[2, 3] = "p/Unidad";
        //        }

        //        if (tipo == 'c')
        //        {
        //            hoja_trabajo.Cells[1, 1] = "Clientes";
        //            hoja_trabajo.Cells[1, 1].Font.Bold = true;
        //            hoja_trabajo.Cells[1, 1].Font.Underline = true;
        //            hoja_trabajo.Cells[1, 1].Font.Size = 11;
        //            hoja_trabajo.Cells[2, 1] = "Codigo";
        //            hoja_trabajo.Cells[2, 2] = "Direccion - Localidad";
        //            hoja_trabajo.Cells[2, 3] = "CP";
        //            hoja_trabajo.Cells[2, 4] = "Telefono";
        //            hoja_trabajo.Cells[2, 5] = "Nombre y Apellido";
        //            hoja_trabajo.Cells[2, 6] = "CUIT";
        //            hoja_trabajo.Cells[2, 7] = "Tipo";
        //        }

        //        //Establecer rango de celdas
        //        excelCellrange = hoja_trabajo.Range[hoja_trabajo.Cells[2, 1], hoja_trabajo.Cells[grd.Rows.Count + 2, grd.Columns.Count]];
        //        //Autoestablecer ancho de columnas
        //        excelCellrange.EntireColumn.AutoFit();
        //        //rellenar bordes
        //        Microsoft.Office.Interop.Excel.Borders border = excelCellrange.Borders;
        //        border.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
        //        border.Weight = 2d;
        //        //guardar.
        //        libros_trabajo.SaveAs(fichero.FileName, Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookNormal);
        //        libros_trabajo.Close(true);
        //        aplicacion.Quit();

        //    }
        //    return true;
        //}

        //public bool ExportarAExcel(DataTable dt, char tipo)
        //{
        //    SaveFileDialog fichero = new SaveFileDialog();
        //    fichero.Filter = "Excel (*.xls)|*.xls";
        //    if (fichero.ShowDialog() == DialogResult.OK)
        //    {
        //        Microsoft.Office.Interop.Excel.Application aplicacion;
        //        Microsoft.Office.Interop.Excel.Workbook libros_trabajo;
        //        Microsoft.Office.Interop.Excel.Worksheet hoja_trabajo;
        //        Microsoft.Office.Interop.Excel.Range excelCellrange;
        //        aplicacion = new Microsoft.Office.Interop.Excel.Application();
        //        libros_trabajo = aplicacion.Workbooks.Add();
        //        hoja_trabajo = (Microsoft.Office.Interop.Excel.Worksheet) libros_trabajo.Worksheets.get_Item(1);
        //        //Recorremos el DataGridView rellenando la hoja de trabajo
        //        for (int i = 0; i < dt.Rows.Count; i++)
        //        {
        //            for (int j = 0; j < dt.Columns.Count; j++)
        //            {
        //                hoja_trabajo.Cells[i + 2, j + 1] = dt.Rows[i].ItemArray[j].ToString();
        //            }
        //        }

        //        if (tipo == 'p')
        //        {
        //            //hoja_trabajo.Cells[1, 2] = "PRODUCTOS";
        //            //hoja_trabajo.Cells[1, 2].Font.Bold = true;
        //            //hoja_trabajo.Cells[1, 2].Font.Underline = true;
        //            //hoja_trabajo.Cells[1, 2].Font.Size = 11;
        //            hoja_trabajo.Cells[1, 1] = "Codigo";
        //            hoja_trabajo.Cells[1, 2] = "Producto";
        //            hoja_trabajo.Cells[1, 3] = "p/Unidad";
        //        }

        //        if (tipo == 'c')
        //        {
        //            //hoja_trabajo.Cells[1, 1] = "Clientes";
        //            //hoja_trabajo.Cells[1, 1].Font.Bold = true;
        //            //hoja_trabajo.Cells[1, 1].Font.Underline = true;
        //            //hoja_trabajo.Cells[1, 1].Font.Size = 11;
        //            hoja_trabajo.Cells[1, 1] = "Codigo";
        //            hoja_trabajo.Cells[1, 2] = "Direccion - Localidad";
        //            hoja_trabajo.Cells[1, 3] = "CP";
        //            hoja_trabajo.Cells[1, 4] = "Telefono";
        //            hoja_trabajo.Cells[1, 5] = "Nombre y Apellido";
        //            hoja_trabajo.Cells[1, 6] = "CUIT";
        //            hoja_trabajo.Cells[1, 7] = "Tipo";
        //        }

        //        //Establecer rango de celdas
        //        excelCellrange = hoja_trabajo.Range[hoja_trabajo.Cells[1, 1], hoja_trabajo.Cells[dt.Rows.Count + 2, dt.Columns.Count]];
        //        //Autoestablecer ancho de columnas
        //        excelCellrange.EntireColumn.AutoFit();
        //        //rellenar bordes
        //        Microsoft.Office.Interop.Excel.Borders border = excelCellrange.Borders;
        //        border.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
        //        border.Weight = 2d;
        //        //guardar.
        //        libros_trabajo.SaveAs(fichero.FileName, Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookNormal);
        //        libros_trabajo.Close(true);
        //        aplicacion.Quit();
        //    }
        //    return true;
        //}

        public bool ExportarAExcel(List<Cliente> listaClientes)
        {
            try
            {
                hoja_trabajo.Cells[1, 9] = "Clientes";
                hoja_trabajo.Cells[1, 9].Font.Bold = true;
                hoja_trabajo.Cells[1, 9].Font.Underline = true;
                hoja_trabajo.Cells[1, 9].Font.Size = 11;
                hoja_trabajo.Cells[1, 1] = "Codigo";
                hoja_trabajo.Cells[1, 2] = "Direccion - Localidad";
                hoja_trabajo.Cells[1, 3] = "CP";
                hoja_trabajo.Cells[1, 4] = "Telefono";
                hoja_trabajo.Cells[1, 5] = "Nombre y Apellido";
                hoja_trabajo.Cells[1, 6] = "CUIT";
                hoja_trabajo.Cells[1, 7] = "Tipo";

                int i = 0;
                while (i < listaClientes.Count)
                {
                    hoja_trabajo.Cells[i + 2, 1] = listaClientes[i].Numero;
                    hoja_trabajo.Cells[i + 2, 2] = listaClientes[i].Direccion.ToString();
                    hoja_trabajo.Cells[i + 2, 3] = listaClientes[i].CodigoPostal.ToString();
                    hoja_trabajo.Cells[i + 2, 4] = listaClientes[i].Telefono.ToString();
                    hoja_trabajo.Cells[i + 2, 5] = listaClientes[i].Nombre.ToString();
                    hoja_trabajo.Cells[i + 2, 6] = listaClientes[i].CUIT.ToString();
                    hoja_trabajo.Cells[i + 2, 7] = listaClientes[i].Tipo.ToString();
                    i++;
                }
                excelCellrange = hoja_trabajo.Range[hoja_trabajo.Cells[1, 1], hoja_trabajo.Cells[listaClientes.Count + 1, 7]];
                return GenerateFile("clientes.xlsx");
            }
            catch (Exception e)
            {
                MessageBox.Show("Error exportando (1): " + e.Message);
                return false;
            }
        }

        public bool ExportarAExcel(List<Producto> listaProductos)
        {
            try
            {
                hoja_trabajo.Cells[1, 9] = "PRODUCTOS";
                hoja_trabajo.Cells[1, 9].Font.Bold = true;
                hoja_trabajo.Cells[1, 9].Font.Underline = true;
                hoja_trabajo.Cells[1, 9].Font.Size = 11;
                hoja_trabajo.Cells[1, 1] = "Codigo";
                hoja_trabajo.Cells[1, 2] = "Producto";
                hoja_trabajo.Cells[1, 3] = "por Unidad";

                int i = 0;
                while (i < listaProductos.Count)
                {
                    hoja_trabajo.Cells[i + 2, 1] = listaProductos[i].Codigo;
                    hoja_trabajo.Cells[i + 2, 2] = listaProductos[i].Nombre;
                    hoja_trabajo.Cells[i + 2, 3] = listaProductos[i].Precio;
                    i++;
                }
                excelCellrange = hoja_trabajo.Range[hoja_trabajo.Cells[1, 1], hoja_trabajo.Cells[listaProductos.Count + 1, 3]];
                return GenerateFile("productos.xlsx");
            }
            catch (Exception e)
            {
                MessageBox.Show("Error exportando (1): " + e.Message);
                return false;
            }
        }

        public bool ExportarAExcel(List<NotaDeEnvio> notasEnvio)
        {
            try
            {
                hoja_trabajo.Cells[1, 9] = "NOTAS DE ENVÍO";
                hoja_trabajo.Cells[1, 9].Font.Bold = true;
                hoja_trabajo.Cells[1, 9].Font.Underline = true;
                hoja_trabajo.Cells[1, 9].Font.Size = 11;
                hoja_trabajo.Cells[1, 1] = "Número";
                hoja_trabajo.Cells[1, 2] = "Fecha";
                hoja_trabajo.Cells[1, 3] = "Dirección";
                hoja_trabajo.Cells[1, 4] = "Detalle";
                hoja_trabajo.Cells[1, 5] = "Total";
                hoja_trabajo.Cells[1, 6] = "Impresa";

                int i = 0;
                while (i < notasEnvio.Count)
                {
                    hoja_trabajo.Cells[i + 2, 1] = notasEnvio[i].Codigo;
                    hoja_trabajo.Cells[i + 2, 2] = notasEnvio[i].Fecha;
                    hoja_trabajo.Cells[i + 2, 3] = notasEnvio[i].Cliente;
                    hoja_trabajo.Cells[i + 2, 4] = notasEnvio[i].Detalle;
                    hoja_trabajo.Cells[i + 2, 5] = notasEnvio[i].ImporteTotal;
                    if (notasEnvio[i].Impresa == true)
                        hoja_trabajo.Cells[i + 2, 6] = "SI";
                    else
                        hoja_trabajo.Cells[i + 2, 6] = "NO";
                    i++;
                }
                excelCellrange = hoja_trabajo.Range[hoja_trabajo.Cells[1, 1], hoja_trabajo.Cells[notasEnvio.Count + 1, 6]];
                return GenerateFile("notas.xlsx");
            }
            catch (Exception e)
            {
                MessageBox.Show("Error exportando (1): " + e.Message);
                return false;
            }
        }

        public bool ExportarAExcel(List<Venta> listaVentas)
        {
            try
            {
                hoja_trabajo.Cells[1, 9] = "VENTAS";
                hoja_trabajo.Cells[1, 9].Font.Bold = true;
                hoja_trabajo.Cells[1, 9].Font.Underline = true;
                hoja_trabajo.Cells[1, 9].Font.Size = 11;
                hoja_trabajo.Cells[1, 1] = "Producto";
                hoja_trabajo.Cells[1, 2] = "Cantidad";

                int i = 0;
                while (i < listaVentas.Count)
                {
                    hoja_trabajo.Cells[i + 2, 1] = listaVentas[i].Producto;
                    hoja_trabajo.Cells[i + 2, 2] = listaVentas[i].Cantidad;
                    i++;
                }

                excelCellrange = hoja_trabajo.Range[hoja_trabajo.Cells[1, 1], hoja_trabajo.Cells[listaVentas.Count + 1, 2]];
                return GenerateFile("ventas.xlsx");
            }
            catch (Exception e)
            {
                MessageBox.Show("Error exportando (1): " + e.Message);
                return false;
            }
        }

        public bool ExportarAExcel(List<RegistroVenta> listaRegistros)
        {
            try
            {
                hoja_trabajo.Cells[1, 9] = "REGISTRO DE VENTAS";
                hoja_trabajo.Cells[1, 9].Font.Bold = true;
                hoja_trabajo.Cells[1, 9].Font.Underline = true;
                hoja_trabajo.Cells[1, 9].Font.Size = 11;
                hoja_trabajo.Cells[1, 1] = "Código";
                hoja_trabajo.Cells[1, 2] = "Fecha";
                hoja_trabajo.Cells[1, 3] = "Cliente";

                int i = 0;
                while (i < listaRegistros.Count)
                {
                    hoja_trabajo.Cells[i + 2, 1] = listaRegistros[i].id;
                    hoja_trabajo.Cells[i + 2, 2] = listaRegistros[i].fecha;
                    hoja_trabajo.Cells[i + 2, 3] = listaRegistros[i].cliente;
                    i++;
                }

                excelCellrange = hoja_trabajo.Range[hoja_trabajo.Cells[1, 1], hoja_trabajo.Cells[listaRegistros.Count + 1, 3]];
                return GenerateFile("registroVentas.xlsx");
            }
            catch (Exception e)
            {
                MessageBox.Show("Error exportando (1): " + e.Message);
                return false;
            }
        }





        private bool GenerateFile(string archivo)
        {
            try
            {
                excelCellrange.EntireColumn.AutoFit();
                Microsoft.Office.Interop.Excel.Borders border = excelCellrange.Borders;
                border.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                border.Weight = 2d;
                aplicacion.DisplayAlerts = false;
                libros_trabajo.SaveAs(Directory.GetCurrentDirectory().ToString() + "/Copias de seguridad/" + archivo,
                    Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookNormal);
                libros_trabajo.Close(true);
                //MessageBox.Show("Exportación a Excel terminada.");
                aplicacion.Quit();
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show("Error exportando (2): " + e.Message);
                return false;
            }
        }
    }
}
