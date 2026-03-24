using Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace linway_app.Services.Interfaces
{
    public interface IRepartoServices
    {
        void AddReparto(Reparto reparto, List<DiaReparto> diaRepartos);
        void EditReparto(Reparto reparto);
        void EditRepartos(ICollection<Reparto> repartos);
        Task<Reparto> GetRepartoPorIdAsync(long repartoId);
        Task<List<Reparto>> GetRepartosAsync();
    }
}
