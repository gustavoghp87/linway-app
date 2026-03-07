using linway_app.Services.Interfaces;
using Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace linway_app.Services.FormServices
{
    public class VentaServices: IVentaServices
    {
        private readonly IServicesBase<Venta> _services;
        public VentaServices(IServicesBase<Venta> services)
        {
            _services = services;
        }
        public void AddVentas(ICollection<Venta> ventas)
        {
            _services.AddMany(ventas);
        }
        public void DeleteVentas(ICollection<Venta> ventas)
        {
            _services.DeleteMany(ventas);
        }
        public void EditVentas(ICollection<Venta> ventas)
        {
            _services.EditMany(ventas);
        }
        public async Task<List<Venta>> GetVentasAsync()
        {
            List<Venta> ventas = await _services.GetAllAsync();
            return ventas;
        }
        public async Task UpdateVentasDesdeProdVendidosAsync(ICollection<ProdVendido> prodVendidos, bool addingUp)
        {
            var ventasAAgregar = new List<Venta>();
            var ventasAEditar = new List<Venta>();
            List<Venta> lstVentas = await GetVentasAsync();
            foreach (ProdVendido prodVendido in prodVendidos.Where(x => ProductoServices.IsProducto(x.Producto)))
            {
                bool exists = false;
                foreach (var venta in lstVentas)
                {
                    if (venta.ProductoId == prodVendido.ProductoId)
                    {
                        exists = true;
                        venta.Cantidad = addingUp
                            ? venta.Cantidad + prodVendido.Cantidad
                            : venta.Cantidad - prodVendido.Cantidad;
                        ventasAEditar.Add(venta);
                        break;
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
            if (ventasAAgregar.Count > 0)
            {
                _services.AddMany(ventasAAgregar);
            }
            if (ventasAEditar.Count > 0)
            {
                _services.EditMany(ventasAEditar);
            }
        }
    }
}
