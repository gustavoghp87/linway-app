using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

namespace Infrastructure.Repositories
{
    public static class SqliteToMysqlConverter
    {
        public static void ConvertSqliteToMysql()
        {
            string inputFilePath = "linway-db.db.sql";
            string outputFilePath = "output.sql";
            try
            {
                using (StreamReader reader = new StreamReader(inputFilePath))
                using (StreamWriter writer = new StreamWriter(outputFilePath))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        Match match = Regex.Match(line, @"INSERT INTO (\w+) VALUES", RegexOptions.IgnoreCase);
                        if (match.Success)
                        {
                            string tableName = match.Groups[1].Value;
                            string[] columnNames = GetColumnNamesForTable(tableName);
                            string newLine = $"INSERT INTO {tableName} ({string.Join(", ", columnNames)}) VALUES";
                            writer.WriteLine(newLine);
                        }
                        else
                        {
                            writer.WriteLine(line);
                        }
                    }
                }
                Console.WriteLine("Proceso completado. El resultado se ha guardado en 'output.sql'");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
        private static string[] GetColumnNamesForTable(string tableName)
        {
            Console.WriteLine(tableName);
            switch (tableName)
            {
                case "DiaReparto":
                    return new string[] { "Id", "Dia", "Estado" };
                case "Producto":
                    return new string[] { "Id", "Nombre", "Precio", "Estado", "Tipo", "SubTipo" };
                case "Cliente":
                    return new string[] { "Id", "Direccion", "CodigoPostal", "Telefono", "Nombre", "CUIT", "Tipo", "Estado" };
                case "Venta":
                    return new string[] { "Id", "ProductoId", "Cantidad", "Estado" };
                case "Reparto":
                    return new string[] { "Id", "Nombre", "DiaRepartoId", "TA", "TE", "TD", "TT", "TAE", "TotalB", "TL", "Estado" };
                case "RegistroVenta":
                    return new string[] { "Id", "ClienteId", "NombreCliente", "Fecha", "Estado" };
                case "NotaDeEnvio":
                    return new string[] { "Id", "ClienteId", "Fecha", "Impresa", "Detalle", "ImporteTotal", "Estado" };
                case "Recibo":
                    return new string[] { "Id", "ClienteId", "Fecha", "Impreso", "DireccionCliente", "ImporteTotal", "Estado" };
                case "Pedido":
                    return new string[] { "Id", "Direccion", "ClienteId", "RepartoId", "Entregar", "L", "A", "E", "D", "T", "AE", "ProductosText", "Orden", "Estado" };
                case "ProdVendido":
                    return new string[] { "Id", "ProductoId", "NotaDeEnvioId", "RegistroVentaId", "PedidoId", "Cantidad", "Descripcion", "Precio", "Estado" };
                case "DetalleRecibo":
                    return new string[] { "Id", "ReciboId", "Detalle", "Importe", "Estado" };
                default:
                    return new string[] { "" };
            }
        }
    }
}
