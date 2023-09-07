using System;

namespace Models
{
    public static class Constants
    {
        public static string GetConnectionString()
        {
            string connectionString;
            var pc = Environment.MachineName;
            if (pc == "DESKTOP-PC")
            {
                connectionString = "Server=localhost;";
            }
            else
            {
                connectionString = "Server=192.168.0.82;";
            }
            connectionString += "Port=3306;Database=linway;Uid=linway;Pwd=password;";
            return connectionString;
        }

        //public static readonly string ConnectionString = "Data Source=linway-db.db";
        //public static readonly string ConnectionString = "Server=localhost;Port=3306;Database=linway;Uid=root;Pwd=password;";
        public static readonly string FormatoDeFecha = "yyyy-MM-dd";
    }
}
