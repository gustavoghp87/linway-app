using AppServices.UseCases;
using Models;
using System.Threading.Tasks;

namespace AppServices.Interfaces
{
    public interface ILimpiarRepartoUseCase
    {
        Task<UseCaseResponse> ExecuteAsync(Reparto repartoALimpiar);
    }
}
