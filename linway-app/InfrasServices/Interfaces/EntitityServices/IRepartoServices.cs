using Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace linway_app.Services.Interfaces
{
    public interface IRepartoServices
    {
        void Add(Reparto reparto, List<DiaReparto> diaRepartos);
        void Delete(Reparto reparto);
        void Edit(Reparto reparto);
        void EditMany(ICollection<Reparto> repartos);
        Task<Reparto> GetPorIdAsync(long repartoId);
        Task<List<Reparto>> GetAllAsync();
    }
}
