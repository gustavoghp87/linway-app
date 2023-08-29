using Infrastructure.Repositories.DbContexts;
using Infrastructure.Repositories.Interfaces;
using Models;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly LinwayDbContext _context;
        private readonly IRepository<Cliente> _repoCliente;
        private readonly IRepository<DetalleRecibo> _repoDetalleRecibo;
        private readonly IRepository<DiaReparto> _repoDiaReparto;
        private readonly IRepository<NotaDeEnvio> _repoNotaDeEnvio;
        private readonly IRepository<Pedido> _repoPedido;
        private readonly IRepository<Producto> _repoProducto;
        private readonly IRepository<ProdVendido> _repoProdVendido;
        private readonly IRepository<Recibo> _repoRecibo;
        private readonly IRepository<RegistroVenta> _repoRegistroVenta;
        private readonly IRepository<Reparto> _repoReparto;
        private readonly IRepository<Venta> _repoVenta;
        public UnitOfWork(LinwayDbContext context
            //IRepository<Cliente> repoCliente,
            //IRepository<DetalleRecibo> repoDetalleRecibo,
            //IRepository<DiaReparto> repoDiaReparto,
            //IRepository<NotaDeEnvio> repoNotaDeEnvio,
            //IRepository<Pedido> repoPedido,
            //IRepository<Producto> repoProducto,
            //IRepository<ProdVendido> repoProdVendido,
            //IRepository<Recibo> repoRecibo,
            //IRepository<RegistroVenta> repoRegistroVenta,
            //IRepository<Reparto> repoReparto,
            //IRepository<Venta> repoVenta
        )
        {
            _context = context;
            _repoCliente = new RepositoryCliente();
            _repoDetalleRecibo = new RepositoryDetalleRecibo();
            _repoDiaReparto = new RepositoryDiaReparto();
            _repoNotaDeEnvio = new RepositoryNotaDeEnvio();
            _repoPedido = new RepositoryPedido();
            _repoProducto = new RepositoryProducto();
            _repoProdVendido = new RepositoryProdVendido();
            _repoRecibo = new RepositoryRecibo();
            _repoRegistroVenta = new RepositoryRegistroVenta();
            _repoReparto = new RepositoryReparto();
            _repoVenta = new RepositoryVenta();
        }
        public IRepository<Cliente> RepoCliente => _repoCliente;
        public IRepository<Producto> RepoProducto => _repoProducto;
        public IRepository<ProdVendido> RepoProdVendido => _repoProdVendido;
        public IRepository<NotaDeEnvio> RepoNotaDeEnvio => _repoNotaDeEnvio;
        public IRepository<Reparto> RepoReparto => _repoReparto;
        public IRepository<DiaReparto> RepoDiaReparto => _repoDiaReparto;
        public IRepository<Venta> RepoVenta => _repoVenta;
        public IRepository<RegistroVenta> RepoRegistroVenta => _repoRegistroVenta;
        public IRepository<Recibo> RepoRecibo => _repoRecibo;
        public IRepository<Pedido> RepoPedido => _repoPedido;
        public IRepository<DetalleRecibo> RepoDetalleRecibo => _repoDetalleRecibo;
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
