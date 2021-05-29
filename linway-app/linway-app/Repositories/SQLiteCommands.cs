using Microsoft.Data.Sqlite;
using System;

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
                    command.ExecuteNonQuery();
                }
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
    }
}
