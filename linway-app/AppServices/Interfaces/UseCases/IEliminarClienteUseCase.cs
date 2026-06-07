using AppServices.UseCases;
using Models;
using System.Threading.Tasks;

namespace AppServices.Interfaces
{
    public interface IEliminarClienteUseCase
    {
        Task<UseCaseResponse> ExecuteAsync(Cliente cliente);
    }
}
