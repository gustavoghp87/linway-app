using AppServices.UseCases;
using Models;
using System.Threading.Tasks;

namespace AppServices.Interfaces
{
    public interface IEliminarProductoUseCase
    {
        Task<UseCaseResponse> ExecuteAsync(Producto productoAEliminar);
    }
}
