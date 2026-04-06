using AutoMapper;

namespace Models.Entities.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Cliente
            CreateMap<Cliente, ECliente>();
            // Detalle de Recibo
            CreateMap<DetalleRecibo, EDetalleRecibo>();
            // Nota de Envío
            CreateMap<NotaDeEnvio, ENotaDeEnvio>()
                .ForMember(x => x.Direccion, origin => origin.MapFrom(z => z.Cliente.Direccion))
                .ForMember(x => x.Impresa, origin => origin.MapFrom(z => z.Impresa == 1 ? "Sí" : "No"));
            // Pedido
            CreateMap<Pedido, EPedido>()
                .ForMember(x => x.Entregar, origin => origin.MapFrom(z => z.Entregar == 1 ? "Sí" : "No"));
            // Producto
            CreateMap<Producto, EProducto>();
            // Producto Vendido
            CreateMap<ProdVendido, EProdVendido>()
                .ForMember(x => x.Total, origin => origin.MapFrom(z => z.Precio * z.Cantidad));
            // Recibo
            CreateMap<Recibo, ERecibo>().ForMember(x => x.Impreso,
                origin => origin.MapFrom(z => z.Impreso == 1 ? "Sí" : "No"));
            // Registro de Venta
            CreateMap<RegistroVenta, ERegistroVenta>();
            // Venta
            CreateMap<Venta, EVenta>()
                .ForMember(x => x.Detalle, origin => origin.MapFrom(z => z.Producto.Nombre));
        }
    }
}
