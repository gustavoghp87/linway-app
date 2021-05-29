using linway_app.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace linway_app.Services
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