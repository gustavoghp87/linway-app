using InfrasServices.Services;
using linway_app.Services.Interfaces;
using Models;
using System;
using System.Collections.Generic;

namespace linway_app.Services.Delegates
{
    public static class DRecibo
    {
        public readonly static Func<Recibo, bool> addRecibo = AddRecibo;
        public readonly static Func<Recibo, long> addReciboReturnId = AddReciboReturnId;
        public readonly static Func<Recibo, decimal> calculateTotalRecibo = CalculateTotalRecibo;
        public readonly static Action<Recibo> deleteRecibo = DeleteRecibo;
        public readonly static Action<Recibo> editRecibo = EditRecibo;
        public readonly static Func<long, Recibo> getRecibo = GetRecibo;
        public readonly static Func<List<Recibo>> getRecibos = GetRecibos;

        private static readonly IServiceBase<Recibo> _service = ServicesObjects.ServRecibo;
        private static bool AddRecibo(Recibo recibo)
        {
            bool response = _service.Add(recibo);
            if (!response) Console.WriteLine("Algo falló al guardar el Recibo en la base de datos");
            return response;
        }
        private static long AddReciboReturnId(Recibo recibo)
        {
            bool response = AddRecibo(recibo);
            if (!response) return 0;
            var response1 = GetRecibos();
            var last = response1[response1.Count - 1];
            return last.Id;
        }
        private static decimal CalculateTotalRecibo(Recibo recibo)
        {
            decimal subTo = 0;
            if (recibo.DetalleRecibos != null && recibo.DetalleRecibos.Count != 0)
                foreach (DetalleRecibo detalle in recibo.DetalleRecibos)
                {
                    subTo += detalle.Importe;
                }
            return subTo;
        }
        private static void DeleteRecibo(Recibo recibo)
        {
            bool response = _service.Delete(recibo);
            if (!response) Console.WriteLine("Algo falló al eliminar el Recibo de la base de datos");
        }
        private static void EditRecibo(Recibo recibo)
        {
            bool response = _service.Edit(recibo);
            if (!response) Console.WriteLine("Algo falló al editar el Recibo de la base de datos");
        }
        private static Recibo GetRecibo(long reciboId)
        {
            return _service.Get(reciboId);
        }
        private static List<Recibo> GetRecibos()
        {
            return _service.GetAll();
        }
    }
}
