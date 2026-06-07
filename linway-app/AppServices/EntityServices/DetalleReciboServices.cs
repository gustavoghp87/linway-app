using AppServices.Interfaces;
using Models;
using System.Collections.Generic;

namespace AppServices.EntityServices
{
    public class DetalleReciboServices: IDetalleReciboServices
    {
        private readonly IServicesBase<DetalleRecibo> _services;
        public DetalleReciboServices(IServicesBase<DetalleRecibo> services)
        {
            _services = services;
        }
        public void AddMany(ICollection<DetalleRecibo> detalles)
        {
            _services.AddMany(detalles);
        }
        public void DeleteMany(ICollection<DetalleRecibo> detalles)
        {
            _services.DeleteMany(detalles);
        }
    }
}
