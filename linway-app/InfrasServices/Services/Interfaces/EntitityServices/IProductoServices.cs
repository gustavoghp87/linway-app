using Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace linway_app.Services.Interfaces
{
    public interface IProductoServices
    {
        void AddProducto(Producto producto);
        void DeleteProducto(Producto producto);
        void EditProducto(Producto producto);
        Task<Producto> GetProductoPorIdAsync(long productId);
        Task<Producto> GetProductoPorNombreAsync(string nombre);
        Task<Producto> GetProductoPorNombreExactoAsync(string nombre);
        Task<List<Producto>> GetProductosAsync();
    }
}
