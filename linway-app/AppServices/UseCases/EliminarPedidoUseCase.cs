using AppServices.Interfaces;
using Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppServices.UseCases
{
    public class ServiceStatus
    {
        public bool NothingSaved { get; set; }
        public bool Success { get; set; }
    }
    public class EliminarPedidoUseCase : IEliminarPedidoUseCase
    {
        private readonly IPedidoServices _pedidoServices;
        private readonly IProdVendidoServices _prodVendidoServices;
        private readonly ISavingServices _savingServices;
        public EliminarPedidoUseCase(IPedidoServices pedidoServices, IProdVendidoServices prodVendidoServices, ISavingServices savingServices)
        {
            _pedidoServices = pedidoServices;
            _prodVendidoServices = prodVendidoServices;
            _savingServices = savingServices;
        }
        public async Task<UseCaseResponse> ExecuteAsync(Pedido pedidoAEliminar)
        {
            //Reparto reparto = await repartoServices.GetPorIdAsync(pedidoAEliminar.RepartoId);
            //
            List<ProdVendido> prodVendidosDelPedido = pedidoAEliminar.ProdVendidos.ToList();
            foreach (ProdVendido prodVendido in prodVendidosDelPedido)
            {
                prodVendido.PedidoId = null;
            }
            _prodVendidoServices.EditOrDeleteMany(prodVendidosDelPedido);
            //
            _pedidoServices.Delete(pedidoAEliminar);
            //
            bool guardado = await _savingServices.SaveAsync();
            if (!guardado)
            {
                _savingServices.DiscardChanges();
                return UseCaseResponse.Fail("No se hicieron cambios");
            }
            return UseCaseResponse.Ok();
        }
    }
}
