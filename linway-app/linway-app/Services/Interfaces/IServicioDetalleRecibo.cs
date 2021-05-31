using linway_app.Models;
using System.Collections.Generic;

namespace linway_app.Services.Interfaces
{
    public interface IServicioDetalleRecibo
    {
        bool Add(DetalleRecibo detalleRecibo);
        bool Delete(DetalleRecibo detalleRecibo);
        bool Edit(DetalleRecibo detalleRecibo);
        DetalleRecibo Get(long id);
        List<DetalleRecibo> GetAll();
    }
}