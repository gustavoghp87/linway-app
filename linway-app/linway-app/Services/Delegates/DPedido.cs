using linway_app.Forms;
using linway_app.Models;
using linway_app.Models.Enums;
using linway_app.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using static linway_app.Services.Delegates.DCliente;
using static linway_app.Services.Delegates.DNotaDeEnvio;
using static linway_app.Services.Delegates.DProducto;
using static linway_app.Services.Delegates.DProdVendido;
using static linway_app.Services.Delegates.DReparto;

namespace linway_app.Services.Delegates
{
    public static class DPedido
    {
        public readonly static Action<Pedido> addPedido = AddPedido;
        public readonly static Action<Reparto, Cliente, List<ProdVendido>> addPedidoAReparto = AddPedidoAReparto;
        public readonly static Action<string, string, long> addPedidoDesdeNota = AddPedidoDesdeNota;
        public readonly static Action<Pedido> cleanPedido = CleanPedido;
        public readonly static Action<Pedido> deletePedido = DeletePedido;
        public readonly static Action<Pedido> editPedido = EditPedido;
        public readonly static Func<string, Pedido> getPedidoPorDireccion = GetPedidoPorDireccion;
        public readonly static Func<List<Pedido>> getPedidos = GetPedidos;
        public readonly static Func<long, List<Pedido>> getPedidosPorRepartoId = GetPedidosPorRepartoId;

        private static readonly IServiceBase<Pedido> _service = Form1._servPedido;
        private static void AddPedido(Pedido pedido)
        {
            pedido.Orden = GetOrdenMayor(pedido.RepartoId) + 1;
            bool response =_service.Add(pedido);
            if (!response) MessageBox.Show("Algo falló al agregar nuevo Pedido a la base de datos");
        }
        private static void AddPedidoAReparto(Reparto reparto, Cliente cliente, List<ProdVendido> lstProdVendidos)
        {
            bool response = AgregarPedidoAReparto(cliente.Id, reparto.DiaReparto.Dia, reparto.Nombre, lstProdVendidos);
            if (!response) MessageBox.Show("Algo falló al agregar Pedido a Reparto en base de datos");
        }
        private static void AddPedidoDesdeNota(string diaDeReparto, string nombreReparto, long notaDeEnvioId)
        {
            bool response = AgregarDesdeNota(diaDeReparto, nombreReparto, notaDeEnvioId);
            if (!response) MessageBox.Show("Algo falló al agregar Nota de Envío a la base de datos");
        }
        private static void CleanPedido(Pedido pedido)
        {
            pedido.Entregar = 0;
            pedido.L = 0;
            pedido.ProductosText = "";
            pedido.A = 0;
            pedido.E = 0;
            pedido.D = 0;
            pedido.T = 0;
            pedido.Ae = 0;
            editPedido(pedido);
            foreach (ProdVendido prodVendido in pedido.ProdVendidos)
            {
                prodVendido.PedidoId = null;
                editProdVendido(prodVendido);
            }
        }
        private static void DeletePedido(Pedido pedido)
        {
            bool response = _service.Delete(pedido);
            if (!response) MessageBox.Show("Algo falló al eliminar el Pedido de la base de datos");
        }
        private static void EditPedido(Pedido pedido)
        {
            bool response = _service.Edit(pedido);
            if (!response) MessageBox.Show("Algo falló al editar el Pedido en la base de datos");
        }
        private static Pedido GetPedido(long pedidoId)
        {
            return _service.Get(pedidoId);
        }
        private static Pedido GetPedidoPorDireccion(string direccion)
        {
            List<Pedido> lstPedidos = GetPedidos();
            if (lstPedidos == null) return null;
            return lstPedidos.Find(x => x.Direccion.Equals(direccion));
        }
        private static List<Pedido> GetPedidosPorRepartoId(long repartoId)
        {
            var pedidos = GetPedidos();
            return pedidos.Where(x => x.RepartoId == repartoId).ToList();
        }
        private static List<Pedido> GetPedidos()
        {
            return _service.GetAll();
        }

