using linway_app.Models;
using linway_app.Repositories.Interfaces;
using linway_app.Services.Interfaces;
using System.Collections.Generic;

namespace linway_app.Services
{
    public class ServicioVenta : IServicioVenta
    {
        private readonly IUnitOfWork _unitOfWork;
        public ServicioVenta(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public bool Add(Venta venta)
        {
            return _unitOfWork.RepoVenta.Add(venta);
        }
        public bool Delete(Venta venta)
        {
            return _unitOfWork.RepoVenta.Delete(venta);
        }
        public bool Edit(Venta venta)
        {
            return _unitOfWork.RepoVenta.Edit(venta);
        }
        public Venta Get(long id)
        {
            return _unitOfWork.RepoVenta.Get(id);
        }
        public List<Venta> GetAll()
        {
            return _unitOfWork.RepoVenta.GetAll();
        }
    }
}
