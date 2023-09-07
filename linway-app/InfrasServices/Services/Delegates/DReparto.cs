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
        public readonly static Predicate<Reparto> addReparto = AddReparto;
        public readonly static Predicate<ICollection<Reparto>> cleanRepartos = CleanRepartos;
        public readonly static Func<string, string, bool> exportReparto = ExportReparto;
        public readonly static Func<long, Reparto> getReparto = GetReparto;
        public readonly static Func<string, string, Reparto> getRepartoPorDiaYNombre = GetRepartoPorDiaYNombre;
        public readonly static Func<List<Reparto>> getRepartos = GetRepartos;
        public readonly static Func<string, List<Reparto>> getRepartosPorDia = GetRepartosPorDia;
        public readonly static Predicate<Reparto> updateReparto = UpdateReparto;

        private static readonly IServiceBase<Reparto> _service = ServicesObjects.ServReparto;
        
        private static bool AddReparto(Reparto reparto)
        {
            bool success = _service.Add(reparto);
            return success;
        }
        private static bool CleanRepartos(ICollection<Reparto> repartos)
        {
            if (repartos == null || repartos.Count == 0) return false;
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
            bool success = EditRepartos(repartos);
            bool successClean = cleanPedidos(pedidosAEditar);
            bool successEditPV = editProdVendidos(prodVendidosAEditar);
            return success && successClean && successEditPV;
        }
        private static bool EditReparto(Reparto reparto)
        {
            bool success = _service.Edit(reparto);
            return success;
        }
        private static bool EditRepartos(ICollection<Reparto> repartos)
        {
            if (repartos == null || repartos.Count == 0) return false;
            bool success = _service.EditMany(repartos);
            return success;
        }
        private static bool ExportReparto(string dia, string nombreReparto)
        {
            Reparto reparto = getRepartoPorDiaYNombre(dia, nombreReparto);
            bool success = new Exportar().ExportarReparto(reparto);
            return success;
        }
        private static Reparto GetReparto(long repartoId)
        {
            Reparto reparto = _service.Get(repartoId);
            return reparto;
        }
        private static Reparto GetRepartoPorDiaYNombre(string dia, string nombre)
        {
            try
            {
                List<DiaReparto> lstDiasRep = getDiaRepartos();
                if (lstDiasRep == null) return null;
                Reparto reparto = lstDiasRep
                    .Find(x => x.Dia == dia && x.Estado != null && x.Estado != "Eliminado").Reparto.ToList()
                    .Find(x => x.Nombre == nombre && x.Estado != null && x.Estado != "Eliminado");
                return reparto;
            }
            catch (Exception e)
            {
                Logger.LogException(e);
                return null;
            }
        }
        private static List<Reparto> GetRepartos()
        {
            List<Reparto> repartos = _service.GetAll();
            return repartos;
        }
        private static List<Reparto> GetRepartosPorDia(string diaReparto)
        {
            try
            {
                List<DiaReparto> lstDiasRep = getDiaRepartos();
                List<Reparto> lstRepartos = lstDiasRep
                    .Find(x => x.Dia == diaReparto && x.Estado != null && x.Estado != "Eliminado").Reparto.ToList();
                return lstRepartos;
            }
            catch (Exception e)
            {
                Logger.LogException(e);
                return null;
            }
        }
        private static bool UpdateReparto(Reparto reparto)
        {
            if (reparto == null || reparto.Pedidos == null || reparto.Pedidos.Count == 0) return false;
            reparto.Ta = 0;
            reparto.Te = 0;
            reparto.Tt = 0;
            reparto.Tae = 0;
            reparto.Td = 0;
            reparto.TotalB = 0;
            reparto.Tl = 0;
            reparto.Pedidos.ToList().ForEach(pedido =>
            {
                reparto.Ta += pedido.A;
                reparto.Te += pedido.E;
                reparto.Tt += pedido.T;
                reparto.Tae += pedido.Ae;
                reparto.Td += pedido.D;
                reparto.TotalB += pedido.A + pedido.E + pedido.T + pedido.Ae + pedido.D;
                reparto.Tl += pedido.L;
            });
            bool success = EditReparto(reparto);
            return success;
        }
    }
}
