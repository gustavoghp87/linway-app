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
        public async Task AddAsync(Pedido pedido)
        {
            pedido.Orden = await GetOrdenAsync(pedido.RepartoId);
            _services.Add(pedido);
        }
        public void Delete(Pedido pedido)
        {
            _services.Delete(pedido);
        }
        public void DeleteMany(ICollection<Pedido> pedidos)
        {
            _services.DeleteMany(pedidos);
        }
        public void Edit(Pedido pedido)
        {
            _services.Edit(pedido);
        }
        public void EditMany(ICollection<Pedido> pedidos)
        {
            _services.EditMany(pedidos);
        }
        public async Task<Pedido> GetPorIdAsync(long pedidoId)
        {
            Pedido pedido = await _services.GetAsync(pedidoId);
            return pedido;
        }
        public async Task<List<Pedido>> GetPorRepartoIdAsync(long repartoId)
        {
            var pedidos = await GetAllAsync();
            if (pedidos == null)
            {
                return new List<Pedido>();
            }
            pedidos = pedidos.Where(x => x.RepartoId == repartoId).ToList();
            // ver:
            //List<Cliente> clientes = getClientes();
            //foreach (Pedido pedido in pedidos)
            //{
            //    pedido.Direccion = clientes.Find(x => x.Id == pedido.ClienteId)?.Direccion;
            //};
            return pedidos;
        }
        public async Task<List<Pedido>> GetAllAsync()
        {
            List<Pedido> pedidos = await _services.GetAllAsync();
            return pedidos;
        }
        #region private methods
        private async Task<long> GetOrdenAsync(long repartoId)
        {
            var lstPedidos = await GetPorRepartoIdAsync(repartoId);
            if (lstPedidos == null || lstPedidos.Count == 0)
            {
                return 1;
            }
            long lastOrden = lstPedidos.OrderBy(x => x.Orden).Last().Orden;
            return lastOrden + 1;
        }
        #endregion
        #region static methods
        public static void ActualizarCantidadesYDescripcionDePedido(Pedido pedido, bool entregar)  // primero pedido, luego reparto
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
            var gruposPorProducto = pedido.ProdVendidos.GroupBy(pv => pv.Producto.Id);  // se agrupa para que Productos repetidos no tengan Descripción repetida
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
                    }
                    pedido.ProductosText += cantidadDeBolsas + "x20 " + description + " | ";
                }
                else if (!ProductoServices.IsSaldo(producto))
                {
                    if (ProductoServices.IsLiquido(producto))
                    {
                        pedido.L += cantidadTotal;
                    }

                    if (ProductoServices.IsBlanqueador(producto))
                    {
                        pedido.ProductosText += cantidadTotal.ToString() + " kg " + description + " | ";
                    }
                    else
                    {
                        pedido.ProductosText += cantidadTotal.ToString() + "x " + description + " | ";
                    }
                }
                else if (ProductoServices.IsACobrar(producto))
                {
                    pedido.ProductosText += "A cobrar | ";
                }
            }
            if (pedido.ProductosText.Length > 3)
            {
                var ultimosTres = pedido.ProductosText.Substring(pedido.ProductosText.Length - 3, 3);
                if (ultimosTres == " | ")
                {
                    pedido.ProductosText = pedido.ProductosText.Substring(0, pedido.ProductosText.Length - 3);
                }
            }
            pedido.Entregar = entregar ? 1 : 0;
        }
        public static Pedido GetNuevoPedido(Cliente cliente, Reparto reparto)
        {
            return new Pedido()
            {
                Cliente = cliente,
                Direccion = cliente.Direccion,
                Reparto = reparto,
                Entregar = 1,
                ProductosText = "",
                L = 0,
                A = 0,
                Ae = 0,
                D = 0,
                E = 0,
                T = 0
            };
        }
        #endregion
    }
}
