using InfrasServices.Services;
using linway_app.Services.Interfaces;
using Models;
using System;
using System.Collections.Generic;

namespace linway_app.Services.Delegates
{
    public class DDetalleRecibo
    {
        public readonly static Predicate<ICollection<DetalleRecibo>> addDetalles = AddDetalles;
        public readonly static Predicate<ICollection<DetalleRecibo>> deleteDetalles = DeleteDetalles;

        private static readonly IServiceBase<DetalleRecibo> _service = ServicesObjects.ServDetalleRecibo;

        private static bool AddDetalles(ICollection<DetalleRecibo> detalles)
        {
            if (detalles == null || detalles.Count == 0) return false;
            bool success = _service.AddMany(detalles);
            return success;
        }
        private static bool DeleteDetalles(ICollection<DetalleRecibo> detalles)
        {
            bool success = _service.DeleteMany(detalles);
            return success;
        }
    }
}
