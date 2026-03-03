using AutoMapper;
using Infrastructure.Repositories;
using Infrastructure.Repositories.DbContexts;
using Infrastructure.Repositories.Interfaces;
using linway_app.Forms;
using linway_app.Services;
using linway_app.Services.Excel;
using linway_app.Services.FormServices;
using linway_app.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Models.Entities.Mapping;
using System;
using System.Windows.Forms;

namespace linway_app
{
    static class Program
    {
        private static readonly ServiceCollection _services = new ServiceCollection();
        public static ServiceProvider LinwayServiceProvider;

        private static void ConfigureServices(IServiceCollection services)
        {
            var mapperConfig = new MapperConfiguration(m =>
            {
                m.AddProfile(new MappingProfile());
            });
            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);
            //
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
            //
            services.AddDbContext<LinwayDbContext>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped(typeof(IRepository<>), typeof(RepositoryBase<>));
            services.AddScoped(typeof(IServicesBase<>), typeof(ServicesBase<>));
            //
            services.AddScoped<IClienteServices, ClienteServices>();
            services.AddScoped<IDetalleReciboServices, DetalleReciboServices>();
            services.AddScoped<IDiaRepartoServices, DiaRepartoServices>();
            services.AddScoped<IExportarServices, ExportarServices>();
            services.AddScoped<ISavingServices, SavingServices>();
            services.AddScoped<INotaDeEnvioServices, NotaDeEnvioServices>();
            services.AddScoped<IPedidoServices, PedidoServices>();
            services.AddScoped<IProductoServices, ProductoServices>();
            services.AddScoped<IProdVendidoServices, ProdVendidoServices>();
            services.AddScoped<IReciboServices, ReciboServices>();
            services.AddScoped<IRegistroVentaServices, RegistroVentaServices>();
            services.AddScoped<IRepartoServices, RepartoServices>();
            services.AddScoped<IVentaServices, VentaServices>();
            //
            services.AddScoped<IOrquestacionServices, OrquestacionServices>();
            //
            LinwayServiceProvider = services.BuildServiceProvider();
        }

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            ConfigureServices(_services);
            //_serviceProvider = _services.BuildServiceProvider();
            //DbAutoBackup.Generate();
            var form1 = LinwayServiceProvider.GetRequiredService<Form1>();
            Application.Run(form1);
        }
    }
}
