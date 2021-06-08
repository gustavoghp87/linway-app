using linway_app.Forms;
using linway_app.Models.DbContexts;
using linway_app.Repositories;
using linway_app.Repositories.Interfaces;
using linway_app.Services;
using linway_app.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows.Forms;

namespace linway_app
{
    static class Program
    {
        public static void ConfigureServices(ServiceCollection services)
        {
            services.AddDbContext<LinwayDbContext>();

            //services.AddScoped<ICliente, Cliente>();
            //services.AddScoped<IDiaReparto, DiaReparto>();
            //services.AddScoped<INotaDeEnvio, NotaDeEnvio>();
            //services.AddScoped<IPedido, Pedido>();
            //services.AddScoped<IProducto, Producto>();
            //services.AddScoped<IProdVendido, ProdVendido>();
            //services.AddScoped<IRecibo, Recibo>();
            //services.AddScoped<IDetalleRecibo, DetalleRecibo>();
            //services.AddScoped<IRegistroVenta, RegistroVenta>();
            //services.AddScoped<IReparto, Reparto>();
            //services.AddScoped<IVenta, Venta>();

            services.AddTransient<Form1>();
            services.AddTransient<FormClientes>();
            services.AddTransient<FormCrearNota>();
            services.AddTransient<FormImprimirNota>();
            services.AddTransient<FormImprimirRecibo>();
            services.AddTransient<FormNotasEnvio>();
            services.AddTransient<FormProductos>();
            services.AddTransient<FormRecibos>();
            services.AddTransient<FormReparto>();
            services.AddTransient<FormVentas>();

            services.AddTransient<IServicioCliente, ServicioCliente>();
            services.AddTransient<IServicioDiaReparto, ServicioDiaReparto>();
            services.AddTransient<IServicioNotaDeEnvio, ServicioNotaDeEnvio>();
            services.AddTransient<IServicioPedido, ServicioPedido>();
            services.AddTransient<IServicioProducto, ServicioProducto>();
            services.AddTransient<IServicioProdVendido, ServicioProdVendido>();
            services.AddTransient<IServicioRecibo, ServicioRecibo>();
            services.AddTransient<IServicioDetalleRecibo, ServicioDetalleRecibo>();
            services.AddTransient<IServicioRegistroVenta, ServicioRegistroVenta>();
            services.AddTransient<IServicioReparto, ServicioReparto>();
            services.AddTransient<IServicioVenta, ServicioVenta>();

            //services.AddScoped<IRepository<Cliente>, RepositoryCliente>();
            //services.AddScoped<IRepository<DiaReparto>, RepositoryDiaReparto>();
            //services.AddScoped<IRepository<NotaDeEnvio>, RepositoryNotaDeEnvio>();
            //services.AddScoped<IRepository<Pedido>, RepositoryPedido>();
            //services.AddScoped<IRepository<Producto>, RepositoryProducto>();
            //services.AddScoped<IRepository<ProdVendido>, RepositoryProdVendido>();
            //services.AddScoped<IRepository<Recibo>, RepositoryRecibo>();
            //services.AddScoped<IRepository<RegistroVenta>, RepositoryRegistroVenta>();
            //services.AddScoped<IRepository<Reparto>, RepositoryReparto>();
            //services.AddScoped<IRepository<Venta>, RepositoryVenta>();
            services.AddScoped(typeof(IRepository<>), typeof(RepositoryBase<>));

            services.AddTransient<IUnitOfWork, UnitOfWork>();
        }

        public static ServiceProvider GetConfig()
        {
            ServiceCollection services = new ServiceCollection();
            ConfigureServices(services);
            ServiceProvider serviceProvider = services.BuildServiceProvider();
            return serviceProvider;
        }

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            var services = new ServiceCollection();
            ConfigureServices(services);
            using (ServiceProvider serviceProvider = services.BuildServiceProvider())
            {
                var form1 = serviceProvider.GetRequiredService<Form1>();
                Application.Run(form1);
            }
        }
    }
}
