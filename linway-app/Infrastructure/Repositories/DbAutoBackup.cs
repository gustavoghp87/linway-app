using Models;
using MySql.Data.MySqlClient;
using System;
using System.IO;

namespace Infrastructure.Repositories
{
    public static class DbAutoBackup
    {
        public static void Generate()
        {
            string connectionString = Constants.GetConnectionString();
            string dumpString = Constants.GetDumpString();
            string fileName = DateTime.Now.ToString(Constants.FormatoDeFecha) + ".sql";
            string backupFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
            try
            {
                using MySqlConnection connection = new MySqlConnection(connectionString);
                connection.Open();
                using MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = connection;
                cmd.CommandText = $"mysqldump {dumpString} > '" + backupFilePath + "';";
                cmd.ExecuteNonQuery();
                connection.Close();
            }
            catch (Exception e)
            {
                Logger.LogException(e);
            }
        }
    }
}
