using InfrasServices.Services;
using linway_app.Services.Interfaces;
using Models;
using System;
using System.Collections.Generic;

namespace linway_app.Services.Delegates
{
    public static class DVentas
    {
        public readonly static Action<Venta> addVenta = AddVenta;
        public readonly static Action<Venta> deleteVenta = DeleteVenta;
        public readonly static Action<Venta> editVenta = EditVenta;
        public readonly static Func<long, Venta> getVenta = GetVenta;
        public readonly static Func<List<Venta>> getVentas = GetVentas;

        private static readonly IServiceBase<Venta> _service = ServicesObjects.ServVenta;
        private static void AddVenta(Venta venta)
        {
            bool response = _service.Add(venta);
            if (!response) Console.WriteLine("Algo falló al agregar Venta a la base de datos");
        }
        private static void DeleteVenta(Venta venta)
        {
            bool response = _service.Delete(venta);
            if (!response) Console.WriteLine("Algo falló al eliminar Venta de la base de datos");
        }
        private static void EditVenta(Venta venta)
        {
            bool response = _service.Edit(venta);
            if (!response) Console.WriteLine("Algo falló al editar Venta en la base de datos");
        }
        private static Venta GetVenta(long ventaId)
        {
            return _service.Get(ventaId);
        }
        private static List<Venta> GetVentas()
        {
            return _service.GetAll();
        }
    }
}
