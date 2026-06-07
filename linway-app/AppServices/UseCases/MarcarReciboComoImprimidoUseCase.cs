using AppServices.Interfaces;
using Models;
using System.Threading.Tasks;

namespace AppServices.UseCases
{
    public class MarcarReciboComoImprimidoUseCase : IMarcarReciboComoImprimidoUseCase
    {
        private readonly IReciboServices _reciboServices;
        private readonly ISavingServices _savingServices;
        public MarcarReciboComoImprimidoUseCase(IReciboServices reciboServices, ISavingServices savingServices)
        {
            _reciboServices = reciboServices;
            _savingServices = savingServices;
        }
        public async Task<UseCaseResponse> ExecuteAsync(Recibo recibo)
        {
            recibo.Impreso = 1;
            _reciboServices.Edit(recibo);
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
