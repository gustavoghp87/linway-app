using Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace linway_app.Services.Interfaces
{
    public interface IPedidoServices
    {
        Task AddPedidoAsync(Pedido pedido);
        void DeletePedido(Pedido pedido);
        void EditPedido(Pedido pedido);
        void EditPedidos(ICollection<Pedido> pedidos);
        Task<Pedido> GetPedidoPorIdAsync(long pedidoId);
        Task<List<Pedido>> GetPedidosAsync();
        Task<List<Pedido>> GetPedidosPorRepartoIdAsync(long repartoId);
    }
}
