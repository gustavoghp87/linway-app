using linway_app.Models;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;

namespace linway_app.Repositories
{
    public static class SQLiteCommands
    {
        public static bool Execute(string commandText)
        {
            try
            {
                using (var connection = new SqliteConnection(DbString.ConnectionString))
                {
                    connection.Open();
                    var command = connection.CreateCommand();
                    command.CommandText = commandText;
                    command.ExecuteNonQueryAsync();
                }
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        //string commandText = $"INSERT INTO Cliente(Direccion, CodigoPostal, Telefono, Name, CUIT, Tipo, Estado) " +
        //                     $"VALUES ('{cliente.Direccion}', '{cliente.CodigoPostal}', '{cliente.Telefono}', " +
        //                             $"'{cliente.Name}', '{cliente.Cuit}', '{cliente.Tipo}', 'Activo')";
        //return SQLiteCommands.Execute(commandText);

    }
}
