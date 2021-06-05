using linway_app.Models;
using System.Collections.Generic;
using System.Linq;

namespace linway_app.Forms.Delegates
{
    public static class DDiaReparto
    {
        public delegate List<Reparto> DGetRepartosPorDia(string diaReparto);

        public readonly static DGetRepartosPorDia getRepartosPorDia
            = new DGetRepartosPorDia(GetRepartosPorDia);

        private static List<Reparto> GetRepartosPorDia(string diaReparto)
        {
            List<DiaReparto> lstDiasRep = Form1._servDiaReparto.GetAll();
            List<Reparto> lstRepartos = lstDiasRep.Find(x => x.Dia == diaReparto).Reparto.ToList();
            return lstRepartos;
        }
    }
}
