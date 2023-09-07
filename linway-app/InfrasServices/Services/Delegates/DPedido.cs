using InfrasServices.Services;
using linway_app.Services.Interfaces;
using Models;
using Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using static linway_app.Services.Delegates.DCliente;
using static linway_app.Services.Delegates.DDiaReparto;
using static linway_app.Services.Delegates.DProducto;
using static linway_app.Services.Delegates.DProdVendido;
using static linway_app.Services.Delegates.DReparto;

namespace linway_app.Services.Delegates
{
    public static class DPedido
    {
        public readonly static Predicate<Pedido> addPedido = AddPedido;
        public readonly static Func<long, long, long> addPedidoIfNotExistsAndReturnId = AddPedidoIfNotExistsAndReturnId;
        public readonly static Predicate<ICollection<Pedido>> cleanPedidos = CleanPedidos;
        public readonly static Predicate<Pedido> deletePedido = DeletePedido;
        public readonly static Predicate<ICollection<Pedido>> editPedidos = EditPedidos;
        public readonly static Func<long, Pedido> getPedido = GetPedido;
        public readonly static Func<ICollection<Pedido>> getPedidos = GetPedidos;
        public readonly static Func<long, ICollection<Pedido>> getPedidosPorRepartoId = GetPedidosPorRepartoId;
        public readonly static Predicate<Cliente> editDireccionClienteEnPedidos = EditDireccionClienteEnPedidos;
        public readonly static Func<Pedido, bool, Pedido> updatePedido = UpdatePedido;

        private static readonly IServiceBase<Pedido> _service = ServicesObjects.ServPedido;

        private static bool AddPedido(Pedido pedido)
        {
            pedido.Orden = GetOrdenMayor(pedido.RepartoId) + 1;
            bool success = _service.Add(pedido);
            return success;
        }
        private static long AddPedidoIfNotExistsAndReturnId(long repartoId, long clienteId)
        {
            Reparto reparto = getReparto(repartoId);
            if (reparto == null) return 0;
            Pedido pedido = reparto.Pedidos.ToList().Find(x => x.ClienteId == clienteId && x.Estado != "Eliminado");
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
                bool success = AddPedido(pedido);
                if (!success)
                {
                    Console.WriteLine("Algo falló al agregar nuevo Pedido a la base de datos (2)");
                }
            }
            long pedidoId = pedido == null ? 0 : pedido.Id;
            return pedidoId;
        }
        private static bool CleanPedidos(ICollection<Pedido> pedidos)
        {
            if (pedidos == null || pedidos.Count == 0) return false;
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
            bool success = EditPedidos(pedidos);
            return success;
        }
        private static bool DeletePedido(Pedido pedido)
        {
            bool success = _service.Delete(pedido);
            return success;
        }
        private static bool EditDireccionClienteEnPedidos(Cliente cliente)
        {
            List<DiaReparto> dias = getDiaRepartos();
            bool success = true;
            foreach (var dia in dias)
            {
                foreach (var reparto in dia.Reparto)
                {
                    foreach (var pedido in reparto.Pedidos)
                    {
                        if (pedido.ClienteId == cliente.Id)
                        {
                            pedido.Direccion = cliente.Direccion;
                            bool successUpdate = EditPedidos(new List<Pedido>() { pedido });
                            if (!successUpdate) success = false;
                        }
                    }
                }
            }
            return success;
        }
        private static bool EditPedidos(ICollection<Pedido> pedidos)
        {
            if (pedidos == null || pedidos.Count == 0) return false;
            bool success = _service.EditMany(pedidos);
            return success;
        }
        private static Pedido GetPedido(long pedidoId)
        {
            Pedido pedido = _service.Get(pedidoId);
            return pedido;
        }
        private static ICollection<Pedido> GetPedidosPorRepartoId(long repartoId)
        {
            var pedidos = GetPedidos();
            if (pedidos == null || pedidos.Count == 0) return new List<Pedido>();
            pedidos = pedidos.Where(x => x.RepartoId == repartoId && x.Estado != null && x.Estado != "Eliminado").ToList();
            // ver:
            //List<Cliente> clientes = getClientes();
            //foreach (Pedido pedido in pedidos)
            //{
            //    pedido.Direccion = clientes.Find(x => x.Id == pedido.ClienteId)?.Direccion;
            //};
            return pedidos;
        }
        private static ICollection<Pedido> GetPedidos()
        {
            ICollection<Pedido> pedidos = _service.GetAll();
            return pedidos;
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
            bool success = EditPedidos(new List<Pedido>() { pedido });
            if (!success) Console.WriteLine("No se editó el Pedido");
            Reparto reparto = getReparto(pedido.RepartoId);
            success = updateReparto(reparto);
            if (!success) Console.WriteLine("No se actualizó el Pedido");
            return pedido;
        }
    }
}
