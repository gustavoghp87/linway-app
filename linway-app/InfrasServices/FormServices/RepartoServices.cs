using linway_app.Services.Interfaces;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace linway_app.Services.FormServices
{
    public class RepartoServices: IRepartoServices
    {
        private readonly IServicesBase<Reparto> _services;
        public RepartoServices(IServicesBase<Reparto> services)
        {
            _services = services;
        }
        public void Add(Reparto reparto, List<DiaReparto> diaRepartos)
        {
            Reparto repartoMismoDiaNombre = diaRepartos.Find(d => d.Id == reparto.DiaRepartoId).Repartos.FirstOrDefault(r => r.Nombre == reparto.Nombre);
            if (repartoMismoDiaNombre != null)
            {
                throw new Exception($"Ya existe un reparto '{reparto.Nombre}' los {reparto.DiaReparto.Dia}");
            }
            _services.Add(reparto);
        }
        public void Edit(Reparto reparto)
        {
            _services.Edit(reparto);
        }
        public void EditMany(ICollection<Reparto> repartos)
        {
            _services.EditMany(repartos);
        }
        public async Task<Reparto> GetPorIdAsync(long repartoId)
        {
            Reparto reparto = await _services.GetAsync(repartoId);
            return reparto;
        }
        public async Task<List<Reparto>> GetAllAsync()
        {
            List<Reparto> repartos = await _services.GetAllAsync();
            return repartos;
        }
        #region static methods
        public static void ActualizarCantidadesDeReparto(Reparto reparto)  // primero pedido, luego reparto
        {
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
        }
        #endregion
    }
}
