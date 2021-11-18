using System;
using System.IO;

namespace linway_app.Services.Delegates
{
    public static class DDbBackup
    {
        public readonly static Action generateDbBackup = Generate;
        public readonly static string windowsUserName = Environment.UserName;
        public readonly static string path = @"C:\Users\" + windowsUserName + @"\Documents\OneDrive\Linway-Backups\";
        //private readonly static string absolutePath = @"C:\Compartidos VV\Sistema Linway\";

        private static void Generate()
        {
            //string nameFile = "linway-db.db";
            string month = DateTime.Now.Month.ToString(); if (month.Length == 1) month = "0" + month;
            string day = DateTime.Now.Day.ToString(); if (day.Length == 1) day = "0" + day;
            string nameFolder = "backup-" + DateTime.Now.Year.ToString() + "-" + month + "-" + day;
            string destinationFolder = Path.Combine(path, nameFolder);
            //string filePath = Path.Combine(destinationFolder, nameFile);
            //string currentFolder = AppDomain.CurrentDomain.BaseDirectory;
            try
            {
                if (Directory.Exists(destinationFolder))
                {
                    Console.WriteLine(destinationFolder + " path already exists");
                    return;
                }
                //Directory.CreateDirectory(destinationFolder);
                Console.WriteLine("The directory was created successfully at {0}", Directory.GetCreationTime(destinationFolder));
                // File.Copy(Path.Combine(currentFolder, nameFile), filePath, true);
                Console.WriteLine("The backup file was created successfully at {0}", Directory.GetCreationTime(destinationFolder));
            }
            catch (Exception e)
            {
                Console.WriteLine("The process failed: {0}", e.ToString());
            }
        }
    }
}
