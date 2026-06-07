using AppServices.UseCases;
using Models;
using System.Threading.Tasks;

namespace AppServices.Interfaces
{
    public interface IAgregarProdVendidoANotaDeEnvioUseCase
    {
        Task<UseCaseResponse> ExecuteAsync(NotaDeEnvio notaDeEnvio, Producto _productoAAgregar, int _cantidadAAgregar);
    }
}
