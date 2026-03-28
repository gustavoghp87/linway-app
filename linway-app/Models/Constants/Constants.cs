using System;

namespace Models
{
    public static class Constants
    {
        private static readonly string DbUser = "linway";
        private static readonly string DbPassword = "password";
        private static readonly string DbName = "linway1";
        private static readonly string DbPort = "3306";
        public static string GetDumpString()
        {
            string host;
            var pc = Environment.MachineName;
            if (pc == "DESKTOP-PC")
            {
                host = "localhost";
            }
            else
            {
                host = "192.168.0.82";
            }
            string dumpString = $"--user={DbUser} --password={DbPassword} --host={host}";
            return dumpString;
        }
        public static string GetConnectionString()
        {
            string server;
            var pc = Environment.MachineName;
            if (pc == "DESKTOP-PC")
            {
                server = "localhost";
            }
            else
            {
                server = "192.168.0.82";
            }
            string connectionString = $"Server={server};Port={DbPort};Database={DbName};Uid={DbUser};Pwd={DbPassword};";
            return connectionString;
        }
        //public static readonly string ConnectionString = "Data Source=linway-db.db";
        //public static readonly string ConnectionString = "Server=localhost;Port=3306;Database=linway;Uid=root;Pwd=password;";
        public static readonly string FormatoDeFecha = "yyyy-MM-dd";
    }
}
