using linway_app.Forms;
using linway_app.Models;
using linway_app.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace linway_app.Services.Delegates
{
    public static class DRegistroVenta
    {
        public readonly static Func<RegistroVenta, bool> addRegistroVenta = AddRegistroVenta;
        public readonly static Func<RegistroVenta, long> addRegistroVentaReturnId = AddRegistroVentaReturnId;
        public readonly static Action<RegistroVenta> deleteRegistroVenta = DeleteRegistroVenta;
        public readonly static Action<RegistroVenta> editRegistroVenta = EditRegistroVenta;
        public readonly static Func<long, RegistroVenta> getRegistroVenta = GetRegistroVenta;
        public readonly static Func<List<RegistroVenta>> getRegistroVentas = GetRegistroVentas;

        private static readonly IServiceBase<RegistroVenta> _service = Form1._servRegistroVenta;
        private static bool AddRegistroVenta(RegistroVenta registroVenta)
        {
            bool response = _service.Add(registroVenta);
            if (!response) MessageBox.Show("Algo falló al agregar Registro de Venta a base de datos");
            return response;
        }
        private static long AddRegistroVentaReturnId(RegistroVenta registroVenta)
        {
            try
            {
                bool response = AddRegistroVenta(registroVenta);
                if (!response)
                {
                    MessageBox.Show("Algo falló al agregar Registro de Venta a base de datos");
                    return 0;
                }
                var lst = GetRegistroVentas();
                return lst.Last().Id;
            }
            catch
            {
                MessageBox.Show("Algo falló al agregar Registro de Venta a base de datos");
                return 0;
            }
        }
        private static void DeleteRegistroVenta(RegistroVenta registroVenta)
        {
            bool success = _service.Delete(registroVenta);
            if (!success) MessageBox.Show("Algo falló al eliminar Registro de Venta a base de datos");
        }
        private static void EditRegistroVenta(RegistroVenta registroVenta)
        {
            bool response = _service.Edit(registroVenta);
            if (!response) MessageBox.Show("Algo falló al editar Registro de Venta en la base de datos");
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
