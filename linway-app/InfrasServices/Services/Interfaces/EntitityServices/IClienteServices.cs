using Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace linway_app.Services.Interfaces
{
    public interface IClienteServices
    {
        Task AddClienteAsync(Cliente cliente);
        Task AddClientePrimeroAsync();
        void DeleteCliente(Cliente cliente);
        void EditCliente(Cliente cliente);
        Task<Cliente> GetClientePorIdAsync(long clientId);
        Task<Cliente> GetClientePorDireccionAsync(string direccion);
        Task<Cliente> GetClientePorDireccionExactaAsync(string direccion);
        Task<List<Cliente>> GetClientesAsync();
    }
}
