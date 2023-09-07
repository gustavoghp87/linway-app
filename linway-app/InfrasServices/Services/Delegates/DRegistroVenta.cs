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
        public readonly static Predicate<RegistroVenta> addRegistroVenta = AddRegistroVenta;
        public readonly static Predicate<RegistroVenta> deleteRegistroVenta = DeleteRegistroVenta;
        public readonly static Predicate<ICollection<RegistroVenta>> deleteRegistros = DeleteRegistros;
        public readonly static Predicate<RegistroVenta> editRegistroVenta = EditRegistroVenta;
        public readonly static Func<long, RegistroVenta> getRegistroVenta = GetRegistroVenta;
        public readonly static Func<List<RegistroVenta>> getRegistroVentas = GetRegistroVentas;

        private static readonly IServiceBase<RegistroVenta> _service = ServicesObjects.ServRegistroVenta;

        private static bool AddRegistroVenta(RegistroVenta registroVenta)
        {
            bool success = _service.Add(registroVenta);
            return success;
        }
        private static bool DeleteRegistroVenta(RegistroVenta registroVenta)
        {
            bool success = _service.Delete(registroVenta);
            return success;
        }
        private static bool DeleteRegistros(ICollection<RegistroVenta> registros)
        {
            bool success = _service.DeleteMany(registros);
            return success;
        }
        private static bool EditRegistroVenta(RegistroVenta registroVenta)
        {
            bool success = _service.Edit(registroVenta);
            return success;
        }
        private static RegistroVenta GetRegistroVenta(long registroVentaId)
        {
            RegistroVenta registoVenta = _service.Get(registroVentaId);
            return registoVenta;
        }
        private static List<RegistroVenta> GetRegistroVentas()
        {
            List<RegistroVenta> registros = _service.GetAll();
            return registros;
        }
    }
}
