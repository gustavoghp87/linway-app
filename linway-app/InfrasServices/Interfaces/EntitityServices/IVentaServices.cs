using Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace linway_app.Services.Interfaces
{
    public interface IVentaServices
    {
        void Delete(Venta venta);
        void DeleteMany(ICollection<Venta> ventas);
        Task<List<Venta>> GetAllAsync();
        Task RestarDesdeProdVendidosAsync(ProdVendido prodVendido);
        Task RestarDesdeProdVendidosAsync(ICollection<ProdVendido> prodVendidos);
        Task SumarDesdeProdVendidosAsync(ProdVendido prodVendido);
        Task SumarDesdeProdVendidosAsync(ICollection<ProdVendido> prodVendidos);
    }
}
