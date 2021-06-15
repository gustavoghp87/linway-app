using linway_app.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace linway_app.Excel
{
    public class Exportar
    {
        private readonly string _extension;
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
                _extension = "xls";
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
                MessageBox.Show("Error exportando (2). Ver si el archivo está abierto y si existe la carpeta Excels... " + e.Message);
                return false;
            }
        }
        public bool ExportarVentas(List<Venta> lstVentas)
        {
            if (lstVentas == null) return false;
            try
            {
                _hoja_trabajo.Cells[1, 9] = "VENTAS";
                _hoja_trabajo.Cells[1, 9].Font.Bold = true;
                _hoja_trabajo.Cells[1, 9].Font.Underline = true;
                _hoja_trabajo.Cells[1, 9].Font.Size = 11;
                _hoja_trabajo.Cells[1, 1] = "Producto";
                _hoja_trabajo.Cells[1, 2] = "Cantidad";
                int i = 0;
                foreach(Venta venta in lstVentas)
                {
                    _hoja_trabajo.Cells[i + 2, 1] = venta.Producto.Nombre;
                    _hoja_trabajo.Cells[i + 2, 2] = venta.Cantidad;
                    i++;
                }
                _excelCellrange = _hoja_trabajo.Range[_hoja_trabajo.Cells[1, 1], _hoja_trabajo.Cells[lstVentas.Count + 1, 2]];
                return GenerateFile("/Excels/ventas." + _extension);
            }
            catch (Exception e)
            {
                MessageBox.Show("Error exportando (1): " + e.Message);
                return false;
            }
        }
        public bool ExportarRegistroVentas(List<RegistroVenta> lstRegistroVentas)
        {
            if (lstRegistroVentas == null) return false;
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
                while (i < lstRegistroVentas.Count)
                {
                    _hoja_trabajo.Cells[i + 2, 1] = lstRegistroVentas[i].Id;
                    _hoja_trabajo.Cells[i + 2, 2] = lstRegistroVentas[i].Fecha;
                    _hoja_trabajo.Cells[i + 2, 3] = lstRegistroVentas[i].NombreCliente;
                    i++;
                }

                _excelCellrange = _hoja_trabajo.Range[_hoja_trabajo.Cells[1, 1], _hoja_trabajo.Cells[lstRegistroVentas.Count + 1, 3]];
                return GenerateFile("/Excels/registroVentas." + _extension);
            }
            catch (Exception e)
            {
                MessageBox.Show("Error exportando (1): " + e.Message);
                return false;
            }
        }
        public bool ExportarReparto(Reparto reparto)
        {
            if (reparto == null) return false;
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
                return GenerateFile("/Excels/reparto-" + reparto.DiaReparto.Dia + "-" + reparto.Nombre + "." + _extension);
            }
            catch (Exception e)
            {
                MessageBox.Show("Error exportando (1): " + e.Message);
                return false;
            }
        }
    }
}
