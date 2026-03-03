using Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace linway_app.Services.Interfaces
{
    public interface IProdVendidoServices
    {
        void AddProdVendidos(ICollection<ProdVendido> prodVendidos);
        void DeleteProdVendido(ProdVendido prodVendido);
        void EditProdVendido(ProdVendido prodVendido);
        void EditProdVendidos(ICollection<ProdVendido> prodVendidos);
        Task<List<ProdVendido>> GetProdVendidos();
    }
}
