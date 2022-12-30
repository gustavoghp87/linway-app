using InfrasServices.Services;
using linway_app.Services.Interfaces;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using static linway_app.Services.Delegates.DProducto;

namespace linway_app.Services.Delegates
{
    public static class DVentas
    {
        public readonly static Action<ICollection<Venta>> addVentas = AddVentas;
        public readonly static Action<ICollection<Venta>> deleteVentas = DeleteVentas;
        public readonly static Action<ICollection<Venta>> editVentas = EditVentas;
        public readonly static Func<List<Venta>> getVentas = GetVentas;
        public readonly static Action<ICollection<ProdVendido>, bool> updateVentasDesdeProdVendidos = UpdateVentasDesdeProdVendidos;

        private static readonly IServiceBase<Venta> _service = ServicesObjects.ServVenta;

        private static void AddVentas(ICollection<Venta> ventas)
        {
            if (ventas == null || ventas.Count == 0) return;
            bool response = _service.AddMany(ventas);
            if (!response) Console.WriteLine("Algo falló al agregar Ventas a la base de datos");
        }
        private static void DeleteVentas(ICollection<Venta> ventas)
        {
            bool response = _service.DeleteMany(ventas);
            if (!response) Console.WriteLine("Algo falló al eliminar Ventas de la base de datos");
        }
        private static void EditVentas(ICollection<Venta> ventas)
        {
            bool response = _service.EditMany(ventas);
            if (!response) Console.WriteLine("Algo falló al editar Ventas en la base de datos");
        }
        private static List<Venta> GetVentas()
        {
            return _service.GetAll() ?? new List<Venta>();
        }
        private static void UpdateVentasDesdeProdVendidos(ICollection<ProdVendido> prodVendidos, bool addingUp)
        {
            if (prodVendidos == null) return;
            var ventasAAgregar = new List<Venta>();
            var ventasAEditar = new List<Venta>();
            List<Venta> lstVentas = getVentas();
            List<Producto> productos = getProductos().ToList();
            if (lstVentas == null || productos == null) return;

            foreach (ProdVendido prodVendido in prodVendidos)
            {
                Producto producto = productos.Find(x => x.Id == prodVendido.ProductoId);
                if (producto == null || !esProducto(producto)) continue;
                bool exists = false;
                foreach (var venta in lstVentas)
                {
                    if (exists) continue;
                    if (venta.ProductoId == prodVendido.ProductoId)
                    {
                        exists = true;
                        venta.Cantidad = addingUp ? venta.Cantidad + prodVendido.Cantidad : venta.Cantidad - prodVendido.Cantidad;
                        ventasAEditar.Add(venta);
                    }
                }
                if (!exists && addingUp)
                {
                    Venta nuevaVenta = new Venta
                    {
                        ProductoId = prodVendido.ProductoId,
                        Cantidad = prodVendido.Cantidad
                    };
                    ventasAAgregar.Add(nuevaVenta);
                }
            }
            if (ventasAAgregar.Count > 0) AddVentas(ventasAAgregar);
            if (ventasAEditar.Count > 0) EditVentas(ventasAEditar);
        }
    }
}
