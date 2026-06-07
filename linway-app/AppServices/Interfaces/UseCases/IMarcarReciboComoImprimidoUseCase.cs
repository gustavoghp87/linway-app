using AppServices.UseCases;
using Models;
using System.Threading.Tasks;

namespace AppServices.Interfaces
{
    public interface IMarcarReciboComoImprimidoUseCase
    {
        Task<UseCaseResponse> ExecuteAsync(Recibo recibo);
    }
}
