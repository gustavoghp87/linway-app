using Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace linway_app.Services.Interfaces
{
    public interface INotaDeEnvioServices
    {
        void AddNotaDeEnvio(NotaDeEnvio notaDeEnvio);
        void DeleteNotas(ICollection<NotaDeEnvio> notas);
        void EditNotaDeEnvio(NotaDeEnvio notaDeEnvio);
        void EditValores(NotaDeEnvio nota);
        Task<NotaDeEnvio> GetNotaDeEnvioPorIdAsync(long id);
        Task<List<NotaDeEnvio>> GetNotaDeEnviosAsync();
    }
}
