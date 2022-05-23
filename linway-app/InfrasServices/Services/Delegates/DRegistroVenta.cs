using InfrasServices.Services;
using linway_app.Services.Interfaces;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace linway_app.Services.Delegates
{
    public static class DRegistroVenta
    {
        public readonly static Func<RegistroVenta, long> addRegistroVentaReturnId = AddRegistroVentaReturnId;
        public readonly static Action<RegistroVenta> deleteRegistroVenta = DeleteRegistroVenta;
        public readonly static Action<ICollection<RegistroVenta>> deleteRegistros = DeleteRegistros;
        public readonly static Action<RegistroVenta> editRegistroVenta = EditRegistroVenta;
        public readonly static Func<long, RegistroVenta> getRegistroVenta = GetRegistroVenta;
        public readonly static Func<List<RegistroVenta>> getRegistroVentas = GetRegistroVentas;

        private static readonly IServiceBase<RegistroVenta> _service = ServicesObjects.ServRegistroVenta;

        private static bool AddRegistroVenta(RegistroVenta registroVenta)
        {
            bool response = _service.Add(registroVenta);
            if (!response) Console.WriteLine("Algo falló al agregar Registro de Venta a base de datos");
            return response;
        }
        private static long AddRegistroVentaReturnId(RegistroVenta registroVenta)
        {
            try
            {
                bool response = AddRegistroVenta(registroVenta);
                if (!response)
                {
                    Console.WriteLine("Algo falló al agregar Registro de Venta a base de datos");
                    return 0;
                }
                return registroVenta.Id;
            }
            catch
            {
                Console.WriteLine("Algo falló al agregar Registro de Venta a base de datos");
                return 0;
            }
        }
        private static void DeleteRegistroVenta(RegistroVenta registroVenta)
        {
            bool success = _service.Delete(registroVenta);
            if (!success) Console.WriteLine("Algo falló al eliminar Registro de Venta a base de datos");
        }
        private static void DeleteRegistros(ICollection<RegistroVenta> registros)
        {
            bool success = _service.DeleteMany(registros);
            if (!success) Console.WriteLine("Algo falló al eliminar Registro de Venta a base de datos");
        }
        private static void EditRegistroVenta(RegistroVenta registroVenta)
        {
            bool response = _service.Edit(registroVenta);
            if (!response) Console.WriteLine("Algo falló al editar Registro de Venta en la base de datos");
        }
        private static RegistroVenta GetRegistroVenta(long registroVentaId)
        {
            return _service.Get(registroVentaId);
        }
        private static List<RegistroVenta> GetRegistroVentas()
        {
            return _service.GetAll();
        }
    }
}
