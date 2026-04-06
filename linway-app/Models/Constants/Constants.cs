using System;

namespace Models
{
    public static class Constants
    {
        private static readonly string _dbUser = "linway";
        private static readonly string _dbPassword = "password";
        private static readonly string _dbPort = "3306";
        public static readonly string DbName = "linway";
        public static readonly string FormatoDeFecha = "yyyy-MM-dd";
        public static readonly string ArchivoConfigSql = "mysql-backup.cnf";
        public static string GetDatabaseHost()
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
            return host;
        }
        public static string GetConnectionString()
        {
            string server = GetDatabaseHost();
            return $"Server={server};Port={_dbPort};Database={DbName};Uid={_dbUser};Pwd={_dbPassword};";
        }
    }
}
