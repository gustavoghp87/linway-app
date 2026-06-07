using AppServices.UseCases;
using Models;
using System.Threading.Tasks;

namespace AppServices.Interfaces
{
    public interface IEliminarRepartoUseCase
    {
        Task<UseCaseResponse> ExecuteAsync(Reparto reparto);
    }
}
