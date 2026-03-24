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
        public void AddRegistroVenta(RegistroVenta registroVenta)
        {
            _services.Add(registroVenta);
        }
        public void DeleteRegistroVenta(RegistroVenta registroVenta)
        {
            registroVenta.Estado = "Eliminado";
            _services.Edit(registroVenta);
            //_services.Delete(registroVenta);
        }
        public void DeleteRegistros(ICollection<RegistroVenta> registros)
        {
            foreach (var entity in registros)
            {
                entity.Estado = "Eliminado";
            }
            _services.EditMany(registros);
            //_services.DeleteMany(registros);
        }
        public void EditRegistroVenta(RegistroVenta registroVenta)
        {
            _services.Edit(registroVenta);
        }
        public async Task<RegistroVenta> GetRegistroVentaPorIdAsync(long registroVentaId)
        {
            RegistroVenta registoVenta = await _services.GetAsync(registroVentaId);
            return registoVenta;
        }
        public async Task<List<RegistroVenta>> GetRegistroVentasAsync()
        {
            List<RegistroVenta> registros = await _services.GetAllAsync();
            return registros;
        }
    }
}
