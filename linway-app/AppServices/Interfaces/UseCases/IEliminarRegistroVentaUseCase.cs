using AppServices.UseCases;
using Models;
using System.Threading.Tasks;

namespace AppServices.Interfaces
{
    public interface IEliminarRegistroVentaUseCase
    {
        Task<UseCaseResponse> ExecuteAsync(RegistroVenta registroVenta, bool restarDeVentas);
    }
}
