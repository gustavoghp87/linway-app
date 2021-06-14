using linway_app.Models;
using System;
using System.Threading.Tasks;

namespace linway_app.Repositories.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Cliente> RepoCliente { get; }
        IRepository<Producto> RepoProducto { get; }
        IRepository<ProdVendido> RepoProdVendido { get; }
        IRepository<NotaDeEnvio> RepoNotaDeEnvio { get; }
        IRepository<Venta> RepoVenta { get; }
        IRepository<RegistroVenta> RepoRegistroVenta { get; }
        IRepository<Recibo> RepoRecibo { get; }
        IRepository<Reparto> RepoReparto { get; }
        IRepository<DiaReparto> RepoDiaReparto { get; }
        IRepository<Pedido> RepoPedido{ get; }
        IRepository<DetalleRecibo> RepoDetalleRecibo { get; }
        void SaveChanges();
        Task SaveChangesAsync();
    }
}
