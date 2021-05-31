using linway_app.Models;
using linway_app.Repositories.Interfaces;
using linway_app.Services.Interfaces;
using System.Collections.Generic;

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
            return _unitOfWork.RepoDetalleRecibo.Add(detalleRecibo);
        }
        public bool Delete(DetalleRecibo detalleRecibo)
        {
            return _unitOfWork.RepoDetalleRecibo.Delete(detalleRecibo);
        }
        public bool Edit(DetalleRecibo detalleRecibo)
        {
            return _unitOfWork.RepoDetalleRecibo.Edit(detalleRecibo);
        }
        public DetalleRecibo Get(long id)
        {
            return _unitOfWork.RepoDetalleRecibo.Get(id);
        }
        public List<DetalleRecibo> GetAll()
        {
            return _unitOfWork.RepoDetalleRecibo.GetAll();
        }
    }
}
