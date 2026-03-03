using linway_app.Services.Interfaces;
using Models;
using Models.Enums;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace linway_app.Services.FormServices
{
    public class OrquestacionServices: IOrquestacionServices
    {
        private readonly IClienteServices _clienteServices;
        private readonly IDiaRepartoServices _diaRepartoServices;
        private readonly IExportarServices _exportarServices;
        private readonly INotaDeEnvioServices _notaDeEnvioServices;
        private readonly IPedidoServices _pedidoServices;
        private readonly IProductoServices _productoServices;
        private readonly IProdVendidoServices _prodVendidoServices;
        private readonly IRepartoServices _repartoServices;
        private readonly IVentaServices _ventaServices;
        public OrquestacionServices(IClienteServices clienteServices, IDiaRepartoServices diaRepartoServices, IExportarServices exportarServices, INotaDeEnvioServices notaDeEnvioServices, IPedidoServices pedidoServices, IProductoServices productoServices, IProdVendidoServices prodVendidoServices, IRepartoServices repartoServices, IVentaServices ventaServices)
        {
            _clienteServices = clienteServices;
            _diaRepartoServices = diaRepartoServices;
            _exportarServices = exportarServices;
            _notaDeEnvioServices = notaDeEnvioServices;
            _pedidoServices = pedidoServices;
            _productoServices = productoServices;
            _prodVendidoServices = prodVendidoServices;
            _repartoServices = repartoServices;
            _ventaServices = ventaServices;
        }
        public async Task<Pedido> GetPedidoPorRepartoYClienteGenerarSiNoExisteAsync(long repartoId, long clienteId)
        {
            Reparto reparto = await _repartoServices.GetRepartoPorIdAsync(repartoId);
            if (reparto == null)
            {
                return null;
            }
            Pedido pedido = reparto.Pedidos.ToList().Find(x => x.ClienteId == clienteId && x.Estado != "Eliminado");
            if (pedido == null)
            {
                Cliente cliente = await _clienteServices.GetClientePorIdAsync(clienteId);
                if (cliente == null)
                {
                    return null;
                }
                pedido = new Pedido()
                {
                    ClienteId = cliente.Id,
                    RepartoId = reparto.Id,
                    Direccion = cliente.Direccion,
                    Entregar = 1,
                    Estado = "Activo",
                    ProductosText = "",
                    L = 0,
                    A = 0,
                    Ae = 0,
                    D = 0,
                    E = 0,
                    T = 0
                };
                //await _pedidoServices.AddPedido(pedido);
            }
            return pedido;
        }
        public void CleanRepartos(ICollection<Reparto> repartos)
        {
            if (repartos == null || repartos.Count == 0)
            {
                return;
            }
            var pedidosAEditar = new List<Pedido>();
            var prodVendidosAEditar = new List<ProdVendido>();
            foreach (Reparto reparto in repartos)
            {
                reparto.Ta = 0;
                reparto.Te = 0;
                reparto.Td = 0;
                reparto.Tt = 0;
                reparto.Tae = 0;
                reparto.TotalB = 0;
                reparto.Tl = 0;
                pedidosAEditar.AddRange(reparto.Pedidos);
                foreach (Pedido pedido in reparto.Pedidos)
                {
                    if (pedido.ProdVendidos == null || pedido.ProdVendidos.Count == 0)
                    {
                        continue;
                    }
                    foreach (ProdVendido prodVendido in pedido.ProdVendidos)
                    {
                        prodVendido.PedidoId = null;
                        prodVendidosAEditar.Add(prodVendido);
                    }
                }
            }
            _repartoServices.EditRepartos(repartos);
            _pedidoServices.CleanPedidos(pedidosAEditar);
            _prodVendidoServices.EditProdVendidos(prodVendidosAEditar);
        }
        public async Task EditClienteYDireccionEnPedidosAsync(Cliente cliente)
        {
            _clienteServices.EditCliente(cliente);
            List<DiaReparto> dias = await _diaRepartoServices.GetDiaRepartos();
            foreach (var dia in dias)
            {
                foreach (var reparto in dia.Reparto)
                {
                    foreach (var pedido in reparto.Pedidos)
                    {
                        if (pedido.ClienteId == cliente.Id)
                        {
                            pedido.Direccion = cliente.Direccion;
                            _pedidoServices.EditPedidos(new List<Pedido>() { pedido });
                        }
                    }
                }
            }
        }
        public NotaDeEnvio EditNotaDeEnvioQuitar(NotaDeEnvio notaDeEnvio, ProdVendido prodVendidoAEliminar)
        {
            _prodVendidoServices.DeleteProdVendido(prodVendidoAEliminar);
            var lstAuxiliar = new List<ProdVendido>();
            foreach (ProdVendido prodVendido in notaDeEnvio.ProdVendidos)
            {
                if (prodVendido.ProductoId != prodVendidoAEliminar.ProductoId)
                {
                    lstAuxiliar.Add(prodVendido);
                }
            }
            notaDeEnvio.ProdVendidos = lstAuxiliar;
            notaDeEnvio.ImporteTotal = NotaDeEnvioServices.ExtraerImporteDeNotaDeEnvio(lstAuxiliar);
            notaDeEnvio.Detalle = NotaDeEnvioServices.ExtraerDetalleDeNotaDeEnvio(lstAuxiliar);
            _notaDeEnvioServices.EditNotaDeEnvio(notaDeEnvio);
            return notaDeEnvio;
        }
        public async Task ExportRepartoAsync(string dia, string nombreReparto)
        {
            Reparto reparto = await GetRepartoPorDiaYNombreAsync(dia, nombreReparto);
            _exportarServices.ExportarReparto(reparto);
        }
        public async Task<Reparto> GetRepartoPorDiaYNombreAsync(string dia, string nombre)
        {
            List<DiaReparto> lstDiasRep = await _diaRepartoServices.GetDiaRepartos();
            if (lstDiasRep == null)
            {
                return null;
            }
            Reparto reparto = lstDiasRep
                .Find(x => x.Dia == dia && x.Estado != null && x.Estado != "Eliminado").Reparto.ToList()
                .Find(x => x.Nombre == nombre && x.Estado != null && x.Estado != "Eliminado");
            return reparto;
        }
        public async Task<List<Reparto>> GetRepartosPorDiaAsync(string diaReparto)
        {
            List<DiaReparto> lstDiasRep = await _diaRepartoServices.GetDiaRepartos();
            List<Reparto> lstRepartos = lstDiasRep
                .Find(x => x.Dia == diaReparto && x.Estado != null && x.Estado != "Eliminado").Reparto.ToList();
            return lstRepartos;
        }
        public async Task UpdatePedidoAsync(Pedido pedido, bool entregar)
        {
            if (pedido == null || pedido.ProdVendidos == null || pedido.ProdVendidos.Count == 0)
            {
                return;
            }
            pedido.ProductosText = "";
            pedido.A = 0;
            pedido.Ae = 0;
            pedido.D = 0;
            pedido.E = 0;
            pedido.L = 0;
            pedido.T = 0;
            pedido.ProdVendidos.ToList().ForEach(prodVendido =>
            {
                string description = ProductoServices.IsProducto(prodVendido.Producto)
                    ? ProdVendidoServices.GetEditedDescripcion(prodVendido.Descripcion)
                    : prodVendido.Descripcion
                ;
                if (ProductoServices.IsPolvo(prodVendido.Producto) && !ProductoServices.IsBlanqueador(prodVendido.Producto))
                {
                    int kilos = 20;
                    long cantidadDeBolsas = prodVendido.Cantidad / kilos;
                    switch (prodVendido.Producto.SubTipo)
                    {
                        case string a when a == TipoPolvo.AlisonEspecial.ToString():
                            pedido.Ae += cantidadDeBolsas;
                            break;
                        case string a when a == TipoPolvo.Alison.ToString():
                            pedido.A += cantidadDeBolsas;
                            break;
                        case string a when a == TipoPolvo.Dispersán.ToString():
                            pedido.D += cantidadDeBolsas;
                            break;
                        case string a when a == TipoPolvo.Texapol.ToString():
                            pedido.T += cantidadDeBolsas;
                            break;
                        case string a when a == TipoPolvo.Eslabón.ToString():
                            pedido.E += cantidadDeBolsas;
                            break;
                        default:
                            break;
                    }
                    pedido.ProductosText += cantidadDeBolsas + "x20 " + description + " | ";
                }
                else if (!ProductoServices.IsSaldo(prodVendido.Producto))
                {
                    if (ProductoServices.IsLiquido(prodVendido.Producto))
                    {
                        pedido.L += prodVendido.Cantidad;
                    }
                    if (ProductoServices.IsBlanqueador(prodVendido.Producto))
                    {
                        pedido.ProductosText += prodVendido.Cantidad.ToString() + " kg " + description + " | ";
                    }
                    else
                    {
                        pedido.ProductosText += prodVendido.Cantidad.ToString() + "x " + description + " | ";
                    }
                }
                else if (ProductoServices.IsACobrar(prodVendido.Producto))
                {
                    pedido.ProductosText += "A cobrar | ";
                }
            });
            if (pedido.ProductosText.Length > 3)
            {
                var lastThree = pedido.ProductosText.Substring(pedido.ProductosText.Length - 3, 3);
                var cleansed = pedido.ProductosText.Substring(0, pedido.ProductosText.Length - 3);
                if (lastThree == " | ")
                {
                    pedido.ProductosText = cleansed;
                }
            }
            pedido.Entregar = entregar ? 1 : 0;
            _pedidoServices.EditPedidos(new List<Pedido>() { pedido });
            Reparto reparto = await _repartoServices.GetRepartoPorIdAsync(pedido.RepartoId);
            _repartoServices.UpdateReparto(reparto);
        }
        public async Task UpdateVentasDesdeProdVendidosAsync(ICollection<ProdVendido> prodVendidos, bool addingUp)
        {
            if (prodVendidos == null)
            {
                return;
            }
            var ventasAAgregar = new List<Venta>();
            var ventasAEditar = new List<Venta>();
            List<Venta> lstVentas = await _ventaServices.GetVentasAsync();
            List<Producto> productos = await _productoServices.GetProductosAsync();
            if (lstVentas == null || productos == null)
            {
                return;
            }
            foreach (ProdVendido prodVendido in prodVendidos)
            {
                Producto producto = productos.Find(x => x.Id == prodVendido.ProductoId);
                if (producto == null || !ProductoServices.IsProducto(producto))
                {
                    continue;
                }
                bool exists = false;
                foreach (var venta in lstVentas)
                {
                    if (exists)
                    {
                        continue;
                    }
                    if (venta.ProductoId == prodVendido.ProductoId)
                    {
                        exists = true;
                        venta.Cantidad = addingUp ? venta.Cantidad + prodVendido.Cantidad : venta.Cantidad - prodVendido.Cantidad;
                        ventasAEditar.Add(venta);
                    }
                }
                if (!exists && addingUp)
                {
                    Venta nuevaVenta = new Venta
                    {
                        ProductoId = prodVendido.ProductoId,
                        Cantidad = prodVendido.Cantidad
                    };
                    ventasAAgregar.Add(nuevaVenta);
                }
            }
            if (ventasAAgregar.Count > 0)
            {
                _ventaServices.AddVentas(ventasAAgregar);
            }
            if (ventasAEditar.Count > 0)
            {
                _ventaServices.EditVentas(ventasAEditar);
            }
        }
    }
}
