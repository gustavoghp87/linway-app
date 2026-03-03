using Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace linway_app.Services.Interfaces
{
    public interface IRepartoServices
    {
        void AddReparto(Reparto reparto);
        void EditReparto(Reparto reparto);
        void EditRepartos(ICollection<Reparto> repartos);
        Task<Reparto> GetRepartoPorIdAsync(long repartoId);
        Task<List<Reparto>> GetRepartosAsync();
        void UpdateReparto(Reparto reparto);
    }
}
