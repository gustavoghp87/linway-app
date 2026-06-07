using Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AppServices.Interfaces
{
    public interface IProductoServices
    {
        void Add(Producto producto);
        void Delete(Producto producto);
        void Edit(Producto producto);
        Task<Producto> GetPorIdAsync(long productId);
        Task<Producto> GetPorNombreAsync(string nombre);
        Task<List<Producto>> GetAllAsync();
    }
}
