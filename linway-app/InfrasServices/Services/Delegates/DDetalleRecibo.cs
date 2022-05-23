using InfrasServices.Services;
using linway_app.Services.Interfaces;
using Models;
using System;
using System.Collections.Generic;

namespace linway_app.Services.Delegates
{
    public class DDetalleRecibo
    {
        public readonly static Action<ICollection<DetalleRecibo>> addDetalles = AddDetalles;
        public readonly static Action<ICollection<DetalleRecibo>> deleteDetalles = DeleteDetalles;

        private static readonly IServiceBase<DetalleRecibo> _service = ServicesObjects.ServDetalleRecibo;

        private static void AddDetalles(ICollection<DetalleRecibo> detalles)
        {
            if (detalles == null || detalles.Count == 0) return;
            bool response = _service.AddMany(detalles);
            if (!response) Console.WriteLine("Algo falló al guardar los Detalles de Recibo en la base de datos");
        }
        private static void DeleteDetalles(ICollection<DetalleRecibo> detalles)
        {
            bool response = _service.DeleteMany(detalles);
            if (!response) Console.WriteLine("Algo falló al eliminar los Detalles de Recibo en la base de datos");
        }
    }
}
