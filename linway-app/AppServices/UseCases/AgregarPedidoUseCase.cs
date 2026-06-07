using AppServices.Interfaces;
using Models;
using System.Threading.Tasks;

namespace AppServices.UseCases
{
    public class AgregarPedidoUseCase : IAgregarPedidoUseCase
    {
        private readonly IPedidoServices _pedidoServices;
        private readonly ISavingServices _savingServices;
        public AgregarPedidoUseCase(IPedidoServices pedidoServices, ISavingServices savingServices)
        {
            _pedidoServices = pedidoServices;
            _savingServices = savingServices;
        }
        public async Task<UseCaseResponse> ExecuteAsync(Pedido pedido)
        {
            await _pedidoServices.AddAsync(pedido);
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
