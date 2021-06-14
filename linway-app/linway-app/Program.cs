using AutoMapper;
using linway_app.Forms;
using linway_app.Models.DbContexts;
using linway_app.Models.Entities.Mapping;
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

            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddScoped(typeof(IServiceBase<>), typeof(ServiceBase<>));
            services.AddScoped(typeof(IRepository<>), typeof(RepositoryBase<>));

            var mapperConfig = new MapperConfiguration(m =>
            {
                m.AddProfile(new MappingProfile());
            });
            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);
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
