using Models;
using System.Collections.Generic;

namespace linway_app.Services.Interfaces
{
    public interface IDetalleReciboServices
    {
        void AddDetalles(ICollection<DetalleRecibo> detalles);
        void DeleteDetalles(ICollection<DetalleRecibo> detalles);
    }
}
