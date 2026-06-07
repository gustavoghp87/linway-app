using AppServices.Interfaces;
using Models;
using Models.Enums;
using System.Threading.Tasks;

namespace AppServices.UseCases
{
    public class AgregarClienteUseCase : IAgregarClienteUseCase
    {
        private readonly IClienteServices _clienteServices;
        private readonly ISavingServices _savingServices;
        public AgregarClienteUseCase(IClienteServices clienteServices, ISavingServices savingServices)
        {
            _clienteServices = clienteServices;
            _savingServices = savingServices;
        }
        public async Task<UseCaseResponse> ExecuteAsync(string direccion, string localidad, string codigoPostal, string telefono, string nombre, string cuit, string tipo)
        {
            var nuevoCliente = new Cliente
            {
                Direccion = localidad != ""
                    ? direccion + " - " + localidad
                    : direccion,
                CodigoPostal = codigoPostal,
                Telefono = telefono,
                Nombre = nombre,
                Cuit = cuit,
                Tipo = tipo
            };
            await _clienteServices.AddAsync(nuevoCliente);
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
