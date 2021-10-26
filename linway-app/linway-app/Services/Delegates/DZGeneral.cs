using System;
using System.Collections.Generic;

namespace linway_app.Services.Delegates
{
    public static class DZGeneral
    {
        public static readonly Func<string, List<string>> ignorarTildes = IgnorarTildes;
        public static readonly Func<string, string> invertirFecha = InvertirFecha;
        public static readonly string FormatoDeFecha = "yyyy-MM-dd";

        private static List<string> IgnorarTildes(string palabra)
        {
            palabra = palabra.ToLower().Trim();
            if (palabra == string.Empty) return null;
            if (palabra.Contains("á") || palabra.Contains("é") || palabra.Contains("í")
                || palabra.Contains("ó") || palabra.Contains("ú")) return null;
            var palabrasList = new List<string>();
            for (int i = 0; i < palabra.Length; i++)
            {
                if (palabra[i].ToString() == "a") palabrasList.Add(palabra.Substring(0, i) + "á" + palabra.Substring(i + 1));
                else if (palabra[i].ToString() == "e") palabrasList.Add(palabra.Substring(0, i) + "é" + palabra.Substring(i + 1));
                else if (palabra[i].ToString() == "i") palabrasList.Add(palabra.Substring(0, i) + "í" + palabra.Substring(i + 1));
                else if (palabra[i].ToString() == "o") palabrasList.Add(palabra.Substring(0, i) + "ó" + palabra.Substring(i + 1));
                else if (palabra[i].ToString() == "u") palabrasList.Add(palabra.Substring(0, i) + "ú" + palabra.Substring(i + 1));
            }
            return palabrasList;
        }
        private static string InvertirFecha(string fecha)
        {
            if (!fecha.Contains("-")) return fecha;
            var array = fecha.Split('-');
            if (array.Length != 3) return fecha;
            return array[2] + "-" + array[1] + "-" + array[0];
        }
    }
}
