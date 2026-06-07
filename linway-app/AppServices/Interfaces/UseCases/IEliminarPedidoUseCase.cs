using AppServices.UseCases;
using Models;
using System.Threading.Tasks;

namespace AppServices.Interfaces
{
    public interface IEliminarPedidoUseCase
    {
        Task<UseCaseResponse> ExecuteAsync(Pedido pedidoAEliminar);
    }
}
