using linway_app.Services.Interfaces;
using Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace linway_app.Services.FormServices
{
    public class RegistroVentaServices: IRegistroVentaServices
    {
        private readonly IServicesBase<RegistroVenta> _services;
        public RegistroVentaServices(IServicesBase<RegistroVenta> services)
        {
            _services = services;
        }
        public void Add(RegistroVenta registroVenta)
        {
            _services.Add(registroVenta);
        }
        public void Delete(RegistroVenta registroVenta)
        {
            _services.Delete(registroVenta);
        }
        public void DeleteMany(ICollection<RegistroVenta> registros)
        {
            _services.DeleteMany(registros);
        }
        public async Task<RegistroVenta> GetPorIdAsync(long registroVentaId)
        {
            RegistroVenta registoVenta = await _services.GetAsync(registroVentaId);
            return registoVenta;
        }
        public async Task<List<RegistroVenta>> GetAllAsync()
        {
            List<RegistroVenta> registros = await _services.GetAllAsync();
            return registros;
        }
    }
}
