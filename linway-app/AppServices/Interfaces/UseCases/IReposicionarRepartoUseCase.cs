using AppServices.UseCases;
using Models;
using System.Threading.Tasks;

namespace AppServices.Interfaces
{
    public interface IReposicionarRepartoUseCase
    {
        Task<UseCaseResponse> ExecuteAsync(Reparto reparto, Pedido pedido1, Pedido pedido2);
    }
}
