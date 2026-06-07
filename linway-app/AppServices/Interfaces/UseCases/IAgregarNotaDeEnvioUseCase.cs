using AppServices.UseCases;
using Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AppServices.Interfaces
{
    public interface IAgregarNotaDeEnvioUseCase
    {
        Task<UseCaseResponse<NotaDeEnvio>> ExecuteAsync(Cliente cliente, ICollection<Producto> productos, ICollection<ProdVendido> prodVendidos, Pedido pedido, Reparto reparto, bool agregarProdVendidosARegistrosYVentas, bool enviarAHojaDeReparto, bool imprimir);
    }
}
