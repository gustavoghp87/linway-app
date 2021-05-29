using linway_app.Models;
using System.Collections.Generic;

namespace linway_app.Services.Interfaces
{
    public interface IServicioVenta
    {
        bool Add(Venta venta);
        bool Delete(Venta venta);
        bool Edit(Venta venta);
        Venta Get(long id);
        List<Venta> GetAll();
    }
}