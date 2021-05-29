using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using linway_app.Models;

namespace linway_app.Excel
{
    class Exportar
    {
        readonly string extension = "xls";
        //readonly string extension = "xlsx";
        readonly Microsoft.Office.Interop.Excel.Application aplicacion;
        readonly Microsoft.Office.Interop.Excel.Workbook libros_trabajo;
        readonly Microsoft.Office.Interop.Excel.Worksheet hoja_trabajo;
        Microsoft.Office.Interop.Excel.Range excelCellrange;

        public Exportar()
        {
            aplicacion = new Microsoft.Office.Interop.Excel.Application();
            libros_trabajo = aplicacion.Workbooks.Add();
            hoja_trabajo = (Microsoft.Office.Interop.Excel.Worksheet)libros_trabajo.Worksheets.get_Item(1);
        }

        private bool GenerateFile(string path)
        {
            try
            {
                excelCellrange.EntireColumn.AutoFit();
                Microsoft.Office.Interop.Excel.Borders border = excelCellrange.Borders;
                border.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                border.Weight = 2d;
                aplicacion.DisplayAlerts = false;
                libros_trabajo.SaveAs(Directory.GetCurrentDirectory().ToString() + path, Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookNormal);
                libros_trabajo.Close(true);
                aplicacion.Quit();
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show("Error exportando (2): " + e.Message + " Ver si el archivo está abierto...");
                return false;
            }
        }

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
                    hoja_trabajo.Cells[i + 2, 1] = listaClientes[i].Id;
                    hoja_trabajo.Cells[i + 2, 2] = listaClientes[i].Direccion.ToString();
                    hoja_trabajo.Cells[i + 2, 3] = listaClientes[i].CodigoPostal.ToString();
                    hoja_trabajo.Cells[i + 2, 4] = listaClientes[i].Telefono.ToString();
                    hoja_trabajo.Cells[i + 2, 5] = listaClientes[i].Name.ToString();
                    hoja_trabajo.Cells[i + 2, 6] = listaClientes[i].Cuit.ToString();
                    hoja_trabajo.Cells[i + 2, 7] = listaClientes[i].Tipo.ToString();
                    i++;
                }
                excelCellrange = hoja_trabajo.Range[hoja_trabajo.Cells[1, 1], hoja_trabajo.Cells[listaClientes.Count + 1, 7]];
                return GenerateFile("/Copias de seguridad/clientes." + extension);
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
                    hoja_trabajo.Cells[i + 2, 1] = listaProductos[i].Id;
                    hoja_trabajo.Cells[i + 2, 2] = listaProductos[i].Nombre;
                    hoja_trabajo.Cells[i + 2, 3] = listaProductos[i].Precio;
                    i++;
                }
                excelCellrange = hoja_trabajo.Range[hoja_trabajo.Cells[1, 1], hoja_trabajo.Cells[listaProductos.Count + 1, 3]];
                return GenerateFile("/Copias de seguridad/productos." + extension);
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
                    hoja_trabajo.Cells[i + 2, 1] = notasEnvio[i].Id;
                    hoja_trabajo.Cells[i + 2, 2] = notasEnvio[i].Fecha;
                    //hoja_trabajo.Cells[i + 2, 3] = notasEnvio[i].Cliente;
                    //foreach (var producto in notasEnvio[i].ProductosVendidos)
                    //{
                    //    notasEnvio[i].Detalle += " &" + producto.Precio.ToString();
                    //}
                    hoja_trabajo.Cells[i + 2, 4] = notasEnvio[i].Detalle;
                    hoja_trabajo.Cells[i + 2, 5] = notasEnvio[i].ImporteTotal;
                    //if (notasEnvio[i].Impresa == true)
                    //    hoja_trabajo.Cells[i + 2, 6] = "SI";
                    //else
                    //    hoja_trabajo.Cells[i + 2, 6] = "NO";
                    i++;
                }
                excelCellrange = hoja_trabajo.Range[hoja_trabajo.Cells[1, 1], hoja_trabajo.Cells[notasEnvio.Count + 1, 6]];
                return GenerateFile("/Copias de seguridad/notas." + extension);
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
                    //* hoja_trabajo.Cells[i + 2, 1] = listaVentas[i].Producto;
                    hoja_trabajo.Cells[i + 2, 2] = listaVentas[i].Cantidad;
                    i++;
                }

                excelCellrange = hoja_trabajo.Range[hoja_trabajo.Cells[1, 1], hoja_trabajo.Cells[listaVentas.Count + 1, 2]];
                return GenerateFile("/Copias de seguridad/ventas." + extension);
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
                    hoja_trabajo.Cells[i + 2, 1] = listaRegistros[i].Id;
                    hoja_trabajo.Cells[i + 2, 2] = listaRegistros[i].Fecha;
                    hoja_trabajo.Cells[i + 2, 3] = listaRegistros[i].NombreCliente;
                    i++;
                }

                excelCellrange = hoja_trabajo.Range[hoja_trabajo.Cells[1, 1], hoja_trabajo.Cells[listaRegistros.Count + 1, 3]];
                return GenerateFile("/Copias de seguridad/registroVentas." + extension);
            }
            catch (Exception e)
            {
                MessageBox.Show("Error exportando (1): " + e.Message);
                return false;
            }
        }

        public bool ExportarAExcel(List<DiaReparto> diasDeReparto)
        {
            try
            {
                //hoja_trabajo.Cells[1, 11] = "REPARTOS";
                //hoja_trabajo.Cells[1, 11].Font.Bold = true;
                //hoja_trabajo.Cells[1, 11].Font.Underline = true;
                //hoja_trabajo.Cells[1, 11].Font.Size = 11;

                hoja_trabajo.Cells[1, 1] = "DIA";
                hoja_trabajo.Cells[1, 1].Font.Bold = true;
                hoja_trabajo.Cells[1, 1].Font.Size = 11;

                hoja_trabajo.Cells[1, 2] = "NOMBRE";
                hoja_trabajo.Cells[1, 2].Font.Bold = true;
                hoja_trabajo.Cells[1, 2].Font.Size = 11;

                hoja_trabajo.Cells[1, 3] = "DIRECCION";
                hoja_trabajo.Cells[1, 3].Font.Bold = true;
                hoja_trabajo.Cells[1, 3].Font.Size = 11;

                hoja_trabajo.Cells[1, 4] = "PRODUCTOS";
                hoja_trabajo.Cells[1, 4].Font.Bold = true;
                hoja_trabajo.Cells[1, 4].Font.Size = 11;

                hoja_trabajo.Cells[1, 5] = "ENTREGAR";
                hoja_trabajo.Cells[1, 5].Font.Bold = true;
                hoja_trabajo.Cells[1, 5].Font.Size = 11;

                hoja_trabajo.Cells[1, 6] = "LITROS";
                hoja_trabajo.Cells[1, 6].Font.Bold = true;
                hoja_trabajo.Cells[1, 6].Font.Size = 11;

                hoja_trabajo.Cells[1, 7] = "A";
                hoja_trabajo.Cells[1, 7].Font.Bold = true;
                hoja_trabajo.Cells[1, 7].Font.Size = 11;

                hoja_trabajo.Cells[1, 8] = "E";
                hoja_trabajo.Cells[1, 8].Font.Bold = true;
                hoja_trabajo.Cells[1, 8].Font.Size = 11;

                hoja_trabajo.Cells[1, 9] = "D";
                hoja_trabajo.Cells[1, 9].Font.Bold = true;
                hoja_trabajo.Cells[1, 9].Font.Size = 11;

                hoja_trabajo.Cells[1, 10] = "T";
                hoja_trabajo.Cells[1, 10].Font.Bold = true;
                hoja_trabajo.Cells[1, 10].Font.Size = 11;

                hoja_trabajo.Cells[1, 11] = "AE";
                hoja_trabajo.Cells[1, 11].Font.Bold = true;
                hoja_trabajo.Cells[1, 11].Font.Size = 11;

                int i = 0;
                foreach (DiaReparto dia in diasDeReparto)
                {
                    //foreach (Reparto reparto in dia.Repartos)
                    //{
                    //    foreach (Destino destino in reparto.Destinos)
                    //    {
                    //        //MessageBox.Show(reparto.Nombre + " - " + reparto.TT + " - " + reparto.TA + " - " + reparto.TE + " - " + reparto.TD + " - " + reparto.TAE + " - " + reparto.TL + " - " + reparto.TotalB);
                    //        hoja_trabajo.Cells[2 + i, 1] = dia.Dia;
                    //        hoja_trabajo.Cells[2 + i, 1].Font.Bold = true;
                    //        hoja_trabajo.Cells[2 + i, 1].Font.Size = 11;
                    //        hoja_trabajo.Cells[2 + i, 2] = reparto.Nombre;
                    //        hoja_trabajo.Cells[2 + i, 2].Font.Bold = true;
                    //        hoja_trabajo.Cells[2 + i, 2].Font.Size = 11;
                    //        hoja_trabajo.Cells[2 + i, 3] = destino.Direccion;
                    //        hoja_trabajo.Cells[2 + i, 4] = destino.Productos;
                    //        if (destino.Entregar) hoja_trabajo.Cells[2 + i, 5] = "SI";
                    //        else hoja_trabajo.Cells[2 + i, 5] = "NO";
                    //        hoja_trabajo.Cells[2 + i, 6] = destino.L;
                    //        hoja_trabajo.Cells[2 + i, 7] = destino.A;
                    //        hoja_trabajo.Cells[2 + i, 8] = destino.E;
                    //        hoja_trabajo.Cells[2 + i, 9] = destino.D;
                    //        hoja_trabajo.Cells[2 + i, 10] = destino.T;
                    //        hoja_trabajo.Cells[2 + i, 11] = destino.AE;
                    //        i++;
                    //    }
                    //}
                }
                excelCellrange = hoja_trabajo.Range[hoja_trabajo.Cells[1, 1], hoja_trabajo.Cells[i + 1, 11]];
                return GenerateFile("/Copias de seguridad/repartos." + extension);
            }
            catch (Exception e)
            {
                MessageBox.Show("Error exportando (1): " + e.Message);
                return false;
            }
        }

        public bool ExportarAExcel(Reparto reparto, string dia, string litros, string bolsas)
        {
            try
            {
                hoja_trabajo.Cells[1, 3] = "DIA " + dia + " -  RECORRIDO " + reparto.Nombre;
                hoja_trabajo.Cells[1, 3].Font.Bold = true;
                hoja_trabajo.Cells[1, 3].Font.Size = 11;
                hoja_trabajo.Cells[2, 1] = "Dirección";
                hoja_trabajo.Cells[2, 2] = "L";
                hoja_trabajo.Cells[2, 3] = "Productos";
                hoja_trabajo.Cells[2, 4] = "A";
                hoja_trabajo.Cells[2, 5] = "E";
                hoja_trabajo.Cells[2, 6] = "D";
                hoja_trabajo.Cells[2, 7] = "T";
                hoja_trabajo.Cells[2, 8] = "AE";
                hoja_trabajo.Cells[2, 1].Font.Bold = true;
                hoja_trabajo.Cells[2, 2].Font.Bold = true;
                hoja_trabajo.Cells[2, 3].Font.Bold = true;
                hoja_trabajo.Cells[2, 4].Font.Bold = true;
                hoja_trabajo.Cells[2, 5].Font.Bold = true;
                hoja_trabajo.Cells[2, 6].Font.Bold = true;
                hoja_trabajo.Cells[2, 7].Font.Bold = true;
                hoja_trabajo.Cells[2, 8].Font.Bold = true;

                //var paraImprimir = new List<Destino>();
                //foreach (var destino in reparto.Destinos)
                //{
                //    if (destino.Productos.Trim() != "") paraImprimir.Add(destino);
                //}
                int sumaA = 0;
                int sumaE = 0;
                int sumaD = 0;
                int sumaT = 0;
                int sumaAE = 0;
                int i = 0;
                //foreach (var destino in paraImprimir)
                //{
                //    hoja_trabajo.Cells[i + 3, 1] = destino.Direccion;
                //    hoja_trabajo.Cells[i + 3, 2] = destino.L;
                //    hoja_trabajo.Cells[i + 3, 3] = destino.Productos;
                //    hoja_trabajo.Cells[i + 3, 4] = destino.A;
                //    hoja_trabajo.Cells[i + 3, 5] = destino.E;
                //    hoja_trabajo.Cells[i + 3, 6] = destino.D;
                //    hoja_trabajo.Cells[i + 3, 7] = destino.T;
                //    hoja_trabajo.Cells[i + 3, 8] = destino.AE;
                //    sumaA += destino.A;
                //    sumaE += destino.E;
                //    sumaD += destino.D;
                //    sumaT += destino.T;
                //    sumaAE += destino.AE;
                //    i++;
                //}

                hoja_trabajo.Cells[i + 4, 1] = "TOTALES:";
                hoja_trabajo.Cells[i + 4, 2] = litros;
                hoja_trabajo.Cells[i + 4, 1].Font.Bold = true;
                hoja_trabajo.Cells[i + 4, 2].Font.Bold = true;

                hoja_trabajo.Cells[i + 4, 4] = sumaA;
                hoja_trabajo.Cells[i + 4, 5] = sumaE;
                hoja_trabajo.Cells[i + 4, 6] = sumaD;
                hoja_trabajo.Cells[i + 4, 7] = sumaT;
                hoja_trabajo.Cells[i + 4, 8] = sumaAE;

                string pesoTotal = (Int32.Parse(litros) + Int32.Parse(bolsas)).ToString();
                hoja_trabajo.Cells[i + 5, 3] = " TOTAL PESO: " + pesoTotal + "    Total bolsas: " + bolsas;
                hoja_trabajo.Cells[i + 5, 4] = "A";
                hoja_trabajo.Cells[i + 5, 5] = "E";
                hoja_trabajo.Cells[i + 5, 6] = "D";
                hoja_trabajo.Cells[i + 5, 7] = "T";
                hoja_trabajo.Cells[i + 5, 8] = "AE";
                hoja_trabajo.Cells[i + 5, 3].Font.Bold = true;
                hoja_trabajo.Cells[i + 5, 4].Font.Bold = true;
                hoja_trabajo.Cells[i + 5, 5].Font.Bold = true;
                hoja_trabajo.Cells[i + 5, 6].Font.Bold = true;
                hoja_trabajo.Cells[i + 5, 7].Font.Bold = true;
                hoja_trabajo.Cells[i + 5, 8].Font.Bold = true;

                excelCellrange = hoja_trabajo.Range[hoja_trabajo.Cells[1, 1], hoja_trabajo.Cells[i + 5, 8]];
                return GenerateFile("/Repartos/reparto-" + dia + "-" + reparto.Nombre + "." + extension);
            }
            catch (Exception e)
            {
                MessageBox.Show("Error exportando (1): " + e.Message);
                return false;
            }
        }

        public bool ExportarAExcel(List<Recibo> listaRecibos)
        {
            try
            {
                hoja_trabajo.Cells[1, 1] = "Numero";
                hoja_trabajo.Cells[1, 1].Font.Bold = true;
                hoja_trabajo.Cells[1, 2] = "Fecha";
                hoja_trabajo.Cells[1, 2].Font.Bold = true;
                hoja_trabajo.Cells[1, 3] = "Direccion";
                hoja_trabajo.Cells[1, 3].Font.Bold = true;
                hoja_trabajo.Cells[1, 4] = "Total";
                hoja_trabajo.Cells[1, 4].Font.Bold = true;
                hoja_trabajo.Cells[1, 5] = "Impresa";
                hoja_trabajo.Cells[1, 5].Font.Bold = true;
                hoja_trabajo.Cells[1, 6] = "Detalles";
                hoja_trabajo.Cells[1, 6].Font.Bold = true;
                int i = 0;
                while (i < listaRecibos.Count)
                {
                    hoja_trabajo.Cells[i + 2, 1] = listaRecibos[i].Id;
                    hoja_trabajo.Cells[i + 2, 2] = listaRecibos[i].Fecha;
                    hoja_trabajo.Cells[i + 2, 3] = listaRecibos[i].DireccionCliente;
                    hoja_trabajo.Cells[i + 2, 4] = listaRecibos[i].ImporteTotal;
                    //if (listaRecibos[i].Impresa == true) hoja_trabajo.Cells[i + 2, 5] = "SI";
                    //else                                 hoja_trabajo.Cells[i + 2, 5] = "NO";

                    //string detalles = "";
                    //try
                    //{
                    //    foreach (DetalleRecibo detalle in listaRecibos[i].ListaDetalles)
                    //    {
                    //        if (detalle != null) detalles += detalle.Detalle + " por: " + detalle.Importe.ToString() + " | ";
                    //    }
                    //}
                    //catch (Exception e)
                    //{
                    //    MessageBox.Show(e.Message);
                    //}
                    //hoja_trabajo.Cells[i + 2, 6] = detalles;
                    i++;
                }
                excelCellrange = hoja_trabajo.Range[hoja_trabajo.Cells[1, 1], hoja_trabajo.Cells[listaRecibos.Count + 1, 5]];
                return GenerateFile("/Copias de seguridad/recibos." + extension);
            }
            catch (Exception e)
            {
                MessageBox.Show("Error exportando (1): " + e.Message);
                return false;
            }
        }
    }
}
