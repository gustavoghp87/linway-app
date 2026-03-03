using linway_app.Services.Interfaces;
using Models;
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
        public void AddReparto(Reparto reparto)
        {
            _services.Add(reparto);
        }
        public void EditReparto(Reparto reparto)
        {
            _services.Edit(reparto);
        }
        public void EditRepartos(ICollection<Reparto> repartos)
        {
            if (repartos == null || repartos.Count == 0) {
                return;
            }
            _services.EditMany(repartos);
        }
        public async Task<Reparto> GetRepartoPorIdAsync(long repartoId)
        {
            Reparto reparto = await _services.GetAsync(repartoId);
            return reparto;
        }
        public async Task<List<Reparto>> GetRepartosAsync()
        {
            List<Reparto> repartos = await _services.GetAllAsync();
            return repartos;
        }
        public void UpdateReparto(Reparto reparto)
        {
            if (reparto == null || reparto.Pedidos == null || reparto.Pedidos.Count == 0)
            {
                return;
            }
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
            EditReparto(reparto);
        }
    }
}
