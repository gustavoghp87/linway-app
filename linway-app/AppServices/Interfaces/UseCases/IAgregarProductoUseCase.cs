using AppServices.UseCases;
using System.Threading.Tasks;

namespace AppServices.Interfaces
{
    public interface IAgregarProductoUseCase
    {
        Task<UseCaseResponse> ExecuteAsync(string nombre, decimal precio, string tipo, string subTipo);
    }
}
