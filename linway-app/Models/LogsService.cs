using System;
using System.Diagnostics;
using System.IO;

namespace Models
{
    public static class Logger
    {
        public static void LogException(Exception ex)
        {
            string logFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs.txt");
            string reason = GetReason(ex);
            string logMessage = $"{DateTime.UtcNow.AddHours(-3):yyyy-MM-dd HH:mm:ss} - {ex.GetType().Name}: {ex.Message}\n" +
                $"Reason: {reason}\n" +
                $"Source: {ex.Source}\n" +
                $"StackTrace: {ex.StackTrace}\n" +
                $"Line Number: {GetLineNumber(ex)}\n";
            try
            {
                Console.WriteLine(reason);
                Console.WriteLine(logMessage);
                using StreamWriter writer = new StreamWriter(logFilePath, true);
                writer.WriteLine(logMessage);
                writer.WriteLine(new string('-', 80));
            }
            catch (Exception logEx)
            {
                Console.WriteLine($"Error al registrar la excepción en el archivo de registro: {logEx.Message}");
            }
        }
        public static string GetReason(Exception ex)
        {
            if (ex == null)
            {
                return string.Empty;
            }
            string result = ex.Message;
            for (var inner = ex.InnerException; inner != null; inner = inner.InnerException)
            {
                result += " --> " + inner.Message;
            }
            return result;
        }
        private static int GetLineNumber(Exception e)
        {
            var st = new StackTrace(e, true);
            if (st.FrameCount == 0)
            {
                return -1;
            }
            StackFrame frame = st.GetFrame(0);
            return frame.GetFileLineNumber();
        }
    }
}
