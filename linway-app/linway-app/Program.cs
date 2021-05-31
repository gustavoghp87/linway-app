using linway_app.Forms;
using linway_app.Models;
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
            services.AddDbContext<LinwaydbContext>();

            services.AddScoped<ICliente, Cliente>();
            services.AddScoped<IDiaReparto, DiaReparto>();
            services.AddScoped<INotaDeEnvio, NotaDeEnvio>();
            services.AddScoped<IProducto, Producto>();
            services.AddScoped<IProdVendido, ProdVendido>();
            services.AddScoped<IRecibo, Recibo>();
            services.AddScoped<IDetalleRecibo, DetalleRecibo>();
            services.AddScoped<IRegistroVenta, RegistroVenta>();
            services.AddScoped<IReparto, Reparto>();
            services.AddScoped<IVenta, Venta>();

            services.AddScoped<IRepository<Cliente>, RepositoryCliente>();
            services.AddScoped<IRepository<DiaReparto>, RepositoryDiaReparto>();
            services.AddScoped<IRepository<NotaDeEnvio>, RepositoryNotaDeEnvio>();
            services.AddScoped<IRepository<Producto>, RepositoryProducto>();
            services.AddScoped<IRepository<ProdVendido>, RepositoryProdVendido>();
            services.AddScoped<IRepository<Recibo>, RepositoryRecibo>();
            services.AddScoped<IRepository<RegistroVenta>, RepositoryRegistroVenta>();
            services.AddScoped<IRepository<Reparto>, RepositoryReparto>();
            services.AddScoped<IRepository<Venta>, RepositoryVenta>();

            services.AddScoped<IServicioCliente, ServicioCliente>();
            services.AddScoped<IServicioDiaReparto, ServicioDiaReparto>();
            services.AddScoped<IServicioNotaDeEnvio, ServicioNotaDeEnvio>();
            services.AddScoped<IServicioProducto, ServicioProducto>();
            services.AddScoped<IServicioProdVendido, ServicioProdVendido>();
            services.AddScoped<IServicioRecibo, ServicioRecibo>();
            services.AddScoped<IServicioDetalleRecibo, ServicioDetalleRecibo>();
            services.AddScoped<IServicioRegistroVenta, ServicioRegistroVenta>();
            services.AddScoped<IServicioReparto, ServicioReparto>();
            services.AddScoped<IServicioVenta, ServicioVenta>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddTransient<IUnitOfWork, UnitOfWork>();

            services.AddScoped<Form1>();
            services.AddScoped<FormClientes>();
            services.AddScoped<FormCrearNota>();
            services.AddScoped<FormImprimirNota>();
            services.AddScoped<FormImprimirRecibo>();
            services.AddScoped<FormNotasEnvio>();
            services.AddScoped<FormProductos>();
            services.AddScoped<FormRecibos>();
            services.AddScoped<FormReparto>();
            services.AddScoped<FormVentas>();
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
