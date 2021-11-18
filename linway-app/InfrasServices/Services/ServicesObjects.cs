using linway_app.Services.Interfaces;
using Models;

namespace InfrasServices.Services
{
    public static class ServicesObjects
    {
        public static IServiceBase<Cliente> ServCliente;
        public static IServiceBase<DetalleRecibo> ServDetalleRecibo;
        public static IServiceBase<DiaReparto> ServDiaReparto;
        public static IServiceBase<NotaDeEnvio> ServNotaDeEnvio;
        public static IServiceBase<Pedido> ServPedido;
        public static IServiceBase<Producto> ServProducto;
        public static IServiceBase<ProdVendido> ServProdVendido;
        public static IServiceBase<Recibo> ServRecibo;
        public static IServiceBase<RegistroVenta> ServRegistroVenta;
        public static IServiceBase<Reparto> ServReparto;
        public static IServiceBase<Venta> ServVenta;
    }
}
