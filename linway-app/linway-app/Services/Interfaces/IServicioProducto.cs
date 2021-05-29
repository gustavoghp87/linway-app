using linway_app.Models;
using System.Collections.Generic;

namespace linway_app.Services.Interfaces
{
    public interface IServicioProducto
    {
        List<Producto> GetAll();
        Producto Get(long id);
        bool Add(Producto producto);
        bool Edit(Producto producto);
        bool Delete(Producto producto);
    }
}