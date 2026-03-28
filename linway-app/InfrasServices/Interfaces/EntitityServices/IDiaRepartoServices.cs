using Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace linway_app.Services.Interfaces
{
    public interface IDiaRepartoServices
    {
        void Add(DiaReparto diaReparto);
        Task<List<DiaReparto>> GetAllAsync();
    }
}
