using linway_app.Services.Interfaces;
using Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace linway_app.Services.FormServices
{
    public class ReciboServices: IReciboServices
    {
        private readonly IServicesBase<Recibo> _services;
        public ReciboServices(IServicesBase<Recibo> services)
        {
            _services = services;
        }
        public void AddRecibo(Recibo recibo)
        {
            _services.Add(recibo);
        }
        //public decimal CalculateTotalRecibo(Recibo recibo)
        //{
        //    decimal total = 0;
        //    if (recibo.DetalleRecibos == null || recibo.DetalleRecibos.Count == 0) return total;
        //    foreach (DetalleRecibo detalle in recibo.DetalleRecibos)
        //    {
        //        total += detalle.Importe;
        //    }
        //    return total;
        //}
        public void DeleteRecibos(ICollection<Recibo> recibos)
        {
            _services.DeleteMany(recibos);
        }
        public void EditRecibo(Recibo recibo)
        {
            _services.Edit(recibo);
        }
        public async Task<List<Recibo>> GetRecibosAsync()
        {
            List<Recibo> recibos = await _services.GetAllAsync();
            return recibos;
        }
        //public async Task<List<Recibo>> GetRecibosConDetalles()  // prueba de concepto invalidando Lazy Loading
        //{  ... private readonly IRepository<Recibo> _repository;
        //    return await _repository
        //        .Query()
        //        .Where(x => x.Estado != "Eliminado")
        //        .Include(r => r.DetalleRecibos)
        //        .AsNoTracking()
        //        .ToListAsync();
        //}
    }
}
