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
        public void DeleteMany(ICollection<Venta> ventas)
        {
            _services.DeleteMany(ventas);
        }
        public async Task<List<Venta>> GetAllAsync()
        {
            List<Venta> ventas = await _services.GetAllAsync();
            return ventas;
        }
        public async Task RestarDesdeProdVendidosAsync(ProdVendido prodVendido)
        {
            await RestarDesdeProdVendidosAsync(new List<ProdVendido> { prodVendido });
        }
        public async Task RestarDesdeProdVendidosAsync(ICollection<ProdVendido> prodVendidos)
        {
            var ventasAEditar = new List<Venta>();
            List<Venta> lstVentas = await GetAllAsync();
            foreach (ProdVendido prodVendido in prodVendidos.Where(x => ProductoServices.IsProducto(x.Producto)))
            {
                var venta = lstVentas.FirstOrDefault(x => x.ProductoId == prodVendido.ProductoId);
                if (venta != null)
                {
                    venta.Cantidad = (venta.Cantidad - prodVendido.Cantidad >= 0) ? venta.Cantidad - prodVendido.Cantidad : 0;  // no puede ser negativa
                    ventasAEditar.Add(venta);
                }
            }
            if (ventasAEditar.Count > 0)
            {
                _services.EditMany(ventasAEditar);
            }
        }
        public async Task SumarDesdeProdVendidosAsync(ProdVendido prodVendido)
        {
            await SumarDesdeProdVendidosAsync(new List<ProdVendido> { prodVendido });
        }
        public async Task SumarDesdeProdVendidosAsync(ICollection<ProdVendido> prodVendidos)
        {
            var ventasAAgregar = new List<Venta>();
            var ventasAEditar = new List<Venta>();
            List<Venta> lstVentas = await GetAllAsync();
            foreach (ProdVendido prodVendido in prodVendidos.Where(x => ProductoServices.IsProducto(x.Producto)))
            {
                var venta = lstVentas.FirstOrDefault(x => x.ProductoId == prodVendido.ProductoId);
                if (venta != null)
                {
                    venta.Cantidad = venta.Cantidad + prodVendido.Cantidad;
                    ventasAEditar.Add(venta);
                }
                else  // no existe, se agrega
                {
                    var nuevaVenta = new Venta
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
