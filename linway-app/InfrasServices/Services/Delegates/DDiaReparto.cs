using InfrasServices.Services;
using linway_app.Services.Interfaces;
using Models;
using System;
using System.Collections.Generic;

namespace linway_app.Services.Delegates
{
    public static class DDiaReparto
    {
        public readonly static Action<DiaReparto> addDiaReparto = AddDiaReparto;
        public readonly static Func<List<DiaReparto>> getDiaRepartos = GetDiaRepartos;

        private static readonly IServiceBase<DiaReparto> _service = ServicesObjects.ServDiaReparto;

        private static void AddDiaReparto(DiaReparto diaReparto)
        {
            bool response = _service.Add(diaReparto);
            if (!response) Console.WriteLine("Algo falló al agregar nuevo Día de Reparto a la base de datos");
        }
        private static List<DiaReparto> GetDiaRepartos()
        {
            return _service.GetAll();
        }
    }
}
