using linway_app.Models;
using linway_app.Models.DbContexts;
using linway_app.Repositories.Interfaces;
using System.Threading.Tasks;

namespace linway_app.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly LinwayDbContext _context;
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

        public UnitOfWork(LinwayDbContext context)
        {
            _context = context;
        }

        public IRepository<Cliente> RepoCliente =>
            _repoCliente ?? new RepositoryBase<Cliente>(_context);
        public IRepository<Producto> RepoProducto =>
            _repoProducto ?? new RepositoryBase<Producto>(_context);
        public IRepository<ProdVendido> RepoProdVendido =>
            _repoProdVendido ?? new RepositoryBase<ProdVendido>(_context);
        public IRepository<NotaDeEnvio> RepoNotaDeEnvio =>
            _repoNotaDeEnvio ?? new RepositoryBase<NotaDeEnvio>(_context);
        public IRepository<Reparto> RepoReparto =>
            _repoReparto ?? new RepositoryBase<Reparto>(_context);
        public IRepository<DiaReparto> RepoDiaReparto =>
            _repoDiaReparto ?? new RepositoryBase<DiaReparto>(_context);
        public IRepository<Venta> RepoVenta =>
            _repoVenta ?? new RepositoryBase<Venta>(_context);
        public IRepository<RegistroVenta> RepoRegistroVenta =>
            _repoRegistroVenta ?? new RepositoryBase<RegistroVenta>(_context);
        public IRepository<Recibo> RepoRecibo =>
            _repoRecibo ?? new RepositoryBase<Recibo>(_context);
        public IRepository<Pedido> RepoPedido =>
            _repoPedido ?? new RepositoryBase<Pedido>(_context);
        public IRepository<DetalleRecibo> RepoDetalleRecibo =>
            _repoDetalleRecibo ?? new RepositoryBase<DetalleRecibo>(_context);

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
