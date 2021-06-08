using linway_app.Forms;
using linway_app.Models;
using System.Windows.Forms;

namespace linway_app.Services.Delegates
{
    public static class DPedido
    {
        public delegate void DAddPedido(Pedido pedido);
        public delegate void DAddPedidoDesdeNota(string diaDeReparto, string nombreReparto, long notaDeEnvioId);
        public delegate void DDeletePedido(Pedido pedido);
        public delegate void DEditPedido(Pedido pedido);

        public readonly static DAddPedido addPedido
            = new DAddPedido(AddPedido);
        public readonly static DAddPedidoDesdeNota addPedidoDesdeNota
            = new DAddPedidoDesdeNota(AddPedidoDesdeNota);
        public readonly static DDeletePedido deletePedido
            = new DDeletePedido(DeletePedido);
        public readonly static DEditPedido editPedido
            = new DEditPedido(EditPedido);

        private static void AddPedido(Pedido pedido)
        {
            bool response = Form1._servPedido.Add(pedido);
            if (!response) MessageBox.Show("Algo falló al agregar nuevo Pedido a la base de datos");
        }
        private static void AddPedidoDesdeNota(string diaDeReparto, string nombreReparto, long notaDeEnvioId)
        {
            bool response = Form1._servPedido.AgregarDesdeNota(diaDeReparto, nombreReparto, notaDeEnvioId);
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
    }
}
