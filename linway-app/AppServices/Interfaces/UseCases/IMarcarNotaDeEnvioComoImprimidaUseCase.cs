using AppServices.UseCases;
using Models;
using System.Threading.Tasks;

namespace AppServices.Interfaces
{
    public interface IMarcarNotaDeEnvioComoImprimidaUseCase
    {
        Task<UseCaseResponse> ExecuteAsync(NotaDeEnvio notaDeEnvio);
    }
}
