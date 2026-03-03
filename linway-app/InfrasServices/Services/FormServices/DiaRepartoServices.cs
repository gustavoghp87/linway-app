using linway_app.Services.Interfaces;
using Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace linway_app.Services.FormServices
{
    public class DiaRepartoServices: IDiaRepartoServices
    {
        private readonly IServicesBase<DiaReparto> _services;
        public DiaRepartoServices(IServicesBase<DiaReparto> services)
        {
            _services = services;
        }
        public void AddDiaReparto(DiaReparto diaReparto)
        {
            _services.Add(diaReparto);
        }
        public async Task<List<DiaReparto>> GetDiaRepartos()
        {
            List<DiaReparto> diasReparto = await _services.GetAllAsync();
            return diasReparto;
        }
    }
}
