using InfrasServices.Services;
using linway_app.Services.Interfaces;
using Models;
using System;
using System.Collections.Generic;

namespace linway_app.Services.Delegates
{
    public static class DRecibo
    {
        public readonly static Func<Recibo, long> addReciboReturnId = AddReciboReturnId;
        public readonly static Func<Recibo, decimal> calculateTotalRecibo = CalculateTotalRecibo;   // no se está usando
        public readonly static Action<ICollection<Recibo>> deleteRecibos = DeleteRecibos;
        public readonly static Action<Recibo> editRecibo = EditRecibo;
        public readonly static Func<List<Recibo>> getRecibos = GetRecibos;

        private static readonly IServiceBase<Recibo> _service = ServicesObjects.ServRecibo;

        private static long AddReciboReturnId(Recibo recibo)
        {
            bool response = _service.Add(recibo);
            if (!response) return 0;
            return recibo.Id;
        }
        private static decimal CalculateTotalRecibo(Recibo recibo)
        {
            decimal total = 0;
            if (recibo.DetalleRecibos == null || recibo.DetalleRecibos.Count == 0) return total;
            foreach (DetalleRecibo detalle in recibo.DetalleRecibos)
            {
                total += detalle.Importe;
            }
            return total;
        }
        private static void DeleteRecibos(ICollection<Recibo> recibos)
        {
            if (recibos == null || recibos.Count == 0) return;
            bool response = _service.DeleteMany(recibos);
            if (!response) Console.WriteLine("Algo falló al eliminar los Recibos de la base de datos");
        }
        private static void EditRecibo(Recibo recibo)
        {
            bool response = _service.Edit(recibo);
            if (!response) Console.WriteLine("Algo falló al editar el Recibo de la base de datos");
        }
        private static List<Recibo> GetRecibos()
        {
            return _service.GetAll();
        }
    }
}
