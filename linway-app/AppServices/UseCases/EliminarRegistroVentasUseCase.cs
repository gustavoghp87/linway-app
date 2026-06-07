using AppServices.Interfaces;
using Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AppServices.UseCases
{
    public class EliminarRegistroVentasUseCase : IEliminarRegistroVentasUseCase
    {
        private readonly IProdVendidoServices _prodVendidoServices;
        private readonly IRegistroVentaServices _registroVentaServices;
        private readonly IVentaServices _ventaServices;
        private readonly ISavingServices _savingServices;
        public EliminarRegistroVentasUseCase(IProdVendidoServices prodVendidoServices, IRegistroVentaServices registroVentaServices, IVentaServices ventaServices, ISavingServices savingServices)
        {
            _prodVendidoServices = prodVendidoServices;
            _registroVentaServices = registroVentaServices;
            _ventaServices = ventaServices;
            _savingServices = savingServices;
        }
        public async Task<UseCaseResponse> ExecuteAsync(ICollection<RegistroVenta> registrosVentas)  // bool restarDeVentas ?
        {
            var prodVendidosAEditarOEliminar = new List<ProdVendido>();
            foreach (RegistroVenta registroVenta in registrosVentas)
            {
                prodVendidosAEditarOEliminar.AddRange(registroVenta.ProdVendidos);
            }
            foreach (ProdVendido pv in prodVendidosAEditarOEliminar)
            {
                pv.RegistroVentaId = null;
            }
            _prodVendidoServices.EditOrDeleteMany(prodVendidosAEditarOEliminar);
            //
            _registroVentaServices.DeleteMany(registrosVentas);
            //
            await _ventaServices.RestarDesdeProdVendidosAsync(prodVendidosAEditarOEliminar);
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
