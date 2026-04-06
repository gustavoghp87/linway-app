using Models;
using System;
using System.Diagnostics;
using System.IO;

namespace Infrastructure.Repositories
{
    public static class DbAutoBackup
    {
        private static readonly string _cloudFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Backups");
        private static readonly string _defaultsFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Constants.ArchivoConfigSql);
        private static readonly string _host = Constants.GetDatabaseHost();
        private static readonly string _localFolder = @"C:\Users\" + Environment.UserName + @"\Documents\OneDrive\Linway-Backups\";
        public static void Generar()
        {
            if (!File.Exists(_defaultsFile))
            {
                throw new FileNotFoundException($"Archivo de credenciales SQL '{Constants.ArchivoConfigSql}' no encontrado");
            }
            Ejecutar(_cloudFolder);
            Ejecutar(_localFolder);
        }
        private static void Ejecutar(string folder)
        {
            // primero, asegurarse de que exista la carpeta "Backups" en BaseDirectory; si no existe, crearla
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            // segundo, si ya se hizo un backup hoy, no se hace nada
            string fileName = DateTime.Now.ToString(Constants.FormatoDeFecha) + ".sql";
            string backupFilePath = Path.Combine(folder, fileName);
            if (File.Exists(backupFilePath))
            {
                return;
            }
            //
            try
            {
                var psi = new ProcessStartInfo
                {
                    FileName = @"C:\Program Files\MySQL\MySQL Server 8.0\bin\mysqldump.exe",
                    Arguments = $"--defaults-extra-file=\"{_defaultsFile}\" --host={_host} --databases {Constants.DbName} --complete-insert --result-file=\"{backupFilePath}\"",
                    UseShellExecute = false,
                    RedirectStandardError = true,
                    CreateNoWindow = true
                };
                using var process = Process.Start(psi);
                string error = process.StandardError.ReadToEnd();
                process.WaitForExit();
                if (process.ExitCode != 0)
                {
                    throw new Exception("mysqldump failed: " + error);
                }
            }
            catch (Exception e)
            {
                Logger.LogException(e);
            }
        }
    }
}
