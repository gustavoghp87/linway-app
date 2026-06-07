using Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AppServices.Interfaces
{
    public interface IProdVendidoServices
    {
        void Add(ProdVendido prodVendido);
        void AddMany(ICollection<ProdVendido> prodVendidos);
        void Delete(ProdVendido prodVendido);
        void DeleteMany(ICollection<ProdVendido> prodVendidos);
        void EditOrDeleteMany(List<ProdVendido> prodVendidos);
        void Edit(ProdVendido prodVendido);
        void EditMany(ICollection<ProdVendido> prodVendidos);
        Task<List<ProdVendido>> GetAllAsync();
    }
}
