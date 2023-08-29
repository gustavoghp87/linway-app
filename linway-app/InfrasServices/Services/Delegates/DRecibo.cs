using InfrasServices.Services;
using linway_app.Services.Interfaces;
using Models;
using System;
using System.Collections.Generic;

namespace linway_app.Services.Delegates
{
    public static class DRecibo
    {
        public readonly static Predicate<Recibo> addRecibo = AddRecibo;
        public readonly static Func<Recibo, decimal> calculateTotalRecibo = CalculateTotalRecibo;   // no se está usando
        public readonly static Predicate<ICollection<Recibo>> deleteRecibos = DeleteRecibos;
        public readonly static Predicate<Recibo> editRecibo = EditRecibo;
        public readonly static Func<List<Recibo>> getRecibos = GetRecibos;

        private static readonly IServiceBase<Recibo> _service = ServicesObjects.ServRecibo;

        private static bool AddRecibo(Recibo recibo)
        {
            bool success = _service.Add(recibo);
            return success;
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
        private static bool DeleteRecibos(ICollection<Recibo> recibos)
        {
            if (recibos == null || recibos.Count == 0) return false;
            bool success = _service.DeleteMany(recibos);
            if (!success) Console.WriteLine("Algo falló al eliminar los Recibos de la base de datos");
            return success;
        }
        private static bool EditRecibo(Recibo recibo)
        {
            bool success = _service.Edit(recibo);
            if (!success) Console.WriteLine("Algo falló al editar el Recibo de la base de datos");
            return success;
        }
        private static List<Recibo> GetRecibos()
        {
            List<Recibo> recibos = _service.GetAll();
            return recibos;
        }
    }
}
