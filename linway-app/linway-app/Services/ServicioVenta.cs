using linway_app.Models;
using linway_app.Repositories.Interfaces;
using linway_app.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;

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
            venta.Estado = "Activo";
            return _unitOfWork.RepoVenta.Add(venta);
        }
        public bool Delete(Venta venta)
        {
            venta.Estado = "Eliminado";
            return _unitOfWork.RepoVenta.Edit(venta);
        }
        public bool Edit(Venta venta)
        {
            return _unitOfWork.RepoVenta.Edit(venta);
        }
        public Venta Get(long id)
        {
            Venta venta = _unitOfWork.RepoVenta.Get(id);
            return venta == null || venta.Estado == null || venta.Estado == "Eliminado" ? null : venta;
        }
        public List<Venta> GetAll()
        {
            List<Venta> lst = _unitOfWork.RepoVenta.GetAll();
            lst = (from x
                   in lst
                   where x.Estado != null && x.Estado != "Eliminado"
                   select x).ToList();
            return lst;
        }
    }
}
