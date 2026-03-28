using Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace linway_app.Services.Interfaces
{
    public interface IVentaServices
    {
        void DeleteMany(ICollection<Venta> ventas);
        Task<List<Venta>> GetAllAsync();
        Task UpdateDesdeProdVendidosAsync(ICollection<ProdVendido> prodVendidos, bool addingUp);
    }
}
