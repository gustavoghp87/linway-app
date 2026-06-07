using Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AppServices.Interfaces
{
    public interface IRegistroVentaServices
    {
        void Add(RegistroVenta registroVenta);
        void Delete(RegistroVenta registro);
        void DeleteMany(ICollection<RegistroVenta> registros);
        Task<RegistroVenta> GetPorIdAsync(long registroVentaId);
        Task<List<RegistroVenta>> GetAllAsync();
    }
}
