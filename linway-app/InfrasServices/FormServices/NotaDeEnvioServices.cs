using linway_app.Services.Interfaces;
using Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace linway_app.Services.FormServices
{
    public class NotaDeEnvioServices: INotaDeEnvioServices
    {
        private readonly IServicesBase<NotaDeEnvio> _services;
        
        public NotaDeEnvioServices(IServicesBase<NotaDeEnvio> services)
        {
            _services = services;
        }
        public void Add(NotaDeEnvio notaDeEnvio)
        {
            _services.Add(notaDeEnvio);
        }
        public void DeleteMany(ICollection<NotaDeEnvio> notas)
        {
            _services.DeleteMany(notas);
        }

        public void Edit(NotaDeEnvio notaDeEnvio)
        {
            _services.Edit(notaDeEnvio);
        }
        public async Task<NotaDeEnvio> GetPorIdAsync(long id)
        {
            NotaDeEnvio nota = await _services.GetAsync(id);
            return nota;
        }
        public async Task<List<NotaDeEnvio>> GetAllAsync()
        {
            List<NotaDeEnvio> notas = await _services.GetAllAsync();
            return notas;
        }
    }
}
