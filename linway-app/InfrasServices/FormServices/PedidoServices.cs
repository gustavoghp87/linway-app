using linway_app.Services.Interfaces;
using Models;
using Models.Enums;
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
        public async Task AddAsync(Pedido pedido)
        {
            pedido.Orden = await GetOrdenAsync(pedido.RepartoId);
            _services.Add(pedido);
        }
        public void Delete(Pedido pedido)
        {
            _services.Delete(pedido);
        }
        public void DeleteMany(ICollection<Pedido> pedidos)
        {
            _services.DeleteMany(pedidos);
        }
        public void Edit(Pedido pedido)
        {
            _services.Edit(pedido);
        }
        public void EditMany(ICollection<Pedido> pedidos)
        {
            _services.EditMany(pedidos);
        }
        public async Task<Pedido> GetPorIdAsync(long pedidoId)
        {
            Pedido pedido = await _services.GetAsync(pedidoId);
            return pedido;
        }
        public async Task<List<Pedido>> GetPorRepartoIdAsync(long repartoId)
        {
            var pedidos = await GetAllAsync();
            if (pedidos == null)
            {
                return new List<Pedido>();
            }
            pedidos = pedidos.Where(x => x.RepartoId == repartoId).ToList();
            // ver:
            //List<Cliente> clientes = getClientes();
            //foreach (Pedido pedido in pedidos)
            //{
            //    pedido.Direccion = clientes.Find(x => x.Id == pedido.ClienteId)?.Direccion;
            //};
            return pedidos;
        }
        public async Task<List<Pedido>> GetAllAsync()
        {
            List<Pedido> pedidos = await _services.GetAllAsync();
            return pedidos;
        }
        #region private methods
        private async Task<long> GetOrdenAsync(long repartoId)
        {
            var lstPedidos = await GetPorRepartoIdAsync(repartoId);
            if (lstPedidos == null || lstPedidos.Count == 0)
            {
                return 1;
            }
            long lastOrden = lstPedidos.OrderBy(x => x.Orden).Last().Orden;
            return lastOrden + 1;
        }
        #endregion
        #region static methods
        public static Pedido GetNuevoPedido(Cliente cliente, Reparto reparto)
        {
            return new Pedido()
            {
                Cliente = cliente,
                Entregar = 1,
                Reparto = reparto
            };
        }
        #endregion
    }
}
