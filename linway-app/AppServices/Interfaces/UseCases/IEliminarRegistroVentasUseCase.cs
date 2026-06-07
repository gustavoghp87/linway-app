using AppServices.UseCases;
using Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AppServices.Interfaces
{
    public interface IEliminarRegistroVentasUseCase
    {
        Task<UseCaseResponse> ExecuteAsync(ICollection<RegistroVenta> registrosVentas);
    }
}
