using AppServices.Interfaces;
using Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppServices.UseCases
{
    public class LimpiarDiaRepartoUseCase : ILimpiarDiaRepartoUseCase
    {
        private readonly IPedidoServices _pedidoServices;
        private readonly IProdVendidoServices _prodVendidoServices;
        private readonly ISavingServices _savingServices;
        public LimpiarDiaRepartoUseCase(IPedidoServices pedidoServices, IProdVendidoServices prodVendidoServices, ISavingServices savingServices)
        {
            _pedidoServices = pedidoServices;
            _prodVendidoServices = prodVendidoServices;
            _savingServices = savingServices;
        }
        public async Task<UseCaseResponse> ExecuteAsync(ICollection<Reparto> repartosALimpiar)
        {
            List<ProdVendido> prodVendidosALimpiar = repartosALimpiar.SelectMany(x => x.Pedidos).SelectMany(x => x.ProdVendidos).ToList();
            //
            foreach (ProdVendido prodVendido in prodVendidosALimpiar)
            {
                prodVendido.PedidoId = null;
            }
            _prodVendidoServices.EditMany(prodVendidosALimpiar);
            //
            foreach (var reparto in repartosALimpiar)
            {
                foreach (Pedido pedido in reparto.Pedidos)
                {
                    pedido.Entregar = 0;
                }
            }
            _pedidoServices.EditMany(repartosALimpiar.SelectMany(x => x.Pedidos).ToList());
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
