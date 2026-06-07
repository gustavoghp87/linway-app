using AppServices.UseCases;
using Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AppServices.Interfaces
{
    public interface IAgregarRepartoUseCase
    {
        Task<UseCaseResponse> ExecuteAsync(Reparto nuevoReparto, ICollection<DiaReparto> diaRepartos);
    }
}
