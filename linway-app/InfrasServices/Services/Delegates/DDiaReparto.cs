using InfrasServices.Services;
using linway_app.Services.Interfaces;
using Models;
using System;
using System.Collections.Generic;

namespace linway_app.Services.Delegates
{
    public static class DDiaReparto
    {
        public readonly static Predicate<DiaReparto> addDiaReparto = AddDiaReparto;
        public readonly static Func<List<DiaReparto>> getDiaRepartos = GetDiaRepartos;

        private static readonly IServiceBase<DiaReparto> _service = ServicesObjects.ServDiaReparto;

        private static bool AddDiaReparto(DiaReparto diaReparto)
        {
            bool success = _service.Add(diaReparto);
            return success;
        }
        private static List<DiaReparto> GetDiaRepartos()
        {
            List<DiaReparto> diasReparto = _service.GetAll();
            return diasReparto;
        }
    }
}
