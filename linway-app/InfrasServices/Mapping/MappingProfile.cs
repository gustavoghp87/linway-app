using AutoMapper;
using linway_app.Services.FormServices;
using Models.Enums;
using System.Linq;

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
                .ForMember(x => x.Direccion, origin => origin.MapFrom(z => z.Cliente.Direccion))
                .ForMember(x => x.Entregar, origin => origin.MapFrom(z => z.Entregar == 1 ? "Sí" : "No"))
                .AfterMap((src, dest) => SetValoresDePedido(src, dest));
            // Producto
            CreateMap<Producto, EProducto>();
            // Producto Vendido
            CreateMap<ProdVendido, EProdVendido>()
                .ForMember(x => x.Total, origin => origin.MapFrom(z => z.Precio * z.Cantidad));
            // Recibo
            CreateMap<Recibo, ERecibo>()
                .ForMember(x => x.Impreso, origin => origin.MapFrom(z => z.Impreso == 1 ? "Sí" : "No"));
            // Registro de Venta
            CreateMap<RegistroVenta, ERegistroVenta>();
            // Reparto
            CreateMap<Reparto, EReparto>()
                .AfterMap((src, dest) => SetValoresDeReparto(src, dest));
            // Venta
            CreateMap<Venta, EVenta>()
                .ForMember(x => x.Detalle, origin => origin.MapFrom(z => z.Producto.Nombre));
        }
        public static EPedido GetEPedido(Pedido pedido)
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            var mapper = config.CreateMapper();
            var ePedido = mapper.Map<EPedido>(pedido);
            return ePedido;
        }
        public static EReparto GetEReparto(Reparto reparto)
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            var mapper = config.CreateMapper();
            var eReparto = mapper.Map<EReparto>(reparto);
            return eReparto;
        }
        private static void SetValoresDePedido(Pedido src, EPedido dest)
        {
            dest.ProductosText = "";
            dest.A = 0;
            dest.Ae = 0;
            dest.D = 0;
            dest.E = 0;
            dest.L = 0;
            dest.T = 0;
            var gruposPorProducto = src.ProdVendidos.GroupBy(pv => pv.Producto.Id);  // se agrupa para que Productos repetidos no tengan Descripción repetida
            foreach (var grupo in gruposPorProducto)
            {
                ProdVendido referencia = grupo.First();
                Producto producto = referencia.Producto;
                long cantidadTotal = grupo.Sum(x => x.Cantidad);
                string description = ProductoServices.IsProducto(producto)
                    ? ProdVendidoServices.GetEditedDescripcion(referencia.Descripcion)
                    : referencia.Descripcion;
                if (ProductoServices.IsPolvo(producto) && !ProductoServices.IsBlanqueador(producto))
                {
                    int kilos = 20;
                    long cantidadDeBolsas = cantidadTotal / kilos;
                    switch (producto.SubTipo)
                    {
                        case string a when a == TipoPolvo.AlisonEspecial.ToString():
                            dest.Ae += cantidadDeBolsas;
                            break;
                        case string a when a == TipoPolvo.Alison.ToString():
                            dest.A += cantidadDeBolsas;
                            break;
                        case string a when a == TipoPolvo.Dispersán.ToString():
                            dest.D += cantidadDeBolsas;
                            break;
                        case string a when a == TipoPolvo.Texapol.ToString():
                            dest.T += cantidadDeBolsas;
                            break;
                        case string a when a == TipoPolvo.Eslabón.ToString():
                            dest.E += cantidadDeBolsas;
                            break;
                    }
                    dest.ProductosText += cantidadDeBolsas + "x20 " + description + " | ";
                }
                else if (!ProductoServices.IsSaldo(producto))
                {
                    if (ProductoServices.IsLiquido(producto))
                    {
                        dest.L += cantidadTotal;
                    }

                    if (ProductoServices.IsBlanqueador(producto))
                    {
                        dest.ProductosText += cantidadTotal.ToString() + " kg " + description + " | ";
                    }
                    else
                    {
                        dest.ProductosText += cantidadTotal.ToString() + "x " + description + " | ";
                    }
                }
                else if (ProductoServices.IsACobrar(producto))
                {
                    dest.ProductosText += "A cobrar | ";
                }
            }
            if (dest.ProductosText.Length > 3)
            {
                var ultimosTres = dest.ProductosText.Substring(dest.ProductosText.Length - 3, 3);
                if (ultimosTres == " | ")
                {
                    dest.ProductosText = dest.ProductosText.Substring(0, dest.ProductosText.Length - 3);
                }
            }
        }
        private static void SetValoresDeReparto(Reparto src, EReparto dest)
        {
            dest.Ta = 0;
            dest.Te = 0;
            dest.Tt = 0;
            dest.Tae = 0;
            dest.Td = 0;
            dest.TotalB = 0;
            dest.Tl = 0;
            src.Pedidos.ToList().ForEach(pedido =>
            {
                EPedido ePedido = GetEPedido(pedido);
                dest.Ta += ePedido.A;
                dest.Te += ePedido.E;
                dest.Tt += ePedido.T;
                dest.Tae += ePedido.Ae;
                dest.Td += ePedido.D;
                dest.TotalB += ePedido.A + ePedido.E + ePedido.T + ePedido.Ae + ePedido.D;
                dest.Tl += ePedido.L;
            });
        }
    }
}
