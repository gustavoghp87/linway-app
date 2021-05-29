using linway_app.Forms;
using linway_app.Models;
using linway_app.Repositories;
using linway_app.Repositories.Interfaces;
using linway_app.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows.Forms;

namespace linway_app
{
    static class Program
    {
        static void ConfigureServices(ServiceCollection services)
        {
            services.AddDbContext<LinwaydbContext>();

            services.AddScoped<IProducto, Producto>();
            services.AddScoped<IPedido, Pedido>();
            services.AddScoped<IProdVendido, ProdVendido>();

            services.AddScoped<IRepository<Cliente>, RepositoryCliente>();
            services.AddScoped<IRepository<Producto>, RepositoryProducto>();
            services.AddScoped<IRepository<NotaDeEnvio>, RepositoryNotaDeEnvio>();
            services.AddScoped<IRepository<Venta>, RepositoryVenta>();
            services.AddScoped<IRepository<RegistroVenta>, RepositoryRegistroVenta>();
            services.AddScoped<IRepository<Recibo>, RepositoryRecibo>();
            services.AddScoped<IRepository<Reparto>, RepositoryReparto>();


            services.AddScoped<IServicioCliente, ServicioCliente>();
            services.AddScoped<IServicioProducto, ServicioProducto>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();


            services.AddTransient<IUnitOfWork, UnitOfWork>();

            services.AddScoped<Form1>();
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
