using AppServices.Interfaces;
using Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppServices.UseCases
{
    public class LimpiarPedidoUseCase : ILimpiarPedidoUseCase
    {
        private readonly IPedidoServices _pedidoServices;
        private readonly IProdVendidoServices _prodVendidoServices;
        private readonly ISavingServices _savingServices;
        public LimpiarPedidoUseCase(IPedidoServices pedidoServices, IProdVendidoServices prodVendidoServices, ISavingServices savingServices)
        {
            _pedidoServices = pedidoServices;
            _prodVendidoServices = prodVendidoServices;
            _savingServices = savingServices;
        }
        public async Task<UseCaseResponse> ExecuteAsync(Pedido pedidoAEditar)
        {
            Reparto repartoAEditar = pedidoAEditar.Reparto;
            List<ProdVendido> prodVendidosALimpiar = pedidoAEditar.ProdVendidos.ToList();
            //
            foreach (ProdVendido prodVendido in prodVendidosALimpiar)
            {
                prodVendido.PedidoId = null;
            }
            _prodVendidoServices.EditMany(prodVendidosALimpiar);
            //
            foreach (Pedido pedido in repartoAEditar.Pedidos)
            {
                if (pedido.Id == pedidoAEditar.Id)
                {
                    pedido.Entregar = 0;
                }
            }
            _pedidoServices.EditMany(repartoAEditar.Pedidos);
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
