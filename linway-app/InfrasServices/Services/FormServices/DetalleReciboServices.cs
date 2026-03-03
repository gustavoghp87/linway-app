using linway_app.Services.Interfaces;
using Models;
using System.Collections.Generic;

namespace linway_app.Services.FormServices
{
    public class DetalleReciboServices: IDetalleReciboServices
    {
        private readonly IServicesBase<DetalleRecibo> _services;
        public DetalleReciboServices(IServicesBase<DetalleRecibo> services)
        {
            _services = services;
        }
        public void AddDetalles(ICollection<DetalleRecibo> detalles)
        {
            _services.AddMany(detalles);
        }
        public void DeleteDetalles(ICollection<DetalleRecibo> detalles)
        {
            _services.DeleteMany(detalles);
        }
    }
}
