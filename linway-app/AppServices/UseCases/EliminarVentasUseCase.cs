using AppServices.Interfaces;
using Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AppServices.UseCases
{
    public class EliminarVentasUseCase : IEliminarVentasUseCase
    {
        private readonly IVentaServices _ventaServices;
        private readonly ISavingServices _savingServices;
        public EliminarVentasUseCase(IVentaServices ventaServices, ISavingServices savingServices)
        {
            _ventaServices = ventaServices;
            _savingServices = savingServices;
        }
        public async Task<UseCaseResponse> ExecuteAsync(ICollection<Venta> ventas)
        {
            _ventaServices.DeleteMany(ventas);
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
