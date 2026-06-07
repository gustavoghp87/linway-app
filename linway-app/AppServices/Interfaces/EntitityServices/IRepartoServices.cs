using Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AppServices.Interfaces
{
    public interface IRepartoServices
    {
        void Add(Reparto reparto, ICollection<DiaReparto> diaRepartos);
        void Delete(Reparto reparto);
        Task<Reparto> GetPorIdAsync(long repartoId);
    }
}
