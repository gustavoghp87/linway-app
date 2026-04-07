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
        public void Delete(Reparto reparto)
        {
            _services.Delete(reparto);
        }
        public async Task<Reparto> GetPorIdAsync(long repartoId)
        {
            Reparto reparto = await _services.GetAsync(repartoId);
            return reparto;
        }
    }
}
