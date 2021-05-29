using linway_app.Models;
using System.Collections.Generic;

namespace linway_app.Services
{
    public interface IServicioPedido
    {
        bool Add(Reparto reparto);
        bool Delete(Reparto reparto);
        bool Edit(Reparto reparto);
        Reparto Get(long id);
        List<Reparto> GetAll();
    }
}