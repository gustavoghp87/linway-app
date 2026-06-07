using Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AppServices.Interfaces
{
    public interface IReciboServices
    {
        void Add(Recibo recibo);
        void DeleteMany(ICollection<Recibo> recibos);
        void Edit(Recibo recibo);
        Task<List<Recibo>> GetAllAsync();
    }
}
