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
            services.AddTransient<FormRepartos>();
            services.AddTransient<FormVentas>();

            services.AddScoped(typeof(IService<>), typeof(Service<>));
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
