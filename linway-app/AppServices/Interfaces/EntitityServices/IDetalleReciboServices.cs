using Models;
using System.Collections.Generic;

namespace AppServices.Interfaces
{
    public interface IDetalleReciboServices
    {
        void AddMany(ICollection<DetalleRecibo> detalles);
        void DeleteMany(ICollection<DetalleRecibo> detalles);
    }
}
