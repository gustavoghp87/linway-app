using Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace linway_app.Services.Interfaces
{
    public interface IPedidoServices
    {
        Task AddAsync(Pedido pedido);
        void Delete(Pedido pedido);
        void DeleteMany(ICollection<Pedido> pedidos);
        void Edit(Pedido pedido);
        void EditMany(ICollection<Pedido> pedidos);
        Task<Pedido> GetPorIdAsync(long pedidoId);
        Task<List<Pedido>> GetAllAsync();
        Task<List<Pedido>> GetPorRepartoIdAsync(long repartoId);
    }
}
