using Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace linway_app.Services.Interfaces
{
    public interface IReciboServices
    {
        void AddRecibo(Recibo recibo);
        void DeleteRecibos(ICollection<Recibo> recibos);
        void EditRecibo(Recibo recibo);
        Task<List<Recibo>> GetRecibosAsync();
    }
}
