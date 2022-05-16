using InfrasServices.Services;
using linway_app.Excel;
using linway_app.Services.Interfaces;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using static linway_app.Services.Delegates.DDiaReparto;
using static linway_app.Services.Delegates.DPedido;
using static linway_app.Services.Delegates.DProdVendido;

namespace linway_app.Services.Delegates
{
    public static class DReparto
    {
        public readonly static Action<Reparto, Pedido> addPedidoARepartoSimple = AddPedidoARepartoSimple;
        public readonly static Action<Reparto> addReparto = AddReparto;
        public readonly static Action<ICollection<Reparto>> cleanRepartos = CleanRepartos;
        public readonly static Action<Reparto> editReparto = EditReparto;
        public readonly static Func<string, string, bool> exportReparto = ExportReparto;
        public readonly static Func<long, Reparto> getReparto = GetReparto;
        public readonly static Func<string, string, Reparto> getRepartoPorDiaYNombre = GetRepartoPorDiaYNombre;
        public readonly static Func<List<Reparto>> getRepartos = GetRepartos;
        public readonly static Func<string, List<Reparto>> getRepartosPorDia = GetRepartosPorDia;
        public readonly static Action<Reparto, Pedido> substractPedidoAReparto = SubstractPedidoAReparto;

        private static readonly IServiceBase<Reparto> _service = ServicesObjects.ServReparto;
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
            bool response = _service.Add(reparto);
            if (!response) Console.WriteLine("Algo falló al agregar nuevo Reparto a la base de datos");
        }
        private static void CleanRepartos(ICollection<Reparto> repartos)
        {
            if (repartos == null || repartos.Count == 0) return;
            var pedidosAEditar = new List<Pedido>();
            var prodVendidosAEditar = new List<ProdVendido>();
            foreach (Reparto reparto in repartos)
            {
                reparto.Ta = 0;
                reparto.Te = 0;
                reparto.Td = 0;
                reparto.Tt = 0;
                reparto.Tae = 0;
                reparto.TotalB = 0;
                reparto.Tl = 0;
                pedidosAEditar.AddRange(reparto.Pedidos);
                foreach (Pedido pedido in reparto.Pedidos)
                {
                    if (pedido.ProdVendidos == null || pedido.ProdVendidos.Count == 0) continue;
                    foreach (ProdVendido prodVendido in pedido.ProdVendidos)
                    {
                        prodVendido.PedidoId = null;
                        prodVendidosAEditar.Add(prodVendido);
                    }
                }
            }
            EditRepartos(repartos);
            cleanPedidos(pedidosAEditar);
            editProdVendidos(prodVendidosAEditar);
        }
        private static void EditReparto(Reparto reparto)
        {
            bool response = _service.Edit(reparto);
            if (!response) Console.WriteLine("Algo falló al editar el Reparto en la base de datos");
        }
        private static void EditRepartos(ICollection<Reparto> repartos)
        {
            bool response = _service.EditMany(repartos);
            if (!response) Console.WriteLine("Algo falló al editar los Repartos en la base de datos");
        }
        private static bool ExportReparto(string dia, string nombreReparto)
        {
            Reparto reparto = getRepartoPorDiaYNombre(dia, nombreReparto);
            Exportar servExportar = new Exportar();
            return servExportar.ExportarReparto(reparto);
        }
        private static Reparto GetReparto(long repartoId)
        {
            return _service.Get(repartoId);
        }
        private static Reparto GetRepartoPorDiaYNombre(string dia, string nombre)
        {
            try
            {
                return getDiaRepartos()
                    .Find(x => x.Dia == dia && x.Estado != null && x.Estado != "Eliminado").Reparto.ToList()
                    .Find(x => x.Nombre == nombre && x.Estado != null && x.Estado != "Eliminado");
            }
            catch { return null; }
        }
        private static List<Reparto> GetRepartos()
        {
            return _service.GetAll();
        }
        private static List<Reparto> GetRepartosPorDia(string diaReparto)
        {
            try
            {
                List<DiaReparto> lstDiasRep = getDiaRepartos();
                List<Reparto> lstRepartos = lstDiasRep.Find(x => x.Dia == diaReparto && x.Estado != null && x.Estado != "Eliminado").Reparto.ToList();
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
            editReparto(reparto);
        }
    }
}
