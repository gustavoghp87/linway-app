using Microsoft.EntityFrameworkCore;
using Models;

namespace Infrastructure.Repositories.DbContexts
{
    public partial class LinwayDbContext : DbContext
    {
        public virtual DbSet<Cliente> Clientes { get; set; }
        public virtual DbSet<DetalleRecibo> DetalleRecibos { get; set; }
        public virtual DbSet<DiaReparto> DiaRepartos { get; set; }
        public virtual DbSet<NotaDeEnvio> NotaDeEnvios { get; set; }
        public virtual DbSet<Pedido> Pedidos { get; set; }
        public virtual DbSet<Producto> Productos { get; set; }
        public virtual DbSet<ProdVendido> ProdVendidos { get; set; }
        public virtual DbSet<Recibo> Recibos { get; set; }
        public virtual DbSet<RegistroVenta> RegistroVentas { get; set; }
        public virtual DbSet<Reparto> Repartos { get; set; }
        public virtual DbSet<Venta> Ventas { get; set; }
        public LinwayDbContext(DbContextOptions<LinwayDbContext> options) : base(options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySql(Constants.GetConnectionString());
                optionsBuilder.UseLazyLoadingProxies();
                optionsBuilder.AddInterceptors(new EfCommandInterceptor());
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cliente>(entity =>
            {
                entity.HasIndex(e => e.Id);
                entity.Property(e => e.Cuit).HasColumnName("CUIT");
                entity.Property(e => e.Direccion).IsRequired().HasMaxLength(80);
                entity.HasIndex(e => e.Direccion).IsUnique();
            });
            modelBuilder.Entity<DetalleRecibo>(entity =>
            {
                entity.HasIndex(e => e.Id);
                entity.Property(e => e.Detalle).IsRequired();
                entity.HasOne(d => d.Recibo)
                    .WithMany(p => p.DetalleRecibos)
                    .HasForeignKey(d => d.ReciboId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
            modelBuilder.Entity<DiaReparto>(entity =>
            {
                entity.HasIndex(e => e.Id);
                entity.HasIndex(e => e.Dia).IsUnique();
                entity.Property(e => e.Dia).IsRequired();
            });
            modelBuilder.Entity<NotaDeEnvio>(entity =>
            {
                entity.HasIndex(e => e.Id);
                entity.Property(e => e.Detalle).IsRequired();
                entity.Property(e => e.Fecha).IsRequired();
                entity.HasOne(d => d.Cliente)
                    .WithMany(p => p.NotaDeEnvios)
                    .HasForeignKey(d => d.ClienteId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
            modelBuilder.Entity<Pedido>(entity =>
            {
                entity.HasIndex(e => e.Id);
                //entity.Property(e => e.Ae).HasColumnName("AE");
                //entity.Property(e => e.Direccion).IsRequired().HasMaxLength(80);
                entity.HasOne(d => d.Cliente)
                    .WithMany(p => p.Pedidos)
                    .HasForeignKey(d => d.ClienteId)
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(d => d.Reparto)
                    .WithMany(p => p.Pedidos)
                    .HasForeignKey(d => d.RepartoId)
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasIndex(e => new { e.ClienteId, e.RepartoId }).IsUnique();
            });
            modelBuilder.Entity<Producto>(entity =>
            {
                entity.HasIndex(e => e.Id);
                entity.Property(e => e.Nombre).IsRequired().HasMaxLength(60);
                entity.HasIndex(e => e.Nombre).IsUnique();
            });
            modelBuilder.Entity<ProdVendido>(entity =>
            {
                entity.HasIndex(e => e.Id);
                entity.Property(e => e.Descripcion).IsRequired();
                entity.HasOne(d => d.NotaDeEnvio)
                    .WithMany(p => p.ProdVendidos)
                    .HasForeignKey(d => d.NotaDeEnvioId);
                entity.HasOne(d => d.Pedido)
                    .WithMany(p => p.ProdVendidos)
                    .HasForeignKey(d => d.PedidoId);
                entity.HasOne(d => d.Producto)
                    .WithMany(p => p.ProdVendidos)
                    .HasForeignKey(d => d.ProductoId)
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(d => d.RegistroVenta)
                    .WithMany(p => p.ProdVendidos)
                    .HasForeignKey(d => d.RegistroVentaId);
            });
            modelBuilder.Entity<Recibo>(entity =>
            {
                entity.HasIndex(e => e.Id);
                entity.Property(e => e.DireccionCliente).IsRequired().HasMaxLength(80);
                entity.Property(e => e.Fecha).IsRequired();
                entity.HasOne(d => d.Cliente)
                    .WithMany(p => p.Recibos)
                    .HasForeignKey(d => d.ClienteId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
            modelBuilder.Entity<RegistroVenta>(entity =>
            {
                entity.HasIndex(e => e.Id);
                entity.Property(e => e.Fecha).IsRequired();
                entity.Property(e => e.NombreCliente).IsRequired().HasMaxLength(80);  // en realidad es dirección
                entity.HasOne(d => d.Cliente)
                    .WithMany(p => p.RegistroVentas)
                    .HasForeignKey(d => d.ClienteId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
            modelBuilder.Entity<Reparto>(entity =>
            {
                entity.HasIndex(e => e.Id);
                entity.Property(e => e.Nombre).IsRequired().HasMaxLength(40);
                //entity.Property(e => e.Ta).HasColumnName("TA");
                //entity.Property(e => e.Tae).HasColumnName("TAE");
                //entity.Property(e => e.Td).HasColumnName("TD");
                //entity.Property(e => e.Te).HasColumnName("TE");
                //entity.Property(e => e.Tl).HasColumnName("TL");
                //entity.Property(e => e.Tt).HasColumnName("TT");
                entity.HasOne(d => d.DiaReparto)
                    .WithMany(p => p.Repartos)
                    .HasForeignKey(d => d.DiaRepartoId)
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasIndex(e => new { e.DiaRepartoId, e.Nombre }).IsUnique();
            });
            modelBuilder.Entity<Venta>(entity =>
            {
                entity.HasIndex(e => e.Id);
                entity.HasIndex(e => e.ProductoId).IsUnique();
                entity.HasOne(v => v.Producto)
                    .WithMany(p => p.Ventas)
                    .HasForeignKey(v => v.ProductoId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
            OnModelCreatingPartial(modelBuilder);
        }
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
