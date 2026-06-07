using AppServices.EntityServices;
using AppServices.Excel;
using AppServices.Interfaces;
using AppServices.Mapping;
using AppServices.Services;
using AppServices.UseCases;
using AutoMapper;
using Infrastructure.Repositories;
using Infrastructure.Repositories.DbContexts;
using Infrastructure.Repositories.Interfaces;
using AppLinway.Forms;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows.Forms;

namespace AppLinway
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
            services.AddScoped<IAgregarClienteUseCase, AgregarClienteUseCase>();
            services.AddScoped<IAgregarNotaDeEnvioUseCase, AgregarNotaDeEnvioUseCase>();
            services.AddScoped<IAgregarPedidoUseCase, AgregarPedidoUseCase>();
            services.AddScoped<IAgregarProductoUseCase, AgregarProductoUseCase>();
            services.AddScoped<IAgregarProdVendidoANotaDeEnvioUseCase, AgregarProdVendidoANotaDeEnvioUseCase>();
            services.AddScoped<IAgregarReciboUseCase, AgregarReciboUseCase>();
            services.AddScoped<IAgregarRegistroVentaUseCase, AgregarRegistroVentaUseCase>();
            services.AddScoped<IAgregarRepartoUseCase, AgregarRepartoUseCase>();
            services.AddScoped<IEditarClienteUseCase, EditarClienteUseCase>();
            services.AddScoped<IEditarProductoUseCase, EditarProductoUseCase>();
            services.AddScoped<IEliminarClienteUseCase, EliminarClienteUseCase>();
            services.AddScoped<IEliminarNotaDeEnvioUseCase, EliminarNotaDeEnvioUseCase>();
            services.AddScoped<IEliminarPedidoUseCase, EliminarPedidoUseCase>();
            services.AddScoped<IEliminarProductoUseCase, EliminarProductoUseCase>();
            services.AddScoped<IEliminarRecibosUseCase, EliminarRecibosUseCase>();
            services.AddScoped<IEliminarRegistroVentasUseCase, EliminarRegistroVentasUseCase>();
            services.AddScoped<IEliminarRegistroVentaUseCase, EliminarRegistroVentaUseCase>();
            services.AddScoped<IEliminarRepartoUseCase, EliminarRepartoUseCase>();
            services.AddScoped<IEliminarVentasUseCase, EliminarVentasUseCase>();
            services.AddScoped<IEnviarNotaDeEnvioARepartoUseCase, EnviarNotaDeEnvioARepartoUseCase>();
            services.AddScoped<ILimpiarDiaRepartoUseCase, LimpiarDiaRepartoUseCase>();
            services.AddScoped<ILimpiarPedidoUseCase, LimpiarPedidoUseCase>();
            services.AddScoped<ILimpiarRepartosUseCase, LimpiarRepartosUseCase>();
            services.AddScoped<ILimpiarRepartoUseCase, LimpiarRepartoUseCase>();
            services.AddScoped<IMarcarNotaDeEnvioComoImprimidaUseCase, MarcarNotaDeEnvioComoImprimidaUseCase>();
            services.AddScoped<IMarcarReciboComoImprimidoUseCase, MarcarReciboComoImprimidoUseCase>();
            services.AddScoped<IQuitarProdVendidoDeNotaDeEnvioUseCase, QuitarProdVendidoDeNotaDeEnvioUseCase>();
            services.AddScoped<IReposicionarRepartoUseCase, ReposicionarRepartoUseCase>();
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
