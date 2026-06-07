using AppServices.UseCases;
using System.Threading.Tasks;

namespace AppServices.Interfaces
{
    public interface IEditarClienteUseCase
    {
        Task<UseCaseResponse> ExecuteAsync(string direccion, string codigoPostal, string telefono, string nombre, string cuit, string tipo);
    }
}
