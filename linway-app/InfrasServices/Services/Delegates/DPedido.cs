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
        public readonly static Action<Reparto, Cliente, List<ProdVendido>> addOrEditPedidoEnReparto = AddOrEditPedidoEnReparto;
        public readonly static Action<ICollection<Pedido>> cleanPedidos = CleanPedidos;
        public readonly static Action<Pedido> deletePedido = DeletePedido;
        public readonly static Action<Pedido> editPedido = EditPedido;
        public readonly static Func<string, Pedido> getPedidoPorDireccion = GetPedidoPorDireccion;
        public readonly static Func<ICollection<Pedido>> getPedidos = GetPedidos;
        public readonly static Func<long, ICollection<Pedido>> getPedidosPorRepartoId = GetPedidosPorRepartoId;

        private static readonly IServiceBase<Pedido> _service = ServicesObjects.ServPedido;
        private static void AddPedido(Pedido pedido)
        {
            pedido.Orden = GetOrdenMayor(pedido.RepartoId) + 1;
            bool response = _service.Add(pedido);
            if (!response) Console.WriteLine("Algo falló al agregar nuevo Pedido a la base de datos");
        }
        private static void AddOrEditPedidoEnReparto(Reparto reparto, Cliente cliente, List<ProdVendido> lstProdVendidos)
        {
            bool response = AgregarOEditarPedido(cliente.Id, reparto.DiaReparto.Dia, reparto.Nombre, lstProdVendidos);
            if (!response) Console.WriteLine("Algo falló al agregar Pedido a Reparto en base de datos");
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
        private static void EditPedido(Pedido pedido)
        {
            bool response = _service.Edit(pedido);
            if (!response) Console.WriteLine("Algo falló al editar el Pedido en la base de datos");
        }
        private static void EditPedidos(ICollection<Pedido> pedidos)
        {
            bool response = _service.EditMany(pedidos);
            if (!response) Console.WriteLine("Algo falló al editar los Pedidos en la base de datos");
        }
        private static Pedido GetPedido(long pedidoId)
        {
            return _service.Get(pedidoId);
        }
        private static Pedido GetPedidoPorDireccion(string direccion)
        {
            var lstPedidos = getPedidos();
            if (lstPedidos == null) return null;
            return lstPedidos.ToList().Find(x => x.Direccion.Equals(direccion) && x.Estado != null && x.Estado != "Eliminado");
        }
        private static ICollection<Pedido> GetPedidosPorRepartoId(long repartoId)
        {
            var pedidos = getPedidos();
            return pedidos.Where(x => x.RepartoId == repartoId && x.Estado != null && x.Estado != "Eliminado").ToList();
        }
        private static ICollection<Pedido> GetPedidos()
        {
            return _service.GetAll();
        }

        private static bool AgregarOEditarPedido(long clientId, string dia, string repartoNombre, List<ProdVendido> lstProdVendidos)
        {
            if (lstProdVendidos == null || lstProdVendidos.Count == 0) return false;
            Cliente cliente = getCliente(clientId);
            if (cliente == null) return false;
            Reparto reparto = getRepartoPorDiaYNombre(dia, repartoNombre);
            if (reparto == null) return false;
            Pedido pedidoViejo = reparto.Pedidos.ToList().Find(x => x.ClienteId == cliente.Id);

            if (pedidoViejo == null)
            {
                Pedido nuevoPedido = new Pedido
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
                foreach (var prodVendido in lstProdVendidos)
                {
                    nuevoPedido = AgregarProdVendido(nuevoPedido, prodVendido);
                }
                if (nuevoPedido == null)
                {
                    Console.WriteLine("Falló agregar a reparto");
                    return false;
                }
                addPedido(nuevoPedido);
                addPedidoARepartoSimple(reparto, nuevoPedido);
            }
            else
            {
                //substractPedidoAReparto(reparto, pedidoViejo);
                foreach (var prodVendido in lstProdVendidos)
                {
                    pedidoViejo = AgregarProdVendido(pedidoViejo, prodVendido);
                }
                if (pedidoViejo == null)
                {
                    Console.WriteLine("Falló agregar a reparto");
                    return false;
                }
                pedidoViejo.Entregar = 1;
                editPedido(pedidoViejo);
                pedidoViejo = GetPedido(pedidoViejo.Id);
                addPedidoARepartoSimple(reparto, pedidoViejo);
            }
            return true;
        }
        private static Pedido AgregarProdVendido(Pedido pedido, ProdVendido prodVendido)
        {
            //if (!esProducto(prodVendido.Producto)) return null;

            string description = "";
            if (esProducto(prodVendido.Producto)) { description = editDescripcion(prodVendido.Descripcion); }

            if (prodVendido.Producto.Tipo == TipoProducto.Polvo.ToString() && prodVendido.Producto.SubTipo != null
                 && prodVendido.Producto.SubTipo != TipoPolvo.Blanqueador.ToString())
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
            else if (prodVendido.Producto.Tipo != TipoProducto.Saldo.ToString())
            {
                if (prodVendido.Producto.Tipo == TipoProducto.Líquido.ToString())
                {
                    pedido.L += prodVendido.Cantidad;
                }
                
                if (prodVendido.Producto.Tipo == TipoProducto.Polvo.ToString()
                     && prodVendido.Producto.SubTipo == TipoPolvo.Blanqueador.ToString())
                    pedido.ProductosText += prodVendido.Cantidad.ToString() + " kilos " + description + " | ";
                else
                    pedido.ProductosText += prodVendido.Cantidad.ToString() + "x " + description + " | ";
            }
            else if (prodVendido.Producto.SubTipo != null && prodVendido.Producto.SubTipo == TipoSaldo.ACobrar.ToString())
            {
                pedido.ProductosText += "A cobrar | ";
            }

            return pedido;
        }
        private static long GetOrdenMayor(long repartoId)
        {
            var lstPedidos = GetPedidosPorRepartoId(repartoId);
            if (lstPedidos == null || lstPedidos.Count == 0) return 1;
            long lastOrden = lstPedidos.OrderBy(x => x.Orden).Last().Orden;
            return lastOrden;
        }
    }
}
