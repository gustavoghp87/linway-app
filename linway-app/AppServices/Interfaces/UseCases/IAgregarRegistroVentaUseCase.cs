using AppServices.UseCases;
using Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AppServices.Interfaces
{
    public interface IAgregarRegistroVentaUseCase
    {
        Task<UseCaseResponse> ExecuteAsync(ICollection<Venta> ventas, Cliente cliente, bool enviarAReparto, Reparto reparto);
    }
}