        private static bool AgregarDesdeNota(string diaDeReparto, string nombreReparto, long notaDeEnvioId)
        {
            NotaDeEnvio nota = getNotaDeEnvio(notaDeEnvioId);
            Reparto reparto = getRepartoPorDiaYNombre(diaDeReparto, nombreReparto);
            if (nota == null || reparto == null) return false;
            List<Pedido> lstPedidos = getPedidosPorRepartoId(reparto.Id);
            if (lstPedidos == null || lstPedidos.Count == 0) return false;
            Pedido pedidoViejo = lstPedidos.Find(x => x.ClienteId == nota.ClienteId);
            Common(pedidoViejo, reparto, nota.Cliente, nota.ProdVendidos.ToList());
            return true;
        }
        private static bool AgregarPedidoAReparto(long clientId, string dia, string repartoNombre, List<ProdVendido> lstProdVendidos)
        {
            if (lstProdVendidos == null || lstProdVendidos.Count == 0) return false;
            Cliente cliente = getCliente(clientId);
            if (cliente == null) return false;
            Reparto reparto = getRepartoPorDiaYNombre(dia, repartoNombre);
            if (reparto == null) return false;
            Pedido pedidoViejo = reparto.Pedidos.ToList().Find(x => x.ClienteId == cliente.Id);
            Common(pedidoViejo, reparto, cliente, lstProdVendidos);
            return true;
        }
        private static void Common(Pedido pedidoViejo, Reparto reparto, Cliente cliente, List<ProdVendido> lstProdVendidos)
        {
            if (pedidoViejo == null)
            {
                Pedido nuevoPedido = new Pedido();
                nuevoPedido.ClienteId = cliente.Id;
                nuevoPedido.RepartoId = reparto.Id;
                nuevoPedido.Direccion = cliente.Direccion;
                nuevoPedido.Entregar = 1;
                nuevoPedido.ProductosText = "";
                nuevoPedido.ProdVendidos = lstProdVendidos;
                nuevoPedido.A = 0;
                nuevoPedido.Ae = 0;
                nuevoPedido.D = 0;
                nuevoPedido.E = 0;
                nuevoPedido.L = 0;
                nuevoPedido.T = 0;
                foreach (var prodVendido in lstProdVendidos)
                {
                    AgregarProdVendidos(nuevoPedido, prodVendido);
                }
                AddPedido(nuevoPedido);
                addPedidoARepartoSimple(reparto, nuevoPedido);
            }
            else
            {
                substractPedidoAReparto(reparto, pedidoViejo);
                foreach (var prodVendido in lstProdVendidos)
                {
                    AgregarProdVendidos(pedidoViejo, prodVendido);
                }
                EditPedido(pedidoViejo);
                pedidoViejo = GetPedido(pedidoViejo.Id);
                addPedidoARepartoSimple(reparto, pedidoViejo);
            }
        }
        private static void AgregarProdVendidos(Pedido pedido, ProdVendido prodVendido)
        {
            if (!esProducto(prodVendido.Producto)) return;
            pedido.ProductosText += prodVendido.Cantidad.ToString() + "x " + prodVendido.Descripcion + " | ";
            if (prodVendido.Producto.Tipo == TipoProducto.Polvo.ToString() && prodVendido.Producto.SubTipo != null)
            {
                int kilos = 20;
                switch (prodVendido.Producto.SubTipo.ToLower())
                {
                    case string a when a == TipoPolvo.AlisonEspecial.ToString():
                        pedido.Ae += prodVendido.Cantidad/kilos;
                        break;
                    case string a when a == TipoPolvo.Alison.ToString():
                        pedido.A += prodVendido.Cantidad/kilos;
                        break;
                    case string a when a == TipoPolvo.Dispersán.ToString():
                        pedido.D += prodVendido.Cantidad/kilos;
                        break;
                    case string a when a == TipoPolvo.Texapol.ToString():
                        pedido.T += prodVendido.Cantidad/kilos;
                        break;
                    case string a when a == TipoPolvo.Eslabón.ToString():
                        pedido.E += prodVendido.Cantidad/kilos;
                        break;
                    default:
                        break;
                }
            }
            if (prodVendido.Producto.Tipo == TipoProducto.Líquido.ToString())
            {
                pedido.L += prodVendido.Cantidad;
            }
        }
        private static long GetOrdenMayor(long repartoId)
        {
            List<Pedido> lstPedidos = GetPedidosPorRepartoId(repartoId);
            if (lstPedidos == null || lstPedidos.Count == 0) return 1;
            long lastOrden = lstPedidos.OrderBy(x => x.Orden).Last().Orden;
            return lastOrden;
        }
    }
}
