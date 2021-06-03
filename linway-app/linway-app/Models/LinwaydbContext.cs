using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace linway_app.Models
{
    public partial class LinwaydbContext : DbContext
    {
        public LinwaydbContext()
        {
        }

        public LinwaydbContext(DbContextOptions<LinwaydbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Cliente> Cliente { get; set; }
        public virtual DbSet<DetalleRecibo> DetalleRecibo { get; set; }
        public virtual DbSet<DiaReparto> DiaReparto { get; set; }
        public virtual DbSet<NotaDeEnvio> NotaDeEnvio { get; set; }
        public virtual DbSet<Pedido> Pedido { get; set; }
        public virtual DbSet<ProdVendido> ProdVendido { get; set; }
        public virtual DbSet<Producto> Producto { get; set; }
        public virtual DbSet<Recibo> Recibo { get; set; }
        public virtual DbSet<RegistroVenta> RegistroVenta { get; set; }
        public virtual DbSet<Reparto> Reparto { get; set; }
        public virtual DbSet<Venta> Venta { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlite(@"DataSource=linway-db.db");
                optionsBuilder.UseLazyLoadingProxies();
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cliente>(entity =>
            {
                entity.HasIndex(e => e.Id);

                entity.Property(e => e.Cuit).HasColumnName("CUIT");

                entity.Property(e => e.Direccion).IsRequired();
            });

            modelBuilder.Entity<DetalleRecibo>(entity =>
            {
                entity.HasIndex(e => e.Id);

                entity.Property(e => e.Detalle).IsRequired();

                entity.HasOne(d => d.Recibo)
                    .WithMany(p => p.DetalleRecibos)
                    .HasForeignKey(d => d.ReciboId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<DiaReparto>(entity =>
            {
                entity.HasIndex(e => e.Id);

                entity.Property(e => e.Dia).IsRequired();
            });

            modelBuilder.Entity<NotaDeEnvio>(entity =>
            {
                entity.HasIndex(e => e.Id);

                entity.Property(e => e.Detalle).IsRequired();

                entity.Property(e => e.Fecha).IsRequired();

                entity.HasOne(d => d.Client)
                    .WithMany(p => p.NotaDeEnvio)
                    .HasForeignKey(d => d.ClientId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<Pedido>(entity =>
            {
                entity.HasIndex(e => e.Id);

                entity.Property(e => e.Ae).HasColumnName("AE");

                entity.Property(e => e.Direccion).IsRequired();

                entity.HasOne(d => d.Cliente)
                    .WithMany(p => p.Pedido)
                    .HasForeignKey(d => d.ClienteId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Reparto)
                    .WithMany(p => p.Pedidos)
                    .HasForeignKey(d => d.RepartoId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
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
                    .WithMany(p => p.ProdVendido)
                    .HasForeignKey(d => d.ProductoId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.RegistroVenta)
                    .WithMany(p => p.ProdVendido)
                    .HasForeignKey(d => d.RegistroVentaId);
            });

            modelBuilder.Entity<Producto>(entity =>
            {
                entity.HasIndex(e => e.Id);

                entity.Property(e => e.Nombre).IsRequired();

                entity.Property(e => e.Estado);
            });

            modelBuilder.Entity<Recibo>(entity =>
            {
                entity.HasIndex(e => e.Id);

                entity.Property(e => e.DireccionCliente).IsRequired();

                entity.Property(e => e.Fecha).IsRequired();

                entity.HasOne(d => d.Cliente)
                    .WithMany(p => p.Recibo)
                    .HasForeignKey(d => d.ClienteId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<RegistroVenta>(entity =>
            {
                entity.HasIndex(e => e.Id);

                entity.Property(e => e.Fecha).IsRequired();

                entity.Property(e => e.NombreCliente).IsRequired();

                entity.HasOne(d => d.Cliente)
                    .WithMany(p => p.RegistroVenta)
                    .HasForeignKey(d => d.ClienteId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<Reparto>(entity =>
            {
                entity.HasIndex(e => e.Id);

                entity.Property(e => e.Nombre).IsRequired();

                entity.Property(e => e.Ta).HasColumnName("TA");

                entity.Property(e => e.Tae).HasColumnName("TAE");

                entity.Property(e => e.Td).HasColumnName("TD");

                entity.Property(e => e.Te).HasColumnName("TE");

                entity.Property(e => e.Tl).HasColumnName("TL");

                entity.Property(e => e.Tt).HasColumnName("TT");

                entity.HasOne(d => d.DiaReparto)
                    .WithMany(p => p.Reparto)
                    .HasForeignKey(d => d.DiaRepartoId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<Venta>(entity =>
            {
                entity.HasIndex(e => e.Id);

                entity.HasOne(d => d.Producto)
                    .WithMany(p => p.Venta)
                    .HasForeignKey(d => d.ProductoId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
