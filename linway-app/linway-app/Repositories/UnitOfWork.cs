using linway_app.Models;
using linway_app.Repositories.Interfaces;
using System.Threading.Tasks;

namespace linway_app.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly LinwaydbContext _context;
        private readonly IRepository<Cliente> _repoCliente;
        private readonly IRepository<Producto> _repoProducto;
        private readonly IRepository<ProdVendido> _repoProdVendido;
        private readonly IRepository<NotaDeEnvio> _repoNotaDeEnvio;
        private readonly IRepository<Reparto> _repoReparto;
        private readonly IRepository<DiaReparto> _repoDiaReparto;
        private readonly IRepository<Venta> _repoVenta;
        private readonly IRepository<RegistroVenta> _repoRegistroVenta;
        private readonly IRepository<Recibo> _repoRecibo;
        private readonly IRepository<Pedido> _repoPedido;
        private readonly IRepository<DetalleRecibo> _repoDetalleRecibo;

        public UnitOfWork(LinwaydbContext context)
        {
            _context = context;
        }

        public IRepository<Cliente> RepoCliente => _repoCliente ?? new RepositoryCliente(_context);
        public IRepository<Producto> RepoProducto => _repoProducto ?? new RepositoryProducto(_context);
        public IRepository<ProdVendido> RepoProdVendido => _repoProdVendido ?? new RepositoryProdVendido(_context);
        public IRepository<NotaDeEnvio> RepoNotaDeEnvio => _repoNotaDeEnvio ?? new RepositoryNotaDeEnvio (_context);
        public IRepository<Reparto> RepoReparto => _repoReparto ?? new RepositoryReparto(_context);
        public IRepository<DiaReparto> RepoDiaReparto => _repoDiaReparto ?? new RepositoryDiaReparto(_context);
        public IRepository<Venta> RepoVenta => _repoVenta ?? new RepositoryVenta(_context);
        public IRepository<RegistroVenta> RepoRegistroVenta => _repoRegistroVenta ?? new RepositoryRegistroVenta(_context);
        public IRepository<Recibo> RepoRecibo => _repoRecibo ?? new RepositoryRecibo(_context);
        public IRepository<Pedido> RepoPedido => _repoPedido ?? new RepositoryPedido(_context);
        public IRepository<DetalleRecibo> RepoDetalleRecibo => _repoDetalleRecibo ?? new RepositoryDetalleRecibo(_context);

        public void Dispose()
        {
            if (_context != null) _context.Dispose();
        }
        public void SaveChanges()
        {
            _context.SaveChangesAsync();
        }
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
