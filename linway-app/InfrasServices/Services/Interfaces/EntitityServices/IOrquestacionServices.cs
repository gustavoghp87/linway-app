using Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace linway_app.Services.Interfaces
{
    public interface IOrquestacionServices
    {
        void CleanRepartos(ICollection<Reparto> repartos);
        Task EditClienteYDireccionEnPedidosAsync(Cliente cliente);
        NotaDeEnvio EditNotaDeEnvioQuitar(NotaDeEnvio notaDeEnvio, ProdVendido prodVendidoAEliminar);
        Task ExportRepartoAsync(string dia, string nombreReparto);
        Task<Pedido> GetPedidoPorRepartoYClienteGenerarSiNoExisteAsync(long repartoId, long clienteId);
        Task<Reparto> GetRepartoPorDiaYNombreAsync(string dia, string nombre);
        Task<List<Reparto>> GetRepartosPorDiaAsync(string diaReparto);
        Task UpdatePedidoAsync(Pedido pedido, bool entregar);
        Task UpdateVentasDesdeProdVendidosAsync(ICollection<ProdVendido> prodVendidos, bool addingUp);
    }
}
