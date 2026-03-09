using Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace linway_app.Services.Interfaces
{
    public interface IProdVendidoServices
    {
        void AddProdVendido(ProdVendido prodVendido);
        void AddProdVendidos(ICollection<ProdVendido> prodVendidos);
        void DeleteProdVendido(ProdVendido prodVendido);
        void EditProdVendido(ProdVendido prodVendido);
        void EditProdVendidos(ICollection<ProdVendido> prodVendidos);
        Task<List<ProdVendido>> GetProdVendidosAsync();
    }
}
