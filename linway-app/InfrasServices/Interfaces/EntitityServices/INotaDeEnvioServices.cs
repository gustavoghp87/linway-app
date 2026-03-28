using Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace linway_app.Services.Interfaces
{
    public interface INotaDeEnvioServices
    {
        void Add(NotaDeEnvio notaDeEnvio);
        void DeleteMany(ICollection<NotaDeEnvio> notas);
        void Edit(NotaDeEnvio notaDeEnvio);
        Task<NotaDeEnvio> GetPorIdAsync(long id);
        Task<List<NotaDeEnvio>> GetAllAsync();
    }
}
