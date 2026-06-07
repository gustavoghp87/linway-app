using AppServices.Interfaces;
using Models;
using System.Threading.Tasks;

namespace AppServices.UseCases
{
    public class MarcarNotaDeEnvioComoImprimidaUseCase : IMarcarNotaDeEnvioComoImprimidaUseCase
    {
        private readonly INotaDeEnvioServices _notaDeEnvioServices;
        private readonly ISavingServices _savingServices;
        public MarcarNotaDeEnvioComoImprimidaUseCase(INotaDeEnvioServices notaDeEnvioServices, ISavingServices savingServices)
        {
            _notaDeEnvioServices = notaDeEnvioServices;
            _savingServices = savingServices;
        }
        public async Task<UseCaseResponse> ExecuteAsync(NotaDeEnvio notaDeEnvio)
        {
            notaDeEnvio.ImporteTotal = 1;
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
