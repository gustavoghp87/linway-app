using linway_app.Services.Interfaces;
using Models;
using Models.Enums;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace linway_app.Services.FormServices
{
    public class PedidoServices: IPedidoServices
    {
        private readonly IServicesBase<Pedido> _services;
        public PedidoServices(IServicesBase<Pedido> services)
        {
            _services = services;
        }
        public async Task AddPedidoAsync(Pedido pedido)
        {
            pedido.Orden = await GetOrdenAsync(pedido.RepartoId);
            _services.Add(pedido);
        }
        public void DeletePedido(Pedido pedido)
        {
            _services.Delete(pedido);
        }
        public void EditPedido(Pedido pedido)
        {
            _services.Edit(pedido);
        }
        public void EditPedidos(ICollection<Pedido> pedidos)
        {
            _services.EditMany(pedidos);
        }
        public async Task<Pedido> GetPedidoPorIdAsync(long pedidoId)
        {
            Pedido pedido = await _services.GetAsync(pedidoId);
            return pedido;
        }
        public async Task<ICollection<Pedido>> GetPedidosPorRepartoIdAsync(long repartoId)
        {
            var pedidos = await GetPedidosAsync();
            if (pedidos == null)
            {
                return new List<Pedido>();
            }
            pedidos = pedidos.Where(x => x.RepartoId == repartoId && x.Estado != "Eliminado").ToList();
            // ver:
            //List<Cliente> clientes = getClientes();
            //foreach (Pedido pedido in pedidos)
            //{
            //    pedido.Direccion = clientes.Find(x => x.Id == pedido.ClienteId)?.Direccion;
            //};
            return pedidos;
        }
        public async Task<ICollection<Pedido>> GetPedidosAsync()
        {
            ICollection<Pedido> pedidos = await _services.GetAllAsync();
            return pedidos;
        }
        #region private methods
        private async Task<long> GetOrdenAsync(long repartoId)
        {
            var lstPedidos = await GetPedidosPorRepartoIdAsync(repartoId);
            if (lstPedidos == null || lstPedidos.Count == 0)
            {
                return 1;
            }
            long lastOrden = lstPedidos.OrderBy(x => x.Orden).Last().Orden;
            return lastOrden + 1;
        }
        #endregion
        #region static methods
        public static void ActualizarEtiquetasDePedido(Pedido pedido, bool entregar)
        {
            if (pedido == null)
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
                    : prodVendido.Descripcion;
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
        }
        #endregion
    }
}
