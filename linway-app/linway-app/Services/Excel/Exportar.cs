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
            return DateTimeOffset.Now.ToUnixTimeSeconds().ToString();
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
                + DateTime.UtcNow.ToString("yyyy-MM-dd") + "-" + GetTimestamp() + ".";
            try
            {
                using (var fs = new FileStream(path + _extension, FileMode.Create, FileAccess.Write))
                {
                    IWorkbook workbook = new XSSFWorkbook();
                    ISheet sheet1 = workbook.CreateSheet("Hoja1");
                    // sheet1.AddMergedRegion(new CellRangeAddress(0, 0, 0, reparto.Pedidos.Count + 2));

                    var style1 = workbook.CreateCellStyle();
                        style1.FillPattern = FillPattern.SolidForeground;
                        style1.FillForegroundColor = HSSFColor.Blue.Index2;
                    var fontBold = workbook.CreateFont();
                        fontBold.FontHeightInPoints = 11;
                        fontBold.IsBold = true;
                    var wrapCellStyle = workbook.CreateCellStyle();
                        wrapCellStyle.WrapText = true;

                    IRow row0 = sheet1.CreateRow(0);
                         row0.Height = 30 * 15;

                    ICell cell1 = row0.CreateCell(0);
                          cell1.SetCellValue("REPARTO " + reparto.DiaReparto.Dia.ToUpper() + " - " + reparto.Nombre);
                          cell1.CellStyle = style1;
                    ICell cell2 = row0.CreateCell(1);
                          cell2.SetCellValue("Ltrs");
                          cell2.CellStyle = style1;
                    ICell cell3 = row0.CreateCell(2);
                          cell3.SetCellValue("PRODUCTOS      (Impreso " + DateTime.UtcNow.ToString("yyyy-MM-dd") + ")");
                          cell3.CellStyle = style1;
                    ICell cell4 = row0.CreateCell(3);
                          cell4.SetCellValue("A");
                          cell4.CellStyle = style1;
                    ICell cell5 = row0.CreateCell(4);
                          cell5.SetCellValue("E");
                          cell5.CellStyle = style1;
                    ICell cell6 = row0.CreateCell(5);
                          cell6.SetCellValue("D");
                          cell6.CellStyle = style1;
                    ICell cell7 = row0.CreateCell(6);
                          cell7.SetCellValue("T");
                          cell7.CellStyle = style1;
                    ICell cell8 = row0.CreateCell(7);
                          cell8.SetCellValue("AE");
                          cell8.CellStyle = style1;

                    var cellsToBold = new List<ICell>();
                    var cellsToVA = new List<ICell>();
                    var cellsToHA = new List<ICell>();
                    var cellsToWrap = new List<ICell>();

                    var rowIndex = 1;
                    foreach (Pedido pedido in reparto.Pedidos.Where(x => x.Entregar == 1).OrderBy(x => x.Orden))
                    {
                        IRow row = sheet1.CreateRow(rowIndex);

                        ICell cellA = row.CreateCell(0);
                              cellA.SetCellValue(pedido.Direccion);
                              cellsToWrap.Add(cellA);
                        ICell cellB = row.CreateCell(1);
                              if (pedido.L != 0) cellB.SetCellValue(pedido.L);
                        ICell cellC = row.CreateCell(2);
                              cellC.SetCellValue(pedido.ProductosText);
                              cellsToWrap.Add(cellC);
                        ICell cellD = row.CreateCell(3);
                              if (pedido.A != 0) cellD.SetCellValue(pedido.A);
                        ICell cellE = row.CreateCell(4);
                              if (pedido.E != 0) cellE.SetCellValue(pedido.E);
                        ICell cellF = row.CreateCell(5);
                              if (pedido.D != 0) cellF.SetCellValue(pedido.D);
                        ICell cellG = row.CreateCell(6);
                              if (pedido.T != 0) cellG.SetCellValue(pedido.T);
                        ICell cellH = row.CreateCell(7);
                              if (pedido.Ae != 0) cellH.SetCellValue(pedido.Ae);

                        cellsToVA.Add(cellA);
                        cellsToVA.Add(cellB);
                        cellsToVA.Add(cellC);
                        cellsToVA.Add(cellD);
                        cellsToVA.Add(cellE);
                        cellsToVA.Add(cellF);
                        cellsToVA.Add(cellG);
                        cellsToVA.Add(cellH);

                        cellsToHA.Add(cellB);
                        cellsToHA.Add(cellD);
                        cellsToHA.Add(cellE);
                        cellsToHA.Add(cellF);
                        cellsToHA.Add(cellG);
                        cellsToHA.Add(cellH);
                        //row.Height = (short)AutoSizeMode.GrowAndShrink;
                        rowIndex++;
                    }

                    IRow rowAnteU = sheet1.CreateRow(rowIndex);
                         rowAnteU.Height = 30 * 15;
                        ICell cell9 = rowAnteU.CreateCell(1);
                        cell9.SetCellValue("Ltrs");
                        ICell cell10 = rowAnteU.CreateCell(3);
                        cell10.SetCellValue("A");
                        ICell cell11 = rowAnteU.CreateCell(4);
                        cell11.SetCellValue("E");
                        ICell cell12 = rowAnteU.CreateCell(5);
                        cell12.SetCellValue("D");
                        ICell cell13 = rowAnteU.CreateCell(6);
                        cell13.SetCellValue("T");
                        ICell cell14 = rowAnteU.CreateCell(7);
                        cell14.SetCellValue("Ae");

                    long totalBolsas = reparto.Ta + reparto.Te + reparto.Td + reparto.Tt + reparto.Tae;
                    IRow rowFinal = sheet1.CreateRow(rowIndex + 1);
                         rowFinal.Height = 30 * 15;
                        ICell cell15 = rowFinal.CreateCell(1);
                        cell15.SetCellValue(reparto.Tl);
                        ICell cell15b = rowFinal.CreateCell(2);
                        cell15b.SetCellValue("     PESO | Total litros: " + reparto.Tl + ",  Total bolsas: " + totalBolsas);
                        ICell cell16 = rowFinal.CreateCell(3);
                        cell16.SetCellValue(reparto.Ta);
                        ICell cell17 = rowFinal.CreateCell(4);
                        cell17.SetCellValue(reparto.Te);
                        ICell cell18 = rowFinal.CreateCell(5);
                        cell18.SetCellValue(reparto.Td);
                        ICell cell19 = rowFinal.CreateCell(6);
                        cell19.SetCellValue(reparto.Tt);
                        ICell cell20 = rowFinal.CreateCell(7);
                        cell20.SetCellValue(reparto.Tae);

                    cellsToBold.Add(cell1);
                    cellsToBold.Add(cell2);
                    cellsToBold.Add(cell3);
                    cellsToBold.Add(cell4);
                    cellsToBold.Add(cell5);
                    cellsToBold.Add(cell6);
                    cellsToBold.Add(cell7);
                    cellsToBold.Add(cell8);
                    cellsToBold.Add(cell9);
                    cellsToBold.Add(cell10);
                    cellsToBold.Add(cell11);
                    cellsToBold.Add(cell12);
                    cellsToBold.Add(cell13);
                    cellsToBold.Add(cell14);
                    cellsToBold.Add(cell15);
                    cellsToBold.Add(cell15b);
                    cellsToBold.Add(cell16);
                    cellsToBold.Add(cell17);
                    cellsToBold.Add(cell18);
                    cellsToBold.Add(cell19);
                    cellsToBold.Add(cell20);

                    cellsToVA.Add(cell1);
                    cellsToVA.Add(cell2);
                    cellsToVA.Add(cell3);
                    cellsToVA.Add(cell4);
                    cellsToVA.Add(cell5);
                    cellsToVA.Add(cell6);
                    cellsToVA.Add(cell7);
                    cellsToVA.Add(cell8);
                    cellsToVA.Add(cell9);
                    cellsToVA.Add(cell10);
                    cellsToVA.Add(cell11);
                    cellsToVA.Add(cell12);
                    cellsToVA.Add(cell13);
                    cellsToVA.Add(cell14);
                    cellsToVA.Add(cell15);
                    cellsToVA.Add(cell15b);
                    cellsToVA.Add(cell16);
                    cellsToVA.Add(cell17);
                    cellsToVA.Add(cell18);
                    cellsToVA.Add(cell19);
                    cellsToVA.Add(cell20);

                    cellsToHA.Add(cell1);
                    cellsToHA.Add(cell2);
                    cellsToHA.Add(cell3);
                    cellsToHA.Add(cell4);
                    cellsToHA.Add(cell5);
                    cellsToHA.Add(cell6);
                    cellsToHA.Add(cell7);
                    cellsToHA.Add(cell8);
                    cellsToHA.Add(cell9);
                    cellsToHA.Add(cell10);
                    cellsToHA.Add(cell11);
                    cellsToHA.Add(cell12);
                    cellsToHA.Add(cell13);
                    cellsToHA.Add(cell14);
                    cellsToHA.Add(cell15);
                    cellsToHA.Add(cell15b);
                    cellsToHA.Add(cell16);
                    cellsToHA.Add(cell17);
                    cellsToHA.Add(cell18);
                    cellsToHA.Add(cell19);
                    cellsToHA.Add(cell20);

                    cellsToBold.ForEach(cell =>
                    {
                        cell.CellStyle = workbook.CreateCellStyle();
                        cell.CellStyle.SetFont(fontBold);
                    });
                    cellsToVA.ForEach(cell =>
                    {
                        cell.CellStyle = workbook.CreateCellStyle();
                        cell.CellStyle.VerticalAlignment = VerticalAlignment.Center;
                    });
                    cellsToHA.ForEach(cell =>
                    {
                        cell.CellStyle = workbook.CreateCellStyle();
                        cell.CellStyle.SetFont(fontBold);
                        cell.CellStyle.VerticalAlignment = VerticalAlignment.Center;
                        cell.CellStyle.Alignment = (NPOI.SS.UserModel.HorizontalAlignment)System.Windows.Forms.HorizontalAlignment.Center;
                    });
                    cellsToWrap.ForEach(cell =>
                    {
                        cell.CellStyle = workbook.CreateCellStyle();
                        cell.CellStyle = wrapCellStyle;
                    });


                    sheet1.AutoSizeColumn(0);
                    sheet1.AutoSizeColumn(1);
                    sheet1.SetColumnWidth(2, 25000);
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
