using linway_app.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace linway_app.Excel
{
    public class Exportar
    {
        private readonly string _extension = "xls";
        //readonly string extension = "xlsx";
        private readonly Microsoft.Office.Interop.Excel.Application _aplicacion;
        private readonly Microsoft.Office.Interop.Excel.Workbook _libros_trabajo;
        private readonly Microsoft.Office.Interop.Excel.Worksheet _hoja_trabajo;
        private Microsoft.Office.Interop.Excel.Range _excelCellrange;
        public Exportar()
        {
            try
            {
                _aplicacion = new Microsoft.Office.Interop.Excel.Application();
                _libros_trabajo = _aplicacion.Workbooks.Add();
                _hoja_trabajo = (Microsoft.Office.Interop.Excel.Worksheet)_libros_trabajo.Worksheets.get_Item(1);
            }
            catch (Exception e)
            {
                MessageBox.Show("Error al exportar a Excel: ", e.Message);
            }
        }
        private bool GenerateFile(string path)
        {
            try
            {
                _excelCellrange.EntireColumn.AutoFit();
                Microsoft.Office.Interop.Excel.Borders border = _excelCellrange.Borders;
                border.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                border.Weight = 2d;
                _aplicacion.DisplayAlerts = false;
                _libros_trabajo.SaveAs(Directory.GetCurrentDirectory().ToString() + path, Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookNormal);
                _libros_trabajo.Close(true);
                _aplicacion.Quit();
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
                _hoja_trabajo.Cells[1, 9] = "Clientes";
                _hoja_trabajo.Cells[1, 9].Font.Bold = true;
                _hoja_trabajo.Cells[1, 9].Font.Underline = true;
                _hoja_trabajo.Cells[1, 9].Font.Size = 11;
                _hoja_trabajo.Cells[1, 1] = "Codigo";
                _hoja_trabajo.Cells[1, 2] = "Direccion - Localidad";
                _hoja_trabajo.Cells[1, 3] = "CP";
                _hoja_trabajo.Cells[1, 4] = "Telefono";
                _hoja_trabajo.Cells[1, 5] = "Nombre y Apellido";
                _hoja_trabajo.Cells[1, 6] = "CUIT";
                _hoja_trabajo.Cells[1, 7] = "Tipo";

                int i = 0;
                while (i < listaClientes.Count)
                {
                    _hoja_trabajo.Cells[i + 2, 1] = listaClientes[i].Id;
                    _hoja_trabajo.Cells[i + 2, 2] = listaClientes[i].Direccion.ToString();
                    _hoja_trabajo.Cells[i + 2, 3] = listaClientes[i].CodigoPostal.ToString();
                    _hoja_trabajo.Cells[i + 2, 4] = listaClientes[i].Telefono.ToString();
                    _hoja_trabajo.Cells[i + 2, 5] = listaClientes[i].Nombre.ToString();
                    _hoja_trabajo.Cells[i + 2, 6] = listaClientes[i].Cuit.ToString();
                    _hoja_trabajo.Cells[i + 2, 7] = listaClientes[i].Tipo.ToString();
                    i++;
                }
                _excelCellrange = _hoja_trabajo.Range[_hoja_trabajo.Cells[1, 1], _hoja_trabajo.Cells[listaClientes.Count + 1, 7]];
                return GenerateFile("/Copias de seguridad/clientes." + _extension);
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
                _hoja_trabajo.Cells[1, 9] = "PRODUCTOS";
                _hoja_trabajo.Cells[1, 9].Font.Bold = true;
                _hoja_trabajo.Cells[1, 9].Font.Underline = true;
                _hoja_trabajo.Cells[1, 9].Font.Size = 11;
                _hoja_trabajo.Cells[1, 1] = "Codigo";
                _hoja_trabajo.Cells[1, 2] = "Producto";
                _hoja_trabajo.Cells[1, 3] = "por Unidad";

                int i = 0;
                while (i < listaProductos.Count)
                {
                    _hoja_trabajo.Cells[i + 2, 1] = listaProductos[i].Id;
                    _hoja_trabajo.Cells[i + 2, 2] = listaProductos[i].Nombre;
                    _hoja_trabajo.Cells[i + 2, 3] = listaProductos[i].Precio;
                    i++;
                }
                _excelCellrange = _hoja_trabajo.Range[_hoja_trabajo.Cells[1, 1], _hoja_trabajo.Cells[listaProductos.Count + 1, 3]];
                return GenerateFile("/Copias de seguridad/productos." + _extension);
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
                _hoja_trabajo.Cells[1, 9] = "NOTAS DE ENVÍO";
                _hoja_trabajo.Cells[1, 9].Font.Bold = true;
                _hoja_trabajo.Cells[1, 9].Font.Underline = true;
                _hoja_trabajo.Cells[1, 9].Font.Size = 11;
                _hoja_trabajo.Cells[1, 1] = "Número";
                _hoja_trabajo.Cells[1, 2] = "Fecha";
                _hoja_trabajo.Cells[1, 3] = "Dirección";
                _hoja_trabajo.Cells[1, 4] = "Detalle";
                _hoja_trabajo.Cells[1, 5] = "Total";
                _hoja_trabajo.Cells[1, 6] = "Impresa";

                int i = 0;
                while (i < notasEnvio.Count)
                {
                    _hoja_trabajo.Cells[i + 2, 1] = notasEnvio[i].Id;
                    _hoja_trabajo.Cells[i + 2, 2] = notasEnvio[i].Fecha;
                    //hoja_trabajo.Cells[i + 2, 3] = notasEnvio[i].Cliente;
                    //foreach (var producto in notasEnvio[i].ProductosVendidos)
                    //{
                    //    notasEnvio[i].Detalle += " &" + producto.Precio.ToString();
                    //}
                    _hoja_trabajo.Cells[i + 2, 4] = notasEnvio[i].Detalle;
                    _hoja_trabajo.Cells[i + 2, 5] = notasEnvio[i].ImporteTotal;
                    //if (notasEnvio[i].Impresa == true)
                    //    hoja_trabajo.Cells[i + 2, 6] = "SI";
                    //else
                    //    hoja_trabajo.Cells[i + 2, 6] = "NO";
                    i++;
                }
                _excelCellrange = _hoja_trabajo.Range[_hoja_trabajo.Cells[1, 1], _hoja_trabajo.Cells[notasEnvio.Count + 1, 6]];
                return GenerateFile("/Copias de seguridad/notas." + _extension);
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
                _hoja_trabajo.Cells[1, 9] = "VENTAS";
                _hoja_trabajo.Cells[1, 9].Font.Bold = true;
                _hoja_trabajo.Cells[1, 9].Font.Underline = true;
                _hoja_trabajo.Cells[1, 9].Font.Size = 11;
                _hoja_trabajo.Cells[1, 1] = "Producto";
                _hoja_trabajo.Cells[1, 2] = "Cantidad";

                int i = 0;
                while (i < listaVentas.Count)
                {
                    //* hoja_trabajo.Cells[i + 2, 1] = listaVentas[i].Producto;
                    _hoja_trabajo.Cells[i + 2, 2] = listaVentas[i].Cantidad;
                    i++;
                }

                _excelCellrange = _hoja_trabajo.Range[_hoja_trabajo.Cells[1, 1], _hoja_trabajo.Cells[listaVentas.Count + 1, 2]];
                return GenerateFile("/Copias de seguridad/ventas." + _extension);
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
                _hoja_trabajo.Cells[1, 9] = "REGISTRO DE VENTAS";
                _hoja_trabajo.Cells[1, 9].Font.Bold = true;
                _hoja_trabajo.Cells[1, 9].Font.Underline = true;
                _hoja_trabajo.Cells[1, 9].Font.Size = 11;
                _hoja_trabajo.Cells[1, 1] = "Código";
                _hoja_trabajo.Cells[1, 2] = "Fecha";
                _hoja_trabajo.Cells[1, 3] = "Cliente";

                int i = 0;
                while (i < listaRegistros.Count)
                {
                    _hoja_trabajo.Cells[i + 2, 1] = listaRegistros[i].Id;
                    _hoja_trabajo.Cells[i + 2, 2] = listaRegistros[i].Fecha;
                    _hoja_trabajo.Cells[i + 2, 3] = listaRegistros[i].NombreCliente;
                    i++;
                }

                _excelCellrange = _hoja_trabajo.Range[_hoja_trabajo.Cells[1, 1], _hoja_trabajo.Cells[listaRegistros.Count + 1, 3]];
                return GenerateFile("/Copias de seguridad/registroVentas." + _extension);
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

                _hoja_trabajo.Cells[1, 1] = "DIA";
                _hoja_trabajo.Cells[1, 1].Font.Bold = true;
                _hoja_trabajo.Cells[1, 1].Font.Size = 11;

                _hoja_trabajo.Cells[1, 2] = "NOMBRE";
                _hoja_trabajo.Cells[1, 2].Font.Bold = true;
                _hoja_trabajo.Cells[1, 2].Font.Size = 11;

                _hoja_trabajo.Cells[1, 3] = "DIRECCION";
                _hoja_trabajo.Cells[1, 3].Font.Bold = true;
                _hoja_trabajo.Cells[1, 3].Font.Size = 11;

                _hoja_trabajo.Cells[1, 4] = "PRODUCTOS";
                _hoja_trabajo.Cells[1, 4].Font.Bold = true;
                _hoja_trabajo.Cells[1, 4].Font.Size = 11;

                _hoja_trabajo.Cells[1, 5] = "ENTREGAR";
                _hoja_trabajo.Cells[1, 5].Font.Bold = true;
                _hoja_trabajo.Cells[1, 5].Font.Size = 11;

                _hoja_trabajo.Cells[1, 6] = "LITROS";
                _hoja_trabajo.Cells[1, 6].Font.Bold = true;
                _hoja_trabajo.Cells[1, 6].Font.Size = 11;

                _hoja_trabajo.Cells[1, 7] = "A";
                _hoja_trabajo.Cells[1, 7].Font.Bold = true;
                _hoja_trabajo.Cells[1, 7].Font.Size = 11;

                _hoja_trabajo.Cells[1, 8] = "E";
                _hoja_trabajo.Cells[1, 8].Font.Bold = true;
                _hoja_trabajo.Cells[1, 8].Font.Size = 11;

                _hoja_trabajo.Cells[1, 9] = "D";
                _hoja_trabajo.Cells[1, 9].Font.Bold = true;
                _hoja_trabajo.Cells[1, 9].Font.Size = 11;

                _hoja_trabajo.Cells[1, 10] = "T";
                _hoja_trabajo.Cells[1, 10].Font.Bold = true;
                _hoja_trabajo.Cells[1, 10].Font.Size = 11;

                _hoja_trabajo.Cells[1, 11] = "AE";
                _hoja_trabajo.Cells[1, 11].Font.Bold = true;
                _hoja_trabajo.Cells[1, 11].Font.Size = 11;

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
                _excelCellrange = _hoja_trabajo.Range[_hoja_trabajo.Cells[1, 1], _hoja_trabajo.Cells[i + 1, 11]];
                return GenerateFile("/Copias de seguridad/repartos." + _extension);
            }
            catch (Exception e)
            {
                MessageBox.Show("Error exportando (1): " + e.Message);
                return false;
            }
        }
        public bool ExportarAExcel(Reparto reparto)
        {
            try
            {
                _hoja_trabajo.Cells[1, 3] = "DIA " + reparto.DiaReparto.Dia + " -  RECORRIDO " + reparto.Nombre;
                _hoja_trabajo.Cells[1, 3].Font.Bold = true;
                _hoja_trabajo.Cells[1, 3].Font.Size = 11;
                _hoja_trabajo.Cells[2, 1] = "Dirección";
                _hoja_trabajo.Cells[2, 2] = "Litros";
                _hoja_trabajo.Cells[2, 3] = "Productos";
                _hoja_trabajo.Cells[2, 4] = "A";
                _hoja_trabajo.Cells[2, 5] = "E";
                _hoja_trabajo.Cells[2, 6] = "D";
                _hoja_trabajo.Cells[2, 7] = "T";
                _hoja_trabajo.Cells[2, 8] = "AE";
                _hoja_trabajo.Cells[2, 1].Font.Bold = true;
                _hoja_trabajo.Cells[2, 2].Font.Bold = true;
                _hoja_trabajo.Cells[2, 3].Font.Bold = true;
                _hoja_trabajo.Cells[2, 4].Font.Bold = true;
                _hoja_trabajo.Cells[2, 5].Font.Bold = true;
                _hoja_trabajo.Cells[2, 6].Font.Bold = true;
                _hoja_trabajo.Cells[2, 7].Font.Bold = true;
                _hoja_trabajo.Cells[2, 8].Font.Bold = true;

                List<Pedido> lstPedidosAEntregar = new List<Pedido>();
                long litros = 0;
                foreach (Pedido pedido in reparto.Pedidos)
                {
                    if (pedido.Entregar == 1) lstPedidosAEntregar.Add(pedido);
                    litros += pedido.L;
                }
                long cantidadBolsas = reparto.Ta + reparto.Te + reparto.Td + reparto.Tt + reparto.Tae;
                int i = 0;
                foreach (var pedido in lstPedidosAEntregar)
                {
                    _hoja_trabajo.Cells[i + 3, 1] = pedido.Direccion;
                    _hoja_trabajo.Cells[i + 3, 2] = pedido.L;
                    _hoja_trabajo.Cells[i + 3, 3] = pedido.ProductosText;
                    _hoja_trabajo.Cells[i + 3, 4] = pedido.A;
                    _hoja_trabajo.Cells[i + 3, 5] = pedido.E;
                    _hoja_trabajo.Cells[i + 3, 6] = pedido.D;
                    _hoja_trabajo.Cells[i + 3, 7] = pedido.T;
                    _hoja_trabajo.Cells[i + 3, 8] = pedido.Ae;
                    i++;
                }
                _hoja_trabajo.Cells[i + 4, 1] = "TOTALES:";
                _hoja_trabajo.Cells[i + 4, 2] = litros;
                _hoja_trabajo.Cells[i + 4, 1].Font.Bold = true;
                _hoja_trabajo.Cells[i + 4, 2].Font.Bold = true;
                _hoja_trabajo.Cells[i + 4, 4] = reparto.Ta;
                _hoja_trabajo.Cells[i + 4, 5] = reparto.Te;
                _hoja_trabajo.Cells[i + 4, 6] = reparto.Td;
                _hoja_trabajo.Cells[i + 4, 7] = reparto.Tt;
                _hoja_trabajo.Cells[i + 4, 8] = reparto.Tae;
                _hoja_trabajo.Cells[i + 5, 3] = " TOTAL PESO | Total litros: " + litros + ",    Total bolsas: " + cantidadBolsas.ToString();
                _hoja_trabajo.Cells[i + 5, 4] = "A";
                _hoja_trabajo.Cells[i + 5, 5] = "E";
                _hoja_trabajo.Cells[i + 5, 6] = "D";
                _hoja_trabajo.Cells[i + 5, 7] = "T";
                _hoja_trabajo.Cells[i + 5, 8] = "AE";
                _hoja_trabajo.Cells[i + 5, 3].Font.Bold = true;
                _hoja_trabajo.Cells[i + 5, 4].Font.Bold = true;
                _hoja_trabajo.Cells[i + 5, 5].Font.Bold = true;
                _hoja_trabajo.Cells[i + 5, 6].Font.Bold = true;
                _hoja_trabajo.Cells[i + 5, 7].Font.Bold = true;
                _hoja_trabajo.Cells[i + 5, 8].Font.Bold = true;
                _excelCellrange = _hoja_trabajo.Range[_hoja_trabajo.Cells[1, 1], _hoja_trabajo.Cells[i + 5, 8]];
                return GenerateFile("/Repartos/reparto-" + reparto.DiaReparto.Dia + "-" + reparto.Nombre + "." + _extension);
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
                _hoja_trabajo.Cells[1, 1] = "Numero";
                _hoja_trabajo.Cells[1, 1].Font.Bold = true;
                _hoja_trabajo.Cells[1, 2] = "Fecha";
                _hoja_trabajo.Cells[1, 2].Font.Bold = true;
                _hoja_trabajo.Cells[1, 3] = "Direccion";
                _hoja_trabajo.Cells[1, 3].Font.Bold = true;
                _hoja_trabajo.Cells[1, 4] = "Total";
                _hoja_trabajo.Cells[1, 4].Font.Bold = true;
                _hoja_trabajo.Cells[1, 5] = "Impresa";
                _hoja_trabajo.Cells[1, 5].Font.Bold = true;
                _hoja_trabajo.Cells[1, 6] = "Detalles";
                _hoja_trabajo.Cells[1, 6].Font.Bold = true;
                int i = 0;
                while (i < listaRecibos.Count)
                {
                    _hoja_trabajo.Cells[i + 2, 1] = listaRecibos[i].Id;
                    _hoja_trabajo.Cells[i + 2, 2] = listaRecibos[i].Fecha;
                    _hoja_trabajo.Cells[i + 2, 3] = listaRecibos[i].DireccionCliente;
                    _hoja_trabajo.Cells[i + 2, 4] = listaRecibos[i].ImporteTotal;
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
                _excelCellrange = _hoja_trabajo.Range[_hoja_trabajo.Cells[1, 1], _hoja_trabajo.Cells[listaRecibos.Count + 1, 5]];
                return GenerateFile("/Copias de seguridad/recibos." + _extension);
            }
            catch (Exception e)
            {
                MessageBox.Show("Error exportando (1): " + e.Message);
                return false;
            }
        }
    }
}
