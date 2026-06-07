using AppServices.UseCases;
using Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AppServices.Interfaces
{
    public interface IEliminarVentasUseCase
    {
        Task<UseCaseResponse> ExecuteAsync(ICollection<Venta> ventas);
    }
}
