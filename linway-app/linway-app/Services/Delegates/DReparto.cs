using linway_app.Excel;
using linway_app.Forms;
using linway_app.Models;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using static linway_app.Services.Delegates.DPedido;

namespace linway_app.Services.Delegates
{
    public static class DReparto
    {
        public delegate void DAddPedidoARepartoSimple(Reparto reparto, Pedido pedido);
        public delegate void DAddReparto(Reparto reparto);
        public delegate Reparto DCleanReparto(Reparto reparto);
        public delegate void DEditReparto(Reparto reparto);
        public delegate bool DExportReparto(string dia, string nombreReparto);
        public delegate Reparto DGetReparto(long repartoId);
        public delegate Reparto DGetRepartoPorDiaYNombre(string dia, string nombre);
        public delegate List<Reparto> DGetRepartos();
        public delegate List<Reparto> DGetRepartosPorDia(string diaReparto);
        public delegate void DSubstractPedidoAReparto(Reparto reparto, Pedido pedido);

        public readonly static DAddPedidoARepartoSimple addPedidoARepartoSimple
            = new DAddPedidoARepartoSimple(AddPedidoARepartoSimple);
        public readonly static DAddReparto addReparto
            = new DAddReparto(AddReparto);
        public readonly static DCleanReparto cleanReparto
            = new DCleanReparto(CleanReparto);
        public readonly static DExportReparto exportReparto
            = new DExportReparto(ExportReparto);
        public readonly static DEditReparto editReparto
            = new DEditReparto(EditReparto);
        public readonly static DGetReparto getReparto
            = new DGetReparto(GetReparto);
        public readonly static DGetRepartoPorDiaYNombre getRepartoPorDiaYNombre
            = new DGetRepartoPorDiaYNombre(GetRepartoPorDiaYNombre);
        public readonly static DGetRepartos getRepartos
            = new DGetRepartos(GetRepartos);
        public readonly static DGetRepartosPorDia getRepartosPorDia
            = new DGetRepartosPorDia(GetRepartosPorDia);
        public readonly static DSubstractPedidoAReparto substractPedidoAReparto
            = new DSubstractPedidoAReparto(SubstractPedidoAReparto);


        private static void AddPedidoARepartoSimple(Reparto reparto, Pedido pedido)
        {
            reparto.Ta += pedido.A;
            reparto.Te += pedido.E;
            reparto.Tt += pedido.T;
            reparto.Tae += pedido.Ae;
            reparto.Td += pedido.D;
            reparto.TotalB += pedido.A + pedido.E + pedido.T + pedido.Ae + pedido.D;
            reparto.Tl += pedido.L;
            EditReparto(reparto);
        }
        private static void AddReparto(Reparto reparto)
        {
            bool response = Form1._servReparto.Add(reparto);
            if (!response) MessageBox.Show("Algo falló al agregar nuevo Reparto a la base de datos");
        }
        private static Reparto CleanReparto(Reparto reparto)
        {
            reparto.Ta = 0;
            reparto.Te = 0;
            reparto.Td = 0;
            reparto.Tt = 0;
            reparto.Tae = 0;
            reparto.TotalB = 0;
            reparto.Tl = 0;
            foreach (Pedido pedido in reparto.Pedidos)
            {
                cleanPedido(pedido);
            }
            editReparto(reparto);
            return getReparto(reparto.Id);
        }
        private static void EditReparto(Reparto reparto)
        {
            bool response = Form1._servReparto.Edit(reparto);
            if (!response) MessageBox.Show("Algo falló al editar el Reparto en la base de datos");
        }
        private static bool ExportReparto(string dia, string nombreReparto)
        {
            Reparto reparto = getRepartoPorDiaYNombre(dia, nombreReparto);
            var export = new Exportar();
            bool success = export.ExportarAExcel(reparto);
            if (success) MessageBox.Show("Terminado");
            else MessageBox.Show("Algo falló");
            return success;
        }
        private static Reparto GetReparto(long repartoId)
        {
            return Form1._servReparto.Get(repartoId);
        }
        private static Reparto GetRepartoPorDiaYNombre(string dia, string nombre)
        {
            try
            {
                return Form1._servDiaReparto.GetAll()
                    .Find(x => x.Dia == dia).Reparto.ToList()
                    .Find(x => x.Nombre == nombre);
            }
            catch { return null; }
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
        private static void SubstractPedidoAReparto(Reparto reparto, Pedido pedido)
        {
            reparto.Ta -= pedido.A;
            reparto.Te -= pedido.E;
            reparto.Tt -= pedido.T;
            reparto.Tae -= pedido.Ae;
            reparto.Td -= pedido.D;
            reparto.TotalB -= pedido.A + pedido.E + pedido.T + pedido.Ae + pedido.D;
            reparto.Tl -= pedido.L;
            EditReparto(reparto);
        }
    }
}
