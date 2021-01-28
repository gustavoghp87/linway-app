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

        public bool ExportarAExcel(List<DiasReparto> diasDeReparto)
        {
            try
            {
                MessageBox.Show(diasDeReparto[0].Dia + " - " + diasDeReparto[0].Reparto[0].Nombre + " - " + diasDeReparto[0].Reparto[0].Destinos[0].Direccion);
                return true;
                //hoja_trabajo.Cells[1, 9] = "REPARTOS";
                //hoja_trabajo.Cells[1, 9].Font.Bold = true;
                //hoja_trabajo.Cells[1, 9].Font.Underline = true;
                //hoja_trabajo.Cells[1, 9].Font.Size = 11;

                //hoja_trabajo.Cells[1, 3] = "DIA " + data[1] + " -  RECORRIDO " + data[2];
                //hoja_trabajo.Cells[1, 3].Font.Bold = true;
                //hoja_trabajo.Cells[1, 3].Font.Size = 11;
                
                //hoja_trabajo.Cells[2, 1] = "Dirección";
                //hoja_trabajo.Cells[2, 2] = "L";
                //hoja_trabajo.Cells[2, 3] = "Productos";
                //hoja_trabajo.Cells[2, 4] = "A";
                //hoja_trabajo.Cells[2, 5] = "E";
                //hoja_trabajo.Cells[2, 6] = "D";
                //hoja_trabajo.Cells[2, 7] = "T";
                //hoja_trabajo.Cells[2, 8] = "AE";

                //hoja_trabajo.Cells[listaRepartos.Count + 2, 9] = data[0];
                //hoja_trabajo.Cells[listaRepartos.Count + 2, 9].Font.Bold = true;
                //hoja_trabajo.Cells[listaRepartos.Count + 2, 9].EntireColumn.AutoFit();
                
                //hoja_trabajo.Cells[listaRepartos.Count + 4, 4] = "A";
                //hoja_trabajo.Cells[listaRepartos.Count + 4, 5] = "E";
                //hoja_trabajo.Cells[listaRepartos.Count + 4, 6] = "D";
                //hoja_trabajo.Cells[listaRepartos.Count + 4, 7] = "T";
                //hoja_trabajo.Cells[listaRepartos.Count + 4, 8] = "AE";

                //hoja_trabajo.Cells[listaRepartos.Count + 3, 1] = "TOTALES:";
                //hoja_trabajo.Cells[listaRepartos.Count + 3, 2] = data[3];
                //hoja_trabajo.Cells[listaRepartos.Count + 4, 3] = " TOTAL PESO: " + data[4] + data[5] + "        Total bolsas: " + data[6];
                //hoja_trabajo.Cells[listaRepartos.Count + 3, 4] = data[7];
                //hoja_trabajo.Cells[listaRepartos.Count + 3, 5] = data[8];
                //hoja_trabajo.Cells[listaRepartos.Count + 3, 6] = data[9];
                //hoja_trabajo.Cells[listaRepartos.Count + 3, 7] = data[10];
                //hoja_trabajo.Cells[listaRepartos.Count + 3, 8] = data[11];

                //int i = 0;
                //MessageBox.Show("Hay " + listaRepartos.Count + "repartos");
                //while (i < listaRepartos.Count)
                //{
                //    int j = 0;
                //    while (j < listaRepartos[i].Destinos.Count)
                //    {
                //        hoja_trabajo.Cells[i + j + 2, 1] = listaRepartos[i].Destinos[j].Direccion;
                //        hoja_trabajo.Cells[i + 2, 2] = listaRepartos[i].Destinos[j].L;
                //        hoja_trabajo.Cells[i + 2, 3] = listaRepartos[i].Destinos[j].Productos;
                //        hoja_trabajo.Cells[i + 2, 4] = listaRepartos[i].Destinos[j].A;
                //        hoja_trabajo.Cells[i + 2, 5] = listaRepartos[i].Destinos[j].E;
                //        hoja_trabajo.Cells[i + 2, 6] = listaRepartos[i].Destinos[j].D;
                //        hoja_trabajo.Cells[i + 2, 7] = listaRepartos[i].Destinos[j].T;
                //        hoja_trabajo.Cells[i + 2, 8] = listaRepartos[i].Destinos[j].AE;
                //        j++;
                //    }
                //    //data.Add((listaRepartos.Find(x => x.Nombre == comboBox2.Text).TotalB * 20).ToString());
                //    //data.Add((int.Parse(label22.Text)).ToString());
                //    i++;
                //}

                //excelCellrange = hoja_trabajo.Range[hoja_trabajo.Cells[1, 1], hoja_trabajo.Cells[listaRepartos.Count + 1, 8]];
                //return GenerateFile("repartos.xlsx");
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
                //hoja_trabajo.Cells[1, 9] = "REPARTOS";
                //hoja_trabajo.Cells[1, 9].Font.Bold = true;
                //hoja_trabajo.Cells[1, 9].Font.Underline = true;
                //hoja_trabajo.Cells[1, 9].Font.Size = 11;
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

                int i = 0;
                int sumaA = 0;
                int sumaE = 0;
                int sumaD = 0;
                int sumaT = 0;
                int sumaAE = 0;
                while (i < reparto.Destinos.Count)
                {
                    hoja_trabajo.Cells[i + 3, 1] = reparto.Destinos[i].Direccion;
                    hoja_trabajo.Cells[i + 3, 2] = reparto.Destinos[i].L;
                    hoja_trabajo.Cells[i + 3, 3] = reparto.Destinos[i].Productos;
                    hoja_trabajo.Cells[i + 3, 4] = reparto.Destinos[i].A;
                    hoja_trabajo.Cells[i + 3, 5] = reparto.Destinos[i].E;
                    hoja_trabajo.Cells[i + 3, 6] = reparto.Destinos[i].D;
                    hoja_trabajo.Cells[i + 3, 7] = reparto.Destinos[i].T;
                    hoja_trabajo.Cells[i + 3, 8] = reparto.Destinos[i].AE;
                    sumaA += reparto.Destinos[i].A;
                    sumaE += reparto.Destinos[i].E;
                    sumaD += reparto.Destinos[i].D;
                    sumaT += reparto.Destinos[i].T;
                    sumaAE += reparto.Destinos[i].AE;
                    i++;
                }

                hoja_trabajo.Cells[reparto.Destinos.Count + 3, 1] = "TOTALES:";
                hoja_trabajo.Cells[reparto.Destinos.Count + 3, 2] = litros;
                hoja_trabajo.Cells[reparto.Destinos.Count + 3, 1].Font.Bold = true;
                hoja_trabajo.Cells[reparto.Destinos.Count + 3, 2].Font.Bold = true;

                hoja_trabajo.Cells[reparto.Destinos.Count + 3, 4] = sumaA;
                hoja_trabajo.Cells[reparto.Destinos.Count + 3, 5] = sumaE;
                hoja_trabajo.Cells[reparto.Destinos.Count + 3, 6] = sumaD;
                hoja_trabajo.Cells[reparto.Destinos.Count + 3, 7] = sumaT;
                hoja_trabajo.Cells[reparto.Destinos.Count + 3, 8] = sumaAE;

                string pesoTotal = (Int32.Parse(litros) + Int32.Parse(bolsas)).ToString();
                hoja_trabajo.Cells[reparto.Destinos.Count + 4, 3] = " TOTAL PESO: " + pesoTotal + "    Total bolsas: " + bolsas;
                hoja_trabajo.Cells[reparto.Destinos.Count + 4, 4] = "A";
                hoja_trabajo.Cells[reparto.Destinos.Count + 4, 5] = "E";
                hoja_trabajo.Cells[reparto.Destinos.Count + 4, 6] = "D";
                hoja_trabajo.Cells[reparto.Destinos.Count + 4, 7] = "T";
                hoja_trabajo.Cells[reparto.Destinos.Count + 4, 8] = "AE";
                hoja_trabajo.Cells[reparto.Destinos.Count + 4, 3].Font.Bold = true;
                hoja_trabajo.Cells[reparto.Destinos.Count + 4, 4].Font.Bold = true;
                hoja_trabajo.Cells[reparto.Destinos.Count + 4, 5].Font.Bold = true;
                hoja_trabajo.Cells[reparto.Destinos.Count + 4, 6].Font.Bold = true;
                hoja_trabajo.Cells[reparto.Destinos.Count + 4, 7].Font.Bold = true;
                hoja_trabajo.Cells[reparto.Destinos.Count + 4, 8].Font.Bold = true;


                excelCellrange = hoja_trabajo.Range[hoja_trabajo.Cells[1, 1], hoja_trabajo.Cells[reparto.Destinos.Count + 4, 8]];
                return GenerateFile("reparto-" + dia + "-" + reparto.Nombre +".xlsx");
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
                libros_trabajo.SaveAs(Directory.GetCurrentDirectory().ToString() + "/Repartos/" + archivo,
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





//        for (int i = 0; i < grd.Rows.Count; i++)
//        {
//            for (int j = 0; j < grd.Columns.Count; j++)
//            {
//                hoja_trabajo.Cells[i + 3, j + 1] = grd.Rows[i].Cells[j].Value.ToString();
//            }
//        }

//        for (int i = 0; i < dt.Rows.Count; i++)
//        {
//            for (int j = 0; j < dt.Columns.Count; j++)
//            {
//                hoja_trabajo.Cells[i + 2, j + 1] = dt.Rows[i].ItemArray[j].ToString();
//            }
//        }
