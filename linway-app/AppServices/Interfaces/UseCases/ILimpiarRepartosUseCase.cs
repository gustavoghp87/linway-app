using AppServices.UseCases;
using Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AppServices.Interfaces
{
    public interface ILimpiarRepartosUseCase
    {
        Task<UseCaseResponse> ExecuteAsync(ICollection<DiaReparto> diaRepartos);
    }
}
