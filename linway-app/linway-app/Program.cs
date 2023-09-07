using AutoMapper;
using Infrastructure.Repositories;
using Infrastructure.Repositories.DbContexts;
using Infrastructure.Repositories.Interfaces;
using linway_app.Forms;
using linway_app.Services;
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
        private static ServiceProvider _serviceProvider;

        private static void ConfigureServices(IServiceCollection services)
        {
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
            services.AddDbContext<LinwayDbContext>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddTransient(typeof(IServiceBase<>), typeof(ServiceBase<>));    //antes scoped agosto 2023
            services.AddTransient(typeof(IRepository<>), typeof(RepositoryBase<>));  //antes scoped agosto 2023
            var mapperConfig = new MapperConfiguration(m =>
            {
                m.AddProfile(new MappingProfile());
            });
            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);
        }

        public static ServiceProvider GetConfig()
        {
            return _serviceProvider;
        }

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            ConfigureServices(_services);
            _serviceProvider = _services.BuildServiceProvider();
            //SqliteToMysqlConverter.ConvertSqliteToMysql();
            var form1 = _serviceProvider.GetRequiredService<Form1>();
            Application.Run(form1);
        }
    }
}
