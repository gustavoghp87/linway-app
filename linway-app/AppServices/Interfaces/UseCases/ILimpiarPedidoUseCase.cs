using AppServices.UseCases;
using Models;
using System.Threading.Tasks;

namespace AppServices.Interfaces
{
    public interface ILimpiarPedidoUseCase
    {
        Task<UseCaseResponse> ExecuteAsync(Pedido pedidoAEditar);
    }
}
