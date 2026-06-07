using AppServices.UseCases;
using Models;
using System.Threading.Tasks;

namespace AppServices.Interfaces
{
    public interface IAgregarClienteUseCase
    {
        Task<UseCaseResponse> ExecuteAsync(string direccion, string localidad, string codigoPostal, string telefono, string nombre, string cuit, string tipo);
    }
}
