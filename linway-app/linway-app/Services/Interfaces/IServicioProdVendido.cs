using linway_app.Models;
using System.Collections.Generic;

namespace linway_app.Services.Interfaces
{
    public interface IServicioProdVendido
    {
        bool Add(ProdVendido producto);
        bool Delete(ProdVendido producto);
        bool Edit(ProdVendido producto);
        ProdVendido Get(long id);
        List<ProdVendido> GetAll();
    }
}