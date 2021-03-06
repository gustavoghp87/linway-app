﻿using linway_app.Models;
using linway_app.Models.DbContexts;
using linway_app.Repositories.Interfaces;
using System.Threading.Tasks;

namespace linway_app.Repositories
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
        public UnitOfWork(LinwayDbContext context,
            IRepository<Cliente> repoCliente,
            IRepository<DetalleRecibo> repoDetalleRecibo,
            IRepository<DiaReparto> repoDiaReparto,
            IRepository<NotaDeEnvio> repoNotaDeEnvio,
            IRepository<Pedido> repoPedido,
            IRepository<Producto> repoProducto,
            IRepository<ProdVendido> repoProdVendido,
            IRepository<Recibo> repoRecibo,
            IRepository<RegistroVenta> repoRegistroVenta,
            IRepository<Reparto> repoReparto,
            IRepository<Venta> repoVenta
        )
        {
            _context = context;
            _repoCliente = repoCliente;
            _repoDetalleRecibo = repoDetalleRecibo;
            _repoDiaReparto = repoDiaReparto;
            _repoNotaDeEnvio = repoNotaDeEnvio;
            _repoPedido = repoPedido;
            _repoProducto = repoProducto;
            _repoProdVendido = repoProdVendido;
            _repoRecibo = repoRecibo;
            _repoRegistroVenta = repoRegistroVenta;
            _repoReparto = repoReparto;
            _repoVenta = repoVenta;
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
