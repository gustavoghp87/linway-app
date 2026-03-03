using linway_app.Services.Interfaces;
using Models;
using System.Collections.Generic;
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
            if (ventas == null || ventas.Count == 0)
            {
                return;
            }
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
            List<Venta> ventas = await _services.GetAllAsync() ?? new List<Venta>();
            return ventas;
        }
    }
}
