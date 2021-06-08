using linway_app.Models;
using linway_app.Repositories.Interfaces;
using linway_app.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace linway_app.Services
{
    public class ServicioDetalleRecibo : IServicioDetalleRecibo
    {
        private readonly IUnitOfWork _unitOfWork;
        public ServicioDetalleRecibo(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public bool Add(DetalleRecibo detalleRecibo)
        {
            detalleRecibo.Estado = "Activo";
            return _unitOfWork.RepoDetalleRecibo.Add(detalleRecibo);
        }
        public bool Delete(DetalleRecibo detalleRecibo)
        {
            detalleRecibo.Estado = "Eliminado";
            return _unitOfWork.RepoDetalleRecibo.Edit(detalleRecibo);
        }
        public bool Edit(DetalleRecibo detalleRecibo)
        {
            return _unitOfWork.RepoDetalleRecibo.Edit(detalleRecibo);
        }
        public DetalleRecibo Get(long id)
        {
            DetalleRecibo detalleRecibo = _unitOfWork.RepoDetalleRecibo.Get(id);
            return detalleRecibo == null || detalleRecibo.Estado == null || detalleRecibo.Estado == "Eliminado"
                ? null : detalleRecibo;
        }
        public List<DetalleRecibo> GetAll()
        {
            List<DetalleRecibo> lst = _unitOfWork.RepoDetalleRecibo.GetAll();
            lst = (from x
                   in lst
                   where x.Estado != null && x.Estado != "Eliminado"
                   select x).ToList();
            return lst;
        }
    }
}
