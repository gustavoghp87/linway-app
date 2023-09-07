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
        public readonly static Predicate<ICollection<Venta>> addVentas = AddVentas;
        public readonly static Predicate<ICollection<Venta>> deleteVentas = DeleteVentas;
        public readonly static Predicate<ICollection<Venta>> editVentas = EditVentas;
        public readonly static Func<List<Venta>> getVentas = GetVentas;
        public readonly static Func<ICollection<ProdVendido>, bool, bool> updateVentasDesdeProdVendidos = UpdateVentasDesdeProdVendidos;

        private static readonly IServiceBase<Venta> _service = ServicesObjects.ServVenta;

        private static bool AddVentas(ICollection<Venta> ventas)
        {
            if (ventas == null || ventas.Count == 0) return false;
            bool success = _service.AddMany(ventas);
            return success;
        }
        private static bool DeleteVentas(ICollection<Venta> ventas)
        {
            bool success = _service.DeleteMany(ventas);
            return success;
        }
        private static bool EditVentas(ICollection<Venta> ventas)
        {
            bool succcess = _service.EditMany(ventas);
            return succcess;
        }
        private static List<Venta> GetVentas()
        {
            List<Venta> ventas = _service.GetAll() ?? new List<Venta>();
            return ventas;
        }
        private static bool UpdateVentasDesdeProdVendidos(ICollection<ProdVendido> prodVendidos, bool addingUp)
        {
            if (prodVendidos == null) return false;
            var ventasAAgregar = new List<Venta>();
            var ventasAEditar = new List<Venta>();
            List<Venta> lstVentas = getVentas();
            List<Producto> productos = getProductos().ToList();
            if (lstVentas == null || productos == null) return false;
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
            bool success = true;
            if (ventasAAgregar.Count > 0)
            {
                bool successAV = AddVentas(ventasAAgregar);
                if (!successAV)
                {
                    success = false;
                }
            }
            if (ventasAEditar.Count > 0)
            {
                bool successEV = EditVentas(ventasAEditar);
                if (!successEV)
                {
                    success = false;
                }
            }
            return success;
        }
    }
}
