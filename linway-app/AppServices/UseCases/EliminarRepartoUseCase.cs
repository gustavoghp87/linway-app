using AppServices.Interfaces;
using Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AppServices.UseCases
{
    public class EliminarRepartoUseCase : IEliminarRepartoUseCase
    {
        private readonly IPedidoServices _pedidoServices;
        private readonly IProdVendidoServices _prodVendidoServices;
        private readonly IRepartoServices _repartoServices;
        private readonly ISavingServices _savingServices;
        public EliminarRepartoUseCase(IPedidoServices pedidoServices, IProdVendidoServices prodVendidoServices, IRepartoServices repartoServices, ISavingServices savingServices)
        {
            _pedidoServices = pedidoServices;
            _prodVendidoServices = prodVendidoServices;
            _repartoServices = repartoServices;
            _savingServices = savingServices;
        }
        public async Task<UseCaseResponse> ExecuteAsync(Reparto reparto)
        {
            var prodVendidosDelReparto = new List<ProdVendido>();
            foreach (Pedido pedido in reparto.Pedidos)
            {
                foreach (ProdVendido prodVendido in pedido.ProdVendidos)
                {
                    prodVendido.PedidoId = null;
                }
                prodVendidosDelReparto.AddRange(pedido.ProdVendidos);
            }
            _prodVendidoServices.EditOrDeleteMany(prodVendidosDelReparto);
            //
            _pedidoServices.DeleteMany(reparto.Pedidos);
            //
            _repartoServices.Delete(reparto);
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
