using AppServices.UseCases;
using Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AppServices.Interfaces
{
    public interface IEliminarRecibosUseCase
    {
        Task<UseCaseResponse> ExecuteAsync(List<Recibo> recibos, ICollection<DetalleRecibo> detalles);
    }
}
