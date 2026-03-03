using Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace linway_app.Services.Interfaces
{
    public interface IPedidoServices
    {
        void AddPedido(Pedido pedido);
        void CleanPedidos(ICollection<Pedido> pedidos);
        void DeletePedido(Pedido pedido);
        void EditPedidos(ICollection<Pedido> pedidos);
        Task<Pedido> GetPedido(long pedidoId);
        Task<ICollection<Pedido>> GetPedidos();
        Task<ICollection<Pedido>> GetPedidosPorRepartoId(long repartoId);
    }
}
