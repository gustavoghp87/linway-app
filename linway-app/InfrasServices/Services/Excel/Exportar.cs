using Models;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace linway_app.Excel
{
    public class Exportar
    {
        private readonly string _extension = "xls";

        private string GetTimestamp()
        {
            return DateTimeOffset.Now.ToUnixTimeSeconds().ToString();
        }
        public bool ExportarVentas(ICollection<Venta> lstVentas)
        {
            if (lstVentas == null) return false;
            string path = @"Excels/ventas-" + DateTime.UtcNow.ToString(Constants.FormatoDeFecha) + "-" + GetTimestamp() + ".";
            try
            {
                using var fs = new FileStream(path + _extension, FileMode.Create, FileAccess.Write);
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
                cell1b.SetCellValue(DateTime.UtcNow.ToString(Constants.FormatoDeFecha));
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
            catch (Exception e)
            {
                Console.WriteLine(e.Message, "Error exportando (1):");
                return false;
            }
            return true;
        }
        public bool ExportarReparto(Reparto reparto)
        {
            if (reparto == null) return false;
            string path = @"Excels/reparto-"
                + DateTime.UtcNow.ToString(Constants.FormatoDeFecha) + "-"
                + reparto.DiaReparto.Dia.ToUpper() + "-" + reparto.Nombre + "-"
                + GetTimestamp() + ".";
            try
            {
                using var fs = new FileStream(path + _extension, FileMode.Create, FileAccess.Write);
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
                cell3.SetCellValue("PRODUCTOS      (Impreso " + DateTime.UtcNow.ToString(Constants.FormatoDeFecha) + ")");
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

                cellsToBold.Add(cell1);
                cellsToBold.Add(cell2);
                cellsToBold.Add(cell3);
                cellsToBold.Add(cell4);
                cellsToBold.Add(cell5);
                cellsToBold.Add(cell6);
                cellsToBold.Add(cell7);
                cellsToBold.Add(cell8);

                int rowIndex = 1;
                int totalBolsasA = 0;
                //  totalBolsasA = (int)reparto.Ta;
                int totalBolsasE = 0;
                //  totalBolsasE = (int)reparto.Te;
                int totalBolsasD = 0;
                //  totalBolsasD = (int)reparto.Td;
                int totalBolsasT = 0;
                //  totalBolsasT = (int)reparto.Tt;
                int totalBolsasAe = 0;
                //  totalBolsasAe = (int)reparto.Tae;
                int totalBolsas = 0;
                // long totalBolsas = reparto.Ta + reparto.Te + reparto.Td + reparto.Tt + reparto.Tae;    not working properly

                foreach (Pedido pedido in reparto.Pedidos.Where(x => x.Entregar == 1).OrderBy(x => x.Orden))
                {
                    IRow row = sheet1.CreateRow(rowIndex);

                    ICell cellA = row.CreateCell(0);
                    cellA.SetCellValue(pedido.Direccion);
                    cellsToWrap.Add(cellA);
                    ICell cellB = row.CreateCell(1);
                    if (pedido.L != 0) cellB.SetCellValue(pedido.L);
                    cellsToBold.Add(cellB);
                    ICell cellC = row.CreateCell(2);
                    if (pedido.ProductosText.Contains("x ")) pedido.ProductosText = pedido.ProductosText.Replace("x ", " ");
                    cellC.SetCellValue(pedido.ProductosText);
                    cellsToWrap.Add(cellC);
                    ICell cellD = row.CreateCell(3);
                    if (pedido.A != 0) {
                        cellD.SetCellValue(pedido.A);
                        totalBolsas += (int)pedido.A;
                        totalBolsasA += (int)pedido.A;
                    }
                    cellsToBold.Add(cellD);
                    ICell cellE = row.CreateCell(4);
                    if (pedido.E != 0) {
                        cellE.SetCellValue(pedido.E);
                        totalBolsas += (int)pedido.E;
                        totalBolsasE += (int)pedido.E;
                    }
                    cellsToBold.Add(cellE);
                    ICell cellF = row.CreateCell(5);
                    if (pedido.D != 0) {
                        cellF.SetCellValue(pedido.D);
                        totalBolsas += (int)pedido.D;
                        totalBolsasD += (int)pedido.D;
                    }
                    cellsToBold.Add(cellF);
                    ICell cellG = row.CreateCell(6);
                    if (pedido.T != 0) {
                        cellG.SetCellValue(pedido.T);
                        totalBolsas += (int)pedido.T;
                        totalBolsasT += (int)pedido.T;
                    }
                    cellsToBold.Add(cellG);
                    ICell cellH = row.CreateCell(7);
                    if (pedido.Ae != 0) {
                        cellH.SetCellValue(pedido.Ae);
                        totalBolsas += (int)pedido.Ae;
                        totalBolsasAe += (int)pedido.Ae;
                    }
                    cellsToBold.Add(cellH);

                    row.Height = pedido.Direccion.Length > 40 || pedido.ProductosText.Length > 120 ? (short)500 : row.Height;
                    row.Height = pedido.Direccion.Length > 80 || pedido.ProductosText.Length > 240 ? (short)1000 : row.Height;
                    row.Height = pedido.Direccion.Length > 120 || pedido.ProductosText.Length > 360 ? (short)1500 : row.Height;

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

                IRow rowFinal = sheet1.CreateRow(rowIndex + 1);
                rowFinal.Height = 30 * 15;
                ICell cell15 = rowFinal.CreateCell(1);
                cell15.SetCellValue(reparto.Tl);
                ICell cell15b = rowFinal.CreateCell(2);
                cell15b.SetCellValue("     PESO | Total litros: " + reparto.Tl + ",  Total bolsas: " + totalBolsas);
                ICell cell16 = rowFinal.CreateCell(3);
                cell16.SetCellValue(totalBolsasA);
                ICell cell17 = rowFinal.CreateCell(4);
                cell17.SetCellValue(totalBolsasE);
                ICell cell18 = rowFinal.CreateCell(5);
                cell18.SetCellValue(totalBolsasD);
                ICell cell19 = rowFinal.CreateCell(6);
                cell19.SetCellValue(totalBolsasT);
                ICell cell20 = rowFinal.CreateCell(7);
                cell20.SetCellValue(totalBolsasAe);

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

                cellsToBold.ForEach(cell =>
                {
                    cell.CellStyle = workbook.CreateCellStyle();
                    cell.CellStyle.SetFont(fontBold);
                    cell.CellStyle.VerticalAlignment = VerticalAlignment.Center;
                    cell.CellStyle.Alignment = (NPOI.SS.UserModel.HorizontalAlignment)HorizontalAlignment.Center;
                    cell.CellStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                    cell.CellStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
                    cell.CellStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                    cell.CellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                });
                cellsToVA.ForEach(cell =>
                {
                    cell.CellStyle = workbook.CreateCellStyle();
                    cell.CellStyle.SetFont(fontBold);
                    cell.CellStyle.VerticalAlignment = VerticalAlignment.Center;
                    cell.CellStyle.Alignment = (NPOI.SS.UserModel.HorizontalAlignment)HorizontalAlignment.Center;
                    cell.CellStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                    cell.CellStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
                    cell.CellStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                    cell.CellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                });
                cellsToHA.ForEach(cell =>
                {
                    cell.CellStyle = workbook.CreateCellStyle();
                    cell.CellStyle.SetFont(fontBold);
                    cell.CellStyle.VerticalAlignment = VerticalAlignment.Center;
                    cell.CellStyle.Alignment = (NPOI.SS.UserModel.HorizontalAlignment)HorizontalAlignment.Center;
                    cell.CellStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                    cell.CellStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
                    cell.CellStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                    cell.CellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                });
                cellsToWrap.ForEach(cell =>
                {
                    cell.CellStyle = workbook.CreateCellStyle();
                    cell.CellStyle = wrapCellStyle;
                    cell.CellStyle.VerticalAlignment = VerticalAlignment.Center;
                    cell.CellStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                    cell.CellStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
                    cell.CellStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                    cell.CellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
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
            catch (Exception e)
            {
                Console.WriteLine(e.Message, "Error exportando (1):");
                return false;
            }
            return true;
        }
    }
}
