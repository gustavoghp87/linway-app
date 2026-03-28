using Models;
using System.Collections.Generic;

namespace linway_app.Services.Interfaces
{
    public interface IDetalleReciboServices
    {
        void AddMany(ICollection<DetalleRecibo> detalles);
        void DeleteMany(ICollection<DetalleRecibo> detalles);
    }
}
