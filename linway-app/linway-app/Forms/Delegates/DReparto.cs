using linway_app.Models;
using System.Collections.Generic;
using System.Windows.Forms;

namespace linway_app.Forms.Delegates
{
    public static class DReparto
    {
        public delegate void DAddPedidoARepartoPorNota(Reparto reparto, Cliente cliente,
            List<ProdVendido> lstProdVendidos);

        public readonly static DAddPedidoARepartoPorNota addPedidoARepartoPorNota
            = new DAddPedidoARepartoPorNota(AddPedidoARepartoPorNota);

        public static void AddPedidoARepartoPorNota(Reparto reparto, Cliente cliente,
            List<ProdVendido> lstProdVendidos)
        {
            bool response = Form1._servReparto.AgregarPedidoAReparto(cliente.Id,
                reparto.DiaReparto.Dia, reparto.Nombre, lstProdVendidos);
            if (!response) MessageBox.Show("Algo falló al agregar Pedido a Reparto en base de datos");
        }
    }
}
