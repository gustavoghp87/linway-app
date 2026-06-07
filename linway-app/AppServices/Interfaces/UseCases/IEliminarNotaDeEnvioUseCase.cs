using AppServices.UseCases;
using Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AppServices.Interfaces
{
    public interface IEliminarNotaDeEnvioUseCase
    {
        Task<UseCaseResponse> ExecuteAsync(ICollection<NotaDeEnvio> notas, bool eliminarDeLosRepartos, bool eliminarRegistros, bool restarDeVentas);
    }
}
