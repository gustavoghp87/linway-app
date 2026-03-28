using Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace linway_app.Services.Interfaces
{
    public interface IRegistroVentaServices
    {
        void Add(RegistroVenta registroVenta);
        void DeleteMany(ICollection<RegistroVenta> registros);
        Task<RegistroVenta> GetPorIdAsync(long registroVentaId);
        Task<List<RegistroVenta>> GetAllAsync();
    }
}
