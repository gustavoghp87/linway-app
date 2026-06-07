using AppServices.Interfaces;
using Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AppServices.UseCases
{
    public class ReposicionarRepartoUseCase : IReposicionarRepartoUseCase
    {
        private readonly IPedidoServices _pedidoServices;
        private readonly ISavingServices _savingServices;
        public ReposicionarRepartoUseCase(IPedidoServices pedidoServices, ISavingServices savingServices)
        {
            _pedidoServices = pedidoServices;
            _savingServices = savingServices;
        }
        public async Task<UseCaseResponse> ExecuteAsync(Reparto reparto, Pedido pedido1, Pedido pedido2)
        {
            long order1 = pedido1.Orden;
            long order2 = pedido2.Orden;
            if (order2 == order1)
            {
                return UseCaseResponse.Fail("No se pueden reposicionar pedidos con el mismo orden");
            }
            var pedidosAEditar = new List<Pedido>();
            if (order2 > order1)
            {
                //foreach (Pedido pedido in reparto.Pedidos.Where(x => x.Entregar == 1))
                foreach (Pedido pedido in reparto.Pedidos)
                {
                    if (pedido.Orden > order1 && pedido.Orden < order2)
                    {
                        pedido.Orden -= 1;
                        pedidosAEditar.Add(pedido);
                    }
                }
                pedido1.Orden = pedido2.Orden;
                pedido2.Orden -= 1;
                pedidosAEditar.Add(pedido1);
                pedidosAEditar.Add(pedido2);
            }
            else
            {
                //foreach (Pedido pedido in reparto.Pedidos.Where(x => x.Entregar == 1))
                foreach (Pedido pedido in reparto.Pedidos)
                {
                    if (pedido.Orden < order1 && pedido.Orden > order2)
                    {
                        pedido.Orden += 1;
                        pedidosAEditar.Add(pedido);
                    }
                }
                pedido1.Orden = pedido2.Orden + 1;
                pedidosAEditar.Add(pedido1);
                pedidosAEditar.Add(pedido2);
            }
            _pedidoServices.EditMany(pedidosAEditar);
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
