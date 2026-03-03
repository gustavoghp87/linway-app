using Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace linway_app.Services.Interfaces
{
    public interface IRegistroVentaServices
    {
        void AddRegistroVenta(RegistroVenta registroVenta);
        void DeleteRegistros(ICollection<RegistroVenta> registros);
        void DeleteRegistroVenta(RegistroVenta registroVenta);
        void EditRegistroVenta(RegistroVenta registroVenta);
        Task<RegistroVenta> GetRegistroVentaPorIdAsync(long registroVentaId);
        Task<List<RegistroVenta>> GetRegistroVentasAsync();
    }
}
