using linway_app.Services.Interfaces;
using Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace linway_app.Services.FormServices
{
    public class PedidoServices: IPedidoServices
    {
        private readonly IServicesBase<Pedido> _services;
        public PedidoServices(IServicesBase<Pedido> services)
        {
            _services = services;
        }
        public async void AddPedido(Pedido pedido)
        {
            pedido.Orden = await GetOrden(pedido.RepartoId);
            _services.Add(pedido);
        }
        public void CleanPedidos(ICollection<Pedido> pedidos)
        {
            if (pedidos == null || pedidos.Count == 0)
            {
                return;
            }
            foreach (Pedido pedido in pedidos)
            {
                pedido.Entregar = 0;
                pedido.L = 0;
                pedido.ProductosText = "";
                pedido.A = 0;
                pedido.E = 0;
                pedido.D = 0;
                pedido.T = 0;
                pedido.Ae = 0;
            }
            EditPedidos(pedidos);
        }
        public void DeletePedido(Pedido pedido)
        {
            _services.Delete(pedido);
        }
        public void EditPedidos(ICollection<Pedido> pedidos)
        {
            if (pedidos == null || pedidos.Count == 0)
            {
                return;
            }
            _services.EditMany(pedidos);
        }
        public async Task<Pedido> GetPedido(long pedidoId)
        {
            Pedido pedido = await _services.GetAsync(pedidoId);
            return pedido;
        }
        public async Task<ICollection<Pedido>> GetPedidosPorRepartoId(long repartoId)
        {
            var pedidos = await GetPedidos();
            if (pedidos == null || pedidos.Count == 0)
            {
                return new List<Pedido>();
            }
            pedidos = pedidos.Where(x => x.RepartoId == repartoId && x.Estado != null && x.Estado != "Eliminado").ToList();
            // ver:
            //List<Cliente> clientes = getClientes();
            //foreach (Pedido pedido in pedidos)
            //{
            //    pedido.Direccion = clientes.Find(x => x.Id == pedido.ClienteId)?.Direccion;
            //};
            return pedidos;
        }
        public async Task<ICollection<Pedido>> GetPedidos()
        {
            ICollection<Pedido> pedidos = await _services.GetAllAsync();
            return pedidos;
        }
        #region
        private async Task<long> GetOrden(long repartoId)
        {
            var lstPedidos = await GetPedidosPorRepartoId(repartoId);
            if (lstPedidos == null || lstPedidos.Count == 0)
            {
                return 1;
            }
            long lastOrden = lstPedidos.OrderBy(x => x.Orden).Last().Orden;
            return lastOrden + 1;
        }
        #endregion
    }
}
