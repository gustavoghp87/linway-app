using AppServices.Interfaces;
using Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AppServices.UseCases
{
    public class AgregarRepartoUseCase : IAgregarRepartoUseCase
    {
        private readonly IRepartoServices _repartoServices;
        private readonly ISavingServices _savingServices;
        public AgregarRepartoUseCase(IRepartoServices repartoServices, ISavingServices savingServices)
        {
            _repartoServices = repartoServices;
            _savingServices = savingServices;
        }
        public async Task<UseCaseResponse> ExecuteAsync(Reparto nuevoReparto, ICollection<DiaReparto> diaRepartos)
        {
            _repartoServices.Add(nuevoReparto, diaRepartos);
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
