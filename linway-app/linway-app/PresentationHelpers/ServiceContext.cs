using linway_app.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace linway_app.PresentationHelpers
{
    public static class ServiceContext
    {
        public static (
            IClienteServices ClienteServices,
            IDetalleReciboServices DetalleReciboServices,
            IDiaRepartoServices DiaRepartoServices,
            IExportarServices ExportarServices,
            INotaDeEnvioServices NotaDeEnvioServices,
            IPedidoServices PedidoServices,
            IProductoServices ProductoServices,
            IProdVendidoServices ProdVendidoServices,
            IReciboServices ReciboServices,
            IRegistroVentaServices RegistroVentaServices,
            IRepartoServices RepartoServices,
            ISavingServices SavingServices,
            IVentaServices VentaServices
        ) Get(IServiceProvider sp)
        {
            var clienteServices = sp.GetRequiredService<IClienteServices>();
            var detalleReciboServices = sp.GetRequiredService<IDetalleReciboServices>();
            var diaRepartoServices = sp.GetRequiredService<IDiaRepartoServices>();
            var exportarServices = sp.GetRequiredService<IExportarServices>();
            var notaDeEnvioServices = sp.GetRequiredService<INotaDeEnvioServices>();
            var pedidoServices = sp.GetRequiredService<IPedidoServices>();
            var productoServices = sp.GetRequiredService<IProductoServices>();
            var prodVendidoServices = sp.GetRequiredService<IProdVendidoServices>();
            var reciboServices = sp.GetRequiredService<IReciboServices>();
            var registroVentaServices = sp.GetRequiredService<IRegistroVentaServices>();
            var repartoServices = sp.GetRequiredService<IRepartoServices>();
            var savingServices = sp.GetRequiredService<ISavingServices>();
            var ventaServices = sp.GetRequiredService<IVentaServices>();
            return (
                clienteServices,
                detalleReciboServices,
                diaRepartoServices,
                exportarServices,
                notaDeEnvioServices,
                pedidoServices,
                productoServices,
                prodVendidoServices,
                reciboServices,
                registroVentaServices,
                repartoServices,
                savingServices,
                ventaServices
            );
        }
    }
}
