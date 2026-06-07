using AppServices.Interfaces;
using Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AppServices.UseCases
{
    public class EliminarRecibosUseCase : IEliminarRecibosUseCase
    {
        private readonly IDetalleReciboServices _detalleReciboServices;
        private readonly IReciboServices _reciboServices;
        private readonly ISavingServices _savingServices;
        public EliminarRecibosUseCase(IDetalleReciboServices detalleReciboServices, IReciboServices reciboServices, ISavingServices savingServices)
        {
            _detalleReciboServices = detalleReciboServices;
            _reciboServices = reciboServices;
            _savingServices = savingServices;
        }
        public async Task<UseCaseResponse> ExecuteAsync(List<Recibo> recibos, ICollection<DetalleRecibo> detalles)
        {
            _detalleReciboServices.DeleteMany(detalles);  // primero
            _reciboServices.DeleteMany(recibos);  // segundo
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
