using InfrasServices.Services;
using linway_app.Services.Interfaces;
using Models;
using Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using static linway_app.Services.Delegates.DCliente;
using static linway_app.Services.Delegates.DProducto;
using static linway_app.Services.Delegates.DProdVendido;
using static linway_app.Services.Delegates.DReparto;

namespace linway_app.Services.Delegates
{
    public static class DPedido
    {
        public readonly static Action<Pedido> addPedido = AddPedido;
        public readonly static Func<long, long, long> addPedidoIfNotExistsAndReturnId = AddPedidoIfNotExistsAndReturnId;
        public readonly static Action<ICollection<Pedido>> cleanPedidos = CleanPedidos;
        public readonly static Action<Pedido> deletePedido = DeletePedido;
        public readonly static Action<ICollection<Pedido>> editPedidos = EditPedidos;
        public readonly static Func<long, Pedido> getPedido = GetPedido;
        public readonly static Func<ICollection<Pedido>> getPedidos = GetPedidos;
        public readonly static Func<long, ICollection<Pedido>> getPedidosPorRepartoId = GetPedidosPorRepartoId;
        public readonly static Func<Pedido, bool, Pedido> updatePedido = UpdatePedido;

        private static readonly IServiceBase<Pedido> _service = ServicesObjects.ServPedido;

        private static void AddPedido(Pedido pedido)
        {
            pedido.Orden = GetOrdenMayor(pedido.RepartoId) + 1;
            bool response = _service.Add(pedido);
            if (!response) Console.WriteLine("Algo falló al agregar nuevo Pedido a la base de datos");
        }
        private static long AddPedidoIfNotExistsAndReturnId(long repartoId, long clienteId)
        {
            Reparto reparto = getReparto(repartoId);
            if (reparto == null) return 0;
            Pedido pedido = reparto.Pedidos.ToList().Find(x => x.ClienteId == clienteId);
            if (pedido == null)
            {
                Cliente cliente = getCliente(clienteId);
                if (cliente == null) return 0;
                pedido = new Pedido()
                {
                    ClienteId = cliente.Id,
                    RepartoId = reparto.Id,
                    Direccion = cliente.Direccion,
                    Entregar = 1,
                    ProductosText = "",
                    L = 0,
                    A = 0,
                    Ae = 0,
                    D = 0,
                    E = 0,
                    T = 0
                };
                AddPedido(pedido);
                reparto = getReparto(repartoId);
                pedido = reparto.Pedidos.ToList().Find(x => x.ClienteId == clienteId);
                if (pedido == null) return 0;
            }
            return pedido.Id;
        }
        private static void CleanPedidos(ICollection<Pedido> pedidos)
        {
            if (pedidos == null || pedidos.Count == 0) return;
            foreach (Pedido pedido in pedidos)
            {
                pedido.Entregar = 0;
                pedido.L = 0;
                pedido.ProductosText = "";
                pedido.A = 0;
                pedido.E = 0;
                pedido.D = 0;
                pedido.T = 0;
                pedido.Ae = 0;
            }
            EditPedidos(pedidos);
        }
        private static void DeletePedido(Pedido pedido)
        {
            bool response = _service.Delete(pedido);
            if (!response) Console.WriteLine("Algo falló al eliminar el Pedido de la base de datos");
        }
        private static void EditPedidos(ICollection<Pedido> pedidos)
        {
            if (pedidos == null || pedidos.Count == 0) return;
            bool response = _service.EditMany(pedidos);
            if (!response) Console.WriteLine("Algo falló al editar los Pedidos en la base de datos");
        }
        private static Pedido GetPedido(long pedidoId)
        {
            return _service.Get(pedidoId);
        }
        private static ICollection<Pedido> GetPedidosPorRepartoId(long repartoId)
        {
            var pedidos = GetPedidos();
            if (pedidos == null || pedidos.Count == 0) return new List<Pedido>();
            pedidos = pedidos.Where(x => x.RepartoId == repartoId && x.Estado != null && x.Estado != "Eliminado").ToList();
            return pedidos;
        }
        private static ICollection<Pedido> GetPedidos()
        {
            return _service.GetAll();
        }
        private static long GetOrdenMayor(long repartoId)
        {
            var lstPedidos = GetPedidosPorRepartoId(repartoId);
            if (lstPedidos == null || lstPedidos.Count == 0) return 1;
            long lastOrden = lstPedidos.OrderBy(x => x.Orden).Last().Orden;
            return lastOrden;
        }
        private static Pedido UpdatePedido(Pedido pedido, bool entregar)
        {
            if (pedido == null || pedido.ProdVendidos == null || pedido.ProdVendidos.Count == 0) return pedido;
            pedido.ProductosText = "";
            pedido.A = 0;
            pedido.Ae = 0;
            pedido.D = 0;
            pedido.E = 0;
            pedido.L = 0;
            pedido.T = 0;
            pedido.ProdVendidos.ToList().ForEach(prodVendido =>
            {
                string description =
                    esProducto(prodVendido.Producto) ? editDescripcion(prodVendido.Descripcion) : prodVendido.Descripcion;

                if (isPolvo(prodVendido.Producto) && !isBlanqueador(prodVendido.Producto))
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
                else if (!isSaldo(prodVendido.Producto))
                {
                    if (isLiquido(prodVendido.Producto))
                    {
                        pedido.L += prodVendido.Cantidad;
                    }

                    if (isBlanqueador(prodVendido.Producto))
                        pedido.ProductosText += prodVendido.Cantidad.ToString() + " kg " + description + " | ";
                    else
                        pedido.ProductosText += prodVendido.Cantidad.ToString() + "x " + description + " | ";
                }
                else if (isACobrar(prodVendido.Producto))
                {
                    pedido.ProductosText += "A cobrar | ";
                }
            });
            if (pedido.ProductosText.Length > 3)
            {
                var lastThree = pedido.ProductosText.Substring(pedido.ProductosText.Length - 3, 3);
                var cleansed = pedido.ProductosText.Substring(0, pedido.ProductosText.Length - 3);
                if (lastThree == " | ") pedido.ProductosText = cleansed;
            }
            pedido.Entregar = entregar ? 1 : 0;
            EditPedidos(new List<Pedido>() { pedido });
            updateReparto(getReparto(pedido.RepartoId));
            return pedido;
        }
    }
}
