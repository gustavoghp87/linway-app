using AppServices.UseCases;
using Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AppServices.Interfaces
{
    public interface IAgregarReciboUseCase
    {
        Task<UseCaseResponse> ExecuteAsync(Cliente cliente, ICollection<DetalleRecibo> detalles);
    }
}
