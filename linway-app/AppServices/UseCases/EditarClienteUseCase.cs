using AppServices.Interfaces;
using Models;
using System.Threading.Tasks;

namespace AppServices.UseCases
{
    public class EditarClienteUseCase : IEditarClienteUseCase
    {
        private readonly IClienteServices _clienteServices;
        private readonly ISavingServices _savingServices;
        public EditarClienteUseCase(IClienteServices clienteServices, ISavingServices savingServices)
        {
            _clienteServices = clienteServices;
            _savingServices = savingServices;
        }
        public async Task<UseCaseResponse> ExecuteAsync(string direccion, string codigoPostal, string telefono, string nombre, string cuit, string tipo)
        {
            var cliente = new Cliente
            {
                Direccion = direccion,
                CodigoPostal = codigoPostal,
                Telefono = telefono,
                Nombre = nombre,
                Cuit = cuit,
                Tipo = tipo
            };
            _clienteServices.Edit(cliente);
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
