using AppServices.Interfaces;
using Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppServices.UseCases
{
    public class LimpiarRepartoUseCase : ILimpiarRepartoUseCase
    {
        private readonly IPedidoServices _pedidoServices;
        private readonly IProdVendidoServices _prodVendidoServices;
        private readonly ISavingServices _savingServices;
        public LimpiarRepartoUseCase(IPedidoServices pedidoServices, IProdVendidoServices prodVendidoServices, ISavingServices savingServices)
        {
            _pedidoServices = pedidoServices;
            _prodVendidoServices = prodVendidoServices;
            _savingServices = savingServices;
        }
        public async Task<UseCaseResponse> ExecuteAsync(Reparto repartoALimpiar)
        {
            List<ProdVendido> prodVendidosALimpiar = repartoALimpiar.Pedidos.SelectMany(x => x.ProdVendidos).ToList();
            foreach (ProdVendido prodVendido in prodVendidosALimpiar)
            {
                prodVendido.PedidoId = null;
            }
            _prodVendidoServices.EditMany(prodVendidosALimpiar);
            //
            foreach (Pedido pedido in repartoALimpiar.Pedidos)
            {
                pedido.Entregar = 0;
            }
            _pedidoServices.EditMany(repartoALimpiar.Pedidos);
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
