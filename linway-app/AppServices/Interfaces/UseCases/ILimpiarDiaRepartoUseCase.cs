using AppServices.UseCases;
using Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AppServices.Interfaces
{
    public interface ILimpiarDiaRepartoUseCase
    {
        Task<UseCaseResponse> ExecuteAsync(ICollection<Reparto> repartosALimpiar);
    }
}
