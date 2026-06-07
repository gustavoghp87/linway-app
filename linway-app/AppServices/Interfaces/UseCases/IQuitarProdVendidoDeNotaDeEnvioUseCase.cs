using AppServices.UseCases;
using Models;
using System.Threading.Tasks;

namespace AppServices.Interfaces
{
    public interface IQuitarProdVendidoDeNotaDeEnvioUseCase
    {
        Task<UseCaseResponse> ExecuteAsync(NotaDeEnvio notaDeEnvio, ProdVendido prodVendido, bool restarDeVentas);
    }
}
