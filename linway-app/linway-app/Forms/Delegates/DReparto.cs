using linway_app.Models;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace linway_app.Forms.Delegates
{
    public static class DReparto
    {
        public delegate void DAddReparto(Reparto pedido);
        public delegate void DAddPedidoARepartoPorNota(Reparto reparto, Cliente cliente,
            List<ProdVendido> lstProdVendidos);
        public delegate List<Reparto> DGetRepartos();
        public delegate List<Reparto> DGetRepartosPorDia(string diaReparto);
        public delegate void DEditReparto(Reparto reparto);

        public readonly static DAddReparto addReparto
            = new DAddReparto(AddReparto);
        public readonly static DAddPedidoARepartoPorNota addPedidoARepartoPorNota
            = new DAddPedidoARepartoPorNota(AddPedidoARepartoPorNota);
        public readonly static DGetRepartos getRepartos
            = new DGetRepartos(GetRepartos);
        public readonly static DGetRepartosPorDia getRepartosPorDia
            = new DGetRepartosPorDia(GetRepartosPorDia);
        public readonly static DEditReparto editReparto
            = new DEditReparto(EditReparto);

        private static void AddReparto(Reparto reparto)
        {
            bool response = Form1._servReparto.Add(reparto);
            if (!response) MessageBox.Show("Algo falló al agregar nuevo Reparto a la base de datos");
        }
        private static void AddPedidoARepartoPorNota(Reparto reparto, Cliente cliente,
            List<ProdVendido> lstProdVendidos)
        {
            bool response = Form1._servReparto.AgregarPedidoAReparto(cliente.Id,
                reparto.DiaReparto.Dia, reparto.Nombre, lstProdVendidos);
            if (!response) MessageBox.Show("Algo falló al agregar Pedido a Reparto en base de datos");
        }
        private static List<Reparto> GetRepartos()
        {
            return Form1._servReparto.GetAll();
        }
        private static List<Reparto> GetRepartosPorDia(string diaReparto)
        {
            try
            {
                List<DiaReparto> lstDiasRep = Form1._servDiaReparto.GetAll();
                List<Reparto> lstRepartos = lstDiasRep.Find(x => x.Dia == diaReparto).Reparto.ToList();
                return lstRepartos;
            }
            catch
            {
                return null;
            }
        }
        private static void EditReparto(Reparto reparto)
        {
            bool response = Form1._servReparto.Edit(reparto);
            if (!response) MessageBox.Show("Algo falló al editar el Reparto en la base de datos");
        }
    }
}
