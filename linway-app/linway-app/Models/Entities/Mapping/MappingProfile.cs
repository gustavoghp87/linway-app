using AutoMapper;

namespace linway_app.Models.Entities.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Cliente, ECliente>();
            CreateMap<DetalleRecibo, EDetalleRecibo>();
            CreateMap<NotaDeEnvio, ENotaDeEnvio>()
                .ForMember(x => x.Direccion, origin => origin.MapFrom(z => z.Cliente.Direccion))
                .ForMember(x => x.Impresa, origin => origin.MapFrom(z => z.Impresa == 1 ? "Sí" : "No"));
            CreateMap<Pedido, EPedido>()
                .ForMember(x => x.Entregar, origin => origin.MapFrom(z => z.Entregar == 1 ? "Sí" : "No"));
            CreateMap<Producto, EProducto>();
            CreateMap<ProdVendido, EProdVendido>()
                .ForMember(x => x.Total, origin => origin.MapFrom(z => z.Precio * z.Cantidad));
            CreateMap<Recibo, ERecibo>().ForMember(x => x.Impreso,
                origin => origin.MapFrom(z => z.Impreso == 1 ? "Sí" : "No"));
            CreateMap<RegistroVenta, ERegistroVenta>();
            CreateMap<Venta, EVenta>()
                .ForMember(x => x.Detalle, origin => origin.MapFrom(z => z.Producto.Nombre));
        }
    }
}
