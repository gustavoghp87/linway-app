using linway_app.Forms;
using linway_app.Models;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace linway_app.Services.Delegates
{
    public static class DPedido
    {
        public delegate void DAddPedido(Pedido pedido);
        public delegate void DAddPedidoDesdeNota(string diaDeReparto, string nombreReparto, long notaDeEnvioId);
        public delegate void DDeletePedido(Pedido pedido);
        public delegate void DEditPedido(Pedido pedido);
        public delegate Pedido DGetPedidoPorDireccion(string direccion);
        public delegate List<Pedido> DGetPedidos();
        public delegate List<Pedido> DGetPedidosPorRepartoId(long repartoId);

        public readonly static DAddPedido addPedido
            = new DAddPedido(AddPedido);
        public readonly static DAddPedidoDesdeNota addPedidoDesdeNota
            = new DAddPedidoDesdeNota(AddPedidoDesdeNota);
        public readonly static DDeletePedido deletePedido
            = new DDeletePedido(DeletePedido);
        public readonly static DEditPedido editPedido
            = new DEditPedido(EditPedido);
        public readonly static DGetPedidoPorDireccion getPedidoPorDireccion
            = new DGetPedidoPorDireccion(GetPedidoPorDireccion);
        public readonly static DGetPedidos getPedidos
            = new DGetPedidos(GetPedidos);
        public readonly static DGetPedidosPorRepartoId getPedidosPorRepartoId
            = new DGetPedidosPorRepartoId(GetPedidosPorRepartoId);

        private static void AddPedido(Pedido pedido)
        {
            pedido.Orden = 1000;
            bool response = Form1._servPedido.Add(pedido);
            if (!response) MessageBox.Show("Algo falló al agregar nuevo Pedido a la base de datos");
        }
        private static void AddPedidoDesdeNota(string diaDeReparto, string nombreReparto, long notaDeEnvioId)
        {
            bool response = AgregarDesdeNota(diaDeReparto, nombreReparto, notaDeEnvioId);
            if (!response) MessageBox.Show("Algo falló al agregar Nota de Envío a la base de datos");
        }
        private static void DeletePedido(Pedido pedido)
        {
            bool response = Form1._servPedido.Delete(pedido);
            if (!response) MessageBox.Show("Algo falló al eliminar el Pedido de la base de datos");
        }
        private static void EditPedido(Pedido pedido)
        {
            bool response = Form1._servPedido.Edit(pedido);
            if (!response) MessageBox.Show("Algo falló al editar el Pedido en la base de datos");
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
            return Form1._servPedido.GetAll();
        }

        private static bool AgregarDesdeNota(string diaDeReparto, string nombreReparto, long notaDeEnvioId)
        {
            NotaDeEnvio nota = Form1._servNotaDeEnvio.Get(notaDeEnvioId);
            List<DiaReparto> dias = Form1._servDiaReparto.GetAll();
            DiaReparto dia = dias.Find(x => x.Dia == diaDeReparto);
            Reparto reparto = dia.Reparto.ToList().Find(x => x.Nombre == nombreReparto);
            Pedido pedido = reparto.Pedidos.ToList().Find(x => x.ClienteId == nota.ClienteId);

            if (pedido == null) // no existe pedido para este cliente este día y reparto, se hace pedido
            {
                Pedido nuevoPedido = new Pedido();
                nuevoPedido.ClienteId = nota.ClienteId;
                nuevoPedido.Cliente = nota.Cliente;
                nuevoPedido.RepartoId = reparto.Id;
                nuevoPedido.Direccion = nota.Cliente.Direccion;
                nuevoPedido.Entregar = 1;
                nuevoPedido.ProductosText = "";
                nuevoPedido.ProdVendidos = nota.ProdVendidos;
                foreach (var prodVendido in nota.ProdVendidos)
                {
                    nuevoPedido.ProductosText += prodVendido.Descripcion + " | ";
                }
                AddPedido(nuevoPedido);
            }
            else
            {
                foreach (var prodVendido in nota.ProdVendidos)
                {
                    pedido.ProductosText += prodVendido.Descripcion + " | ";
                    pedido.ProdVendidos.Add(prodVendido);
                }
                Form1._servPedido.Edit(pedido);
            }

            return true;
        }
    }
}
