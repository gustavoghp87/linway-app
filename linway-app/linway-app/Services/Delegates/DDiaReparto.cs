using linway_app.Forms;
using linway_app.Models;
using linway_app.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace linway_app.Services.Delegates
{
    public static class DDiaReparto
    {
        public readonly static Action<DiaReparto> addDiaReparto = AddDiaReparto;
        public readonly static Func<List<DiaReparto>> getDiaRepartos = GetDiaRepartos;

        private static readonly IServiceBase<DiaReparto> _service = Form1._servDiaReparto;
        private static void AddDiaReparto(DiaReparto diaReparto)
        {
            bool response = _service.Add(diaReparto);
            if (!response) MessageBox.Show("Algo falló al agregar nuevo Día de Reparto a la base de datos");
        }
        private static List<DiaReparto> GetDiaRepartos()
        {
            return _service.GetAll();
        }
    }
}
