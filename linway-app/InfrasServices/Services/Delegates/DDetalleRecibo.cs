using InfrasServices.Services;
using linway_app.Services.Interfaces;
using Models;
using System;
using System.Collections.Generic;

namespace linway_app.Services.Delegates
{
    public class DDetalleRecibo
    {
        public readonly static Action<DetalleRecibo> addDetalleRecibo = AddDetalleRecibo;
        //public readonly static Action<DetalleRecibo> deleteDetalleRecibo = DeleteDetalleRecibo;
        public readonly static Action<ICollection<DetalleRecibo>> deleteDetalles = DeleteDetalles;
        public readonly static Func<List<DetalleRecibo>> getDetalleRecibo = GetDetalleRecibos;

        private static readonly IServiceBase<DetalleRecibo> _service = ServicesObjects.ServDetalleRecibo;
        private static void AddDetalleRecibo(DetalleRecibo detalleRecibo)
        {
            bool response = _service.Add(detalleRecibo);
            if (!response) Console.WriteLine("Algo falló al guardar el Detalle de Recibo en la base de datos");
        }
        //private static void DeleteDetalleRecibo(DetalleRecibo detalleRecibo)
        //{
        //    bool response = _service.Delete(detalleRecibo);
        //    if (!response) Console.WriteLine("Algo falló al eliminar el Detalle de Recibo en la base de datos");
        //}
        private static void DeleteDetalles(ICollection<DetalleRecibo> detalles)
        {
            bool response = _service.DeleteMany(detalles);
            if (!response) Console.WriteLine("Algo falló al eliminar los Detalles de Recibo en la base de datos");
        }
        private static List<DetalleRecibo> GetDetalleRecibos()
        {
            return _service.GetAll();
        }
    }
}
