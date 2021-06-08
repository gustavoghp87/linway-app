using linway_app.Models;
using System.Collections.Generic;
using System.Windows.Forms;

namespace linway_app.Forms.Delegates
{
    public static class DDiaReparto
    {
        public delegate void DAddDiaReparto(DiaReparto diaReparto);
        public delegate List<DiaReparto> DGetDiaRepartos();

        public readonly static DAddDiaReparto addDiaReparto
            = new DAddDiaReparto(AddDiaReparto);
        public readonly static DGetDiaRepartos getDiaRepartos
            = new DGetDiaRepartos(GetDiaRepartos);

        private static void AddDiaReparto(DiaReparto diaReparto)
        {
            bool response = Form1._servDiaReparto.Add(diaReparto);
            if (!response) MessageBox.Show("Algo falló al agregar nuevo Día de Reparto a la base de datos");
        }
        private static List<DiaReparto> GetDiaRepartos()
        {
            return Form1._servDiaReparto.GetAll();
        }
    }
}
