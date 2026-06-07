using AppServices.Interfaces;
using Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AppServices.UseCases
{
    public class AgregarReciboUseCase : IAgregarReciboUseCase
    {
        private readonly IDetalleReciboServices _detalleReciboServices;
        private readonly IReciboServices _reciboServices;
        private readonly ISavingServices _savingServices;
        public AgregarReciboUseCase(IDetalleReciboServices detalleReciboServices, IReciboServices reciboServices, ISavingServices savingServices)
        {
            _detalleReciboServices = detalleReciboServices;
            _reciboServices = reciboServices;
            _savingServices = savingServices;
        }
        public async Task<UseCaseResponse> ExecuteAsync(Cliente cliente, ICollection<DetalleRecibo> detalles)
        {
            decimal importe = 0;
            foreach (DetalleRecibo recibo in detalles)
            {
                importe += recibo.Importe;
            }
            var nuevoRecibo = new Recibo
            {
                ClienteId = cliente.Id,
                DireccionCliente = cliente.Direccion,
                ImporteTotal = importe,
                Impreso = 0,
                Fecha = DateTime.Now.ToString(Constants.FormatoDeFecha)
            };
            _reciboServices.Add(nuevoRecibo);
            //
            foreach (DetalleRecibo detalle in detalles)
            {
                detalle.Recibo = nuevoRecibo;
            }
            _detalleReciboServices.AddMany(detalles);
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
