using AppServices.Interfaces;
using Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AppServices.EntityServices
{
    public class DiaRepartoServices: IDiaRepartoServices
    {
        private readonly IServicesBase<DiaReparto> _services;
        public DiaRepartoServices(IServicesBase<DiaReparto> services)
        {
            _services = services;
        }
        public async Task<List<DiaReparto>> GetAllAsync()
        {
            List<DiaReparto> diasReparto = await _services.GetAllAsync();
            return diasReparto;
        }
    }
}
