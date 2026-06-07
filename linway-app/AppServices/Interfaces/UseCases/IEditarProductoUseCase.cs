using AppServices.UseCases;
using System.Threading.Tasks;

namespace AppServices.Interfaces
{
    public interface IEditarProductoUseCase
    {
        Task<UseCaseResponse> ExecuteAsync(string nombre, decimal precio, string tipo, string subTipo);
    }
}
