using AppServices.UseCases;
using Models;
using System.Threading.Tasks;

namespace AppServices.Interfaces
{
    public interface IAgregarPedidoUseCase
    {
        Task<UseCaseResponse> ExecuteAsync(Pedido pedido);
    }
}
