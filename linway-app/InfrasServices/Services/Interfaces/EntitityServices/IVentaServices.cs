using Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace linway_app.Services.Interfaces
{
    public interface IVentaServices
    {
        void AddVentas(ICollection<Venta> ventas);
        void DeleteVentas(ICollection<Venta> ventas);
        void EditVentas(ICollection<Venta> ventas);
        Task<List<Venta>> GetVentasAsync();
        Task UpdateVentasDesdeProdVendidosAsync(ICollection<ProdVendido> prodVendidos, bool addingUp);
    }
}
