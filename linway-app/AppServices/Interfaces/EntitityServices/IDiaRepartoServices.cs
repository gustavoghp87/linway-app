using Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AppServices.Interfaces
{
    public interface IDiaRepartoServices
    {
        Task<List<DiaReparto>> GetAllAsync();
    }
}
