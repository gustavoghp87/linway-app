using linway_app.Forms;
using linway_app.Models;
using linway_app.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace linway_app.Services.Delegates
{
    public static class DVentas
    {
        public readonly static Action<Venta> addVenta = AddVenta;
        public readonly static Action<Venta> deleteVenta = DeleteVenta;
        public readonly static Action<Venta> editVenta = EditVenta;
        public readonly static Func<long, Venta> getVenta = GetVenta;
        public readonly static Func<List<Venta>> getVentas = GetVentas;

        private static readonly IServiceBase<Venta> _service = Form1._servVenta;
        private static void AddVenta(Venta venta)
        {
            bool response = _service.Add(venta);
            if (!response) MessageBox.Show("Algo falló al agregar Venta a la base de datos");
        }
        private static void DeleteVenta(Venta venta)
        {
            bool response = _service.Delete(venta);
            if (!response) MessageBox.Show("Algo falló al eliminar Venta de la base de datos");
        }
        private static void EditVenta(Venta venta)
        {
            bool response = _service.Edit(venta);
            if (!response) MessageBox.Show("Algo falló al editar Venta en la base de datos");
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
