using linway_app.Models;
using System.Collections.Generic;

namespace linway_app.Services
{
    public interface IServicioCliente
    {
        List<Cliente> GetAll();
        Cliente Get(long id);
        bool Add(Cliente c);
        bool Delete(Cliente c);
        bool Edit(Cliente c);
    }
}