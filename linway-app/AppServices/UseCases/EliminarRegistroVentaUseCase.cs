using AppServices.Interfaces;
using Models;
using System.Linq;
using System.Threading.Tasks;

namespace AppServices.UseCases
{
    public class EliminarRegistroVentaUseCase : IEliminarRegistroVentaUseCase
    {
        private readonly IProdVendidoServices _prodVendidoServices;
        private readonly IRegistroVentaServices _registroVentaServices;
        private readonly IVentaServices _ventaServices;
        private readonly ISavingServices _savingServices;
        public EliminarRegistroVentaUseCase(IProdVendidoServices prodVendidoServices, IRegistroVentaServices registroVentaServices, IVentaServices ventaServices, ISavingServices savingServices)
        {
            _prodVendidoServices = prodVendidoServices;
            _registroVentaServices = registroVentaServices;
            _ventaServices = ventaServices;
            _savingServices = savingServices;
        }
        public async Task<UseCaseResponse> ExecuteAsync(RegistroVenta registroVenta, bool restarDeVentas)
        {
            if (restarDeVentas)
            {
                await _ventaServices.RestarDesdeProdVendidosAsync(registroVenta.ProdVendidos);
            }
            //
            foreach (ProdVendido pv in registroVenta.ProdVendidos)
            {
                pv.RegistroVentaId = null;
                pv.PedidoId = null;
            }
            _prodVendidoServices.EditOrDeleteMany(registroVenta.ProdVendidos.ToList());
            //
            _registroVentaServices.Delete(registroVenta);
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
