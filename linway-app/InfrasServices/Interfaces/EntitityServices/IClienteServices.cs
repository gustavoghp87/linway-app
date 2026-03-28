using Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace linway_app.Services.Interfaces
{
    public interface IClienteServices
    {
        Task AddAsync(Cliente cliente);
        void Delete(Cliente cliente);
        void Edit(Cliente cliente);
        Task<Cliente> GetPorIdAsync(long clientId);
        Task<Cliente> GetPorDireccionAsync(string direccion);
        Task<Cliente> GetPorDireccionExactaAsync(string direccion);
        Task<List<Cliente>> GetAllAsync();
    }
}
