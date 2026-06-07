using AppServices.EntityServices;
using AppServices.Interfaces;
using Models;
using System.Linq;
using System.Threading.Tasks;

namespace AppServices.UseCases
{
    public class QuitarProdVendidoDeNotaDeEnvioUseCase : IQuitarProdVendidoDeNotaDeEnvioUseCase
    {
        private readonly INotaDeEnvioServices _notaDeEnvioServices;
        private readonly IProdVendidoServices _prodVendidoServices;
        private readonly IVentaServices _ventaServices;
        private readonly ISavingServices _savingServices;
        public QuitarProdVendidoDeNotaDeEnvioUseCase(INotaDeEnvioServices notaDeEnvioServices, IProdVendidoServices prodVendidoServices, IVentaServices ventaServices, ISavingServices savingServices)
        {
            _prodVendidoServices = prodVendidoServices;
            _notaDeEnvioServices = notaDeEnvioServices;
            _ventaServices = ventaServices;
            _savingServices = savingServices;
        }
        public async Task<UseCaseResponse> ExecuteAsync(NotaDeEnvio notaDeEnvio, ProdVendido prodVendido, bool restarDeVentas)
        {
            if (prodVendido.RegistroVentaId != null && restarDeVentas)
            {
                await _ventaServices.RestarDesdeProdVendidosAsync(prodVendido);
            }
            //
            // nada para actualizar en RV
            //
            _prodVendidoServices.Delete(prodVendido);
            // se actualiza la nota de envío
            notaDeEnvio.ProdVendidos = notaDeEnvio.ProdVendidos.ToList().FindAll(pv => pv.Id != prodVendido.Id);
            notaDeEnvio.ImporteTotal = NotaDeEnvioServices.ExtraerImporteDeNotaDeEnvio(notaDeEnvio.ProdVendidos);
            notaDeEnvio.Detalle = NotaDeEnvioServices.ExtraerDetalleDeNotaDeEnvio(notaDeEnvio.ProdVendidos);
            _notaDeEnvioServices.Edit(notaDeEnvio);
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
