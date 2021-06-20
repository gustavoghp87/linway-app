using linway_app.Models;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace linway_app.Excel
{
    public class Exportar
    {
        private readonly string _extension;

        public Exportar()
        {
            try
            {
                _extension = "xls";
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error al exportar a Excel:");
            }
        }
        private string GetTimestamp()
        {
            DateTimeOffset dto = new DateTimeOffset();
            return dto.ToUnixTimeSeconds().ToString();
        }
        public bool ExportarVentas(List<Venta> lstVentas)
        {
            if (lstVentas == null) return false;
            string path = @"Excels/ventas-" + DateTime.UtcNow.ToString("yyyy-MM-dd") + ".";
            try
            {
                using (var fs = new FileStream(path + _extension, FileMode.Create, FileAccess.Write))
                {
                    IWorkbook workbook = new XSSFWorkbook();
                    ISheet sheet1 = workbook.CreateSheet("Hoja1");
                    sheet1.AddMergedRegion(new CellRangeAddress(0, 0, 0, lstVentas.Count + 2));
                    
                    var style1 = workbook.CreateCellStyle();
                    style1.FillForegroundColor = HSSFColor.Blue.Index2;
                    style1.FillPattern = FillPattern.SolidForeground;
                    style1.VerticalAlignment = VerticalAlignment.Center;

                    var fontBold = workbook.CreateFont();
                    fontBold.FontHeightInPoints = 11;
                    fontBold.IsBold = true;

                    IRow row0 = sheet1.CreateRow(1);
                    row0.Height = 30 * 15;

                    ICell cell1 = row0.CreateCell(6);
                    cell1.CellStyle = style1;
                    cell1.SetCellValue("VENTAS");
                    cell1.CellStyle.SetFont(fontBold);

                    ICell cell1b = row0.CreateCell(7);
                    cell1b.CellStyle = style1;
                    cell1b.SetCellValue(DateTime.UtcNow.ToString("yyyy-MM-dd"));
                    cell1b.CellStyle.SetFont(fontBold);

                    ICell cell2 = row0.CreateCell(0);
                    cell2.CellStyle = style1;
                    cell2.SetCellValue("PRODUCTO");
                    cell2.CellStyle.SetFont(fontBold);

                    ICell cell3 = row0.CreateCell(1);
                    cell3.CellStyle = style1;
                    cell3.SetCellValue("CANT");
                    cell3.CellStyle.SetFont(fontBold);

                    var rowIndex = 2;
                    foreach (Venta venta in lstVentas.OrderBy(x => x.Producto.Nombre))
                    {
                        IRow row = sheet1.CreateRow(rowIndex);
                        var cellA = row.CreateCell(0);
                        cellA.SetCellValue(venta.Producto.Nombre);
                        cellA.CellStyle.SetFont(fontBold);
                        var cellB = row.CreateCell(1);
                        cellB.SetCellValue(venta.Cantidad);
                        rowIndex++;
                    }
                    sheet1.AutoSizeColumn(0);
                    sheet1.AutoSizeColumn(1);
                    sheet1.AutoSizeColumn(7);

                    workbook.Write(fs);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error exportando (1):");
                return false;
            }
            return true;
        }
        public bool ExportarReparto(Reparto reparto)
        {
            if (reparto == null) return false;
            string path = @"Excels/reparto-" + reparto.DiaReparto.Dia.ToUpper() + "-" + reparto.Nombre + "-"
                + DateTime.UtcNow.ToString("yyyy-MM-dd") + GetTimestamp() + ".";
            try
            {
                using (var fs = new FileStream(path + _extension, FileMode.Create, FileAccess.Write))
                {
                    IWorkbook workbook = new XSSFWorkbook();
                    ISheet sheet1 = workbook.CreateSheet("Hoja1");
                    sheet1.AddMergedRegion(new CellRangeAddress(0, 0, 0, reparto.Pedidos.Count + 2));

                    var styleVA = workbook.CreateCellStyle();
                    styleVA.VerticalAlignment = VerticalAlignment.Center;
                    //style1.FillForegroundColor = HSSFColor.LightGreen.Index;
                    //style1.FillPattern = FillPattern.SolidForeground;

                    var fontBold = workbook.CreateFont();
                    fontBold.FontHeightInPoints = 11;
                    fontBold.IsBold = true;

                    IRow row0 = sheet1.CreateRow(1);

                    ICell cell1 = row0.CreateCell(0);
                    cell1.SetCellValue("REPARTO " + reparto.DiaReparto.Dia.ToUpper() + " - " + reparto.Nombre);
                    cell1.CellStyle = workbook.CreateCellStyle();
                    //cell1.CellStyle = styleVA;
                    cell1.CellStyle.SetFont(fontBold);
                    //cell1.CellStyle.SetFont(fontBold);
                    ICell cell2 = row0.CreateCell(1);
                    cell2.CellStyle = styleVA;
                    cell2.SetCellValue("Ltrs");
                    cell2.CellStyle.SetFont(fontBold);
                    ICell cell3 = row0.CreateCell(2);
                    cell3.CellStyle = styleVA;
                    cell3.SetCellValue("PRODUCTOS      (Impreso " + DateTime.UtcNow.ToString("yyyy-MM-dd") + ")");
                    cell3.CellStyle.SetFont(fontBold);
                    ICell cell4 = row0.CreateCell(3);
                    cell4.CellStyle = styleVA;
                    cell4.SetCellValue("A");
                    cell4.CellStyle.SetFont(fontBold);
                    ICell cell5 = row0.CreateCell(4);
                    cell5.CellStyle = styleVA;
                    cell5.SetCellValue("E");
                    cell5.CellStyle.SetFont(fontBold);
                    ICell cell6 = row0.CreateCell(5);
                    cell6.CellStyle = styleVA;
                    cell6.SetCellValue("D");
                    cell6.CellStyle.SetFont(fontBold);
                    ICell cell7 = row0.CreateCell(6);
                    cell7.CellStyle = styleVA;
                    cell7.SetCellValue("T");
                    cell7.CellStyle.SetFont(fontBold);
                    ICell cell8 = row0.CreateCell(7);
                    cell8.CellStyle = styleVA;
                    cell8.SetCellValue("AE");
                    cell8.CellStyle.SetFont(fontBold);

                    var rowIndex = 2;
                    foreach (Pedido pedido in reparto.Pedidos.Where(x => x.Entregar == 1).OrderBy(x => x.Orden))
                    {
                        IRow row = sheet1.CreateRow(rowIndex);
                        var wrapCellStyle = workbook.CreateCellStyle();
                        //wrapCellStyle.WrapText = true;

                        var cellA = row.CreateCell(0);
                        cellA.SetCellValue(pedido.Direccion);
                        cellA.CellStyle.SetFont(fontBold);
                        //cellA.CellStyle = wrapCellStyle;
                        cellA.CellStyle = styleVA;
                        if (pedido.L != 0)
                        {
                            var cellB = row.CreateCell(1);
                            cellB.SetCellValue(pedido.L);
                            //cellB.CellStyle = wrapCellStyle;
                            //cellB.CellStyle = styleVA;
                        }
                        var cellC = row.CreateCell(2);
                        cellC.SetCellValue(pedido.ProductosText);
                        //cellC.CellStyle = wrapCellStyle;
                        cellC.CellStyle = styleVA;
                        if (pedido.A != 0)
                        {
                            var cellD = row.CreateCell(3);
                            cellD.SetCellValue(pedido.A);
                            cellD.CellStyle = styleVA;
                        }
                        if (pedido.E != 0)
                        {
                            var cellE = row.CreateCell(4);
                            cellE.SetCellValue(pedido.E);
                            cellE.CellStyle = styleVA;
                        }
                        if (pedido.D != 0)
                        {
                            var cellF = row.CreateCell(5);
                            cellF.SetCellValue(pedido.D);
                            cellF.CellStyle = styleVA;
                        }
                        if (pedido.T != 0)
                        {
                            var cellG = row.CreateCell(6);
                            cellG.SetCellValue(pedido.T);
                            cellG.CellStyle = styleVA;
                        }
                        if (pedido.Ae != 0)
                        {
                            var cellH = row.CreateCell(7);
                            cellH.SetCellValue(pedido.Ae);
                            cellH.CellStyle = styleVA;
                        }
                        row.Height = (short)AutoSizeMode.GrowAndShrink;
                        rowIndex++;
                    }

                    IRow rowAnteU = sheet1.CreateRow(rowIndex);
                    rowAnteU.Height = 30 * 15;
                    ICell cell9 = rowAnteU.CreateCell(1);
                    cell9.CellStyle = styleVA;
                    //cell9.CellStyle = styleHA;
                    cell9.SetCellValue("Ltrs");
                    cell9.CellStyle.SetFont(fontBold);
                    ICell cell10 = rowAnteU.CreateCell(3);
                    cell10.CellStyle = styleVA;
                    //cell10.CellStyle = styleHA;
                    cell10.SetCellValue("A");
                    cell10.CellStyle.SetFont(fontBold);
                    ICell cell11 = rowAnteU.CreateCell(4);
                    cell11.CellStyle = styleVA;
                    //cell11.CellStyle = styleHA;
                    cell11.SetCellValue("E");
                    cell11.CellStyle.SetFont(fontBold);
                    ICell cell12 = rowAnteU.CreateCell(5);
                    cell12.CellStyle = styleVA;
                    //cell12.CellStyle = styleHA;
                    cell12.SetCellValue("D");
                    cell12.CellStyle.SetFont(fontBold);
                    ICell cell13 = rowAnteU.CreateCell(6);
                    cell13.CellStyle = styleVA;
                    //cell13.CellStyle = styleHA;
                    cell13.SetCellValue("T");
                    cell13.CellStyle.SetFont(fontBold);
                    ICell cell14 = rowAnteU.CreateCell(7);
                    cell14.CellStyle = styleVA;
                    //cell14.CellStyle = styleHA;
                    cell14.SetCellValue("Ae");
                    cell14.CellStyle.SetFont(fontBold);

                    IRow rowFinal = sheet1.CreateRow(rowIndex + 1);
                    rowFinal.Height = 30 * 15;
                    ICell cell15 = rowFinal.CreateCell(1);
                    cell15.CellStyle = styleVA;
                    //cell15.CellStyle = styleHA;
                    cell15.SetCellValue(reparto.Tl);
                    cell15.CellStyle.SetFont(fontBold);
                    ICell cell15b = rowFinal.CreateCell(2);
                    cell15b.CellStyle = styleVA;
                    //cell15b.CellStyle = styleHA;
                    long totalBolsas = reparto.Ta + reparto.Te + reparto.Td + reparto.Tt + reparto.Tae;
                    cell15b.SetCellValue("     PESO | Total litros: " + reparto.Tl + ",  Total bolsas: " + totalBolsas);
                    cell15b.CellStyle.SetFont(fontBold);
                    ICell cell16 = rowFinal.CreateCell(3);
                    cell16.CellStyle = styleVA;
                    //cell16.CellStyle = styleHA;
                    cell16.SetCellValue(reparto.Ta);
                    cell16.CellStyle.SetFont(fontBold);
                    ICell cell17 = rowFinal.CreateCell(4);
                    cell17.CellStyle = styleVA;
                    //cell17.CellStyle = styleHA;
                    cell17.SetCellValue(reparto.Te);
                    cell17.CellStyle.SetFont(fontBold);
                    ICell cell18 = rowFinal.CreateCell(5);
                    cell18.CellStyle = styleVA;
                    //cell18.CellStyle = styleHA;
                    cell18.SetCellValue(reparto.Td);
                    cell18.CellStyle.SetFont(fontBold);
                    ICell cell19 = rowFinal.CreateCell(6);
                    cell19.CellStyle = styleVA;
                    //cell19.CellStyle = styleHA;
                    cell19.SetCellValue(reparto.Tt);
                    cell19.CellStyle.SetFont(fontBold);
                    ICell cell20 = rowFinal.CreateCell(7);
                    //cell20.CellStyle = styleVA;
                    //cell20.CellStyle = styleHA;
                    cell20.SetCellValue(reparto.Tae);
                    cell20.CellStyle.SetFont(fontBold);

                    sheet1.AutoSizeColumn(0);
                    sheet1.AutoSizeColumn(1);
                    sheet1.AutoSizeColumn(2);
                    sheet1.AutoSizeColumn(3);
                    sheet1.AutoSizeColumn(4);
                    sheet1.AutoSizeColumn(5);
                    sheet1.AutoSizeColumn(6);
                    sheet1.AutoSizeColumn(7);

                    workbook.Write(fs);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error exportando (1):");
                return false;
            }
            return true;
        }
    }
}
