using linway_app.Models;
using linway_app.Repositories.Interfaces;
using linway_app.Services.Interfaces;
using System.Collections.Generic;

namespace linway_app.Services
{
    class ServicioRecibo : IServicioRecibo
    {
        private readonly IUnitOfWork _unitOfWork;

        public ServicioRecibo(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public bool Add(Recibo recibo)
        {
            return _unitOfWork.RepoRecibo.Add(recibo);
        }
        public bool Delete(Recibo recibo)
        {
            return _unitOfWork.RepoRecibo.Delete(recibo);
        }
        public bool Edit(Recibo recibo)
        {
            return _unitOfWork.RepoRecibo.Edit(recibo);
        }
        public Recibo Get(long id)
        {
            return _unitOfWork.RepoRecibo.Get(id);
        }
        public List<Recibo> GetAll()
        {
            return _unitOfWork.RepoRecibo.GetAll();
        }

        public void CalcularImporteTotal(Recibo recibo)
        {
            double subTo = 0;
            if (recibo.DetalleRecibos != null && recibo.DetalleRecibos.Count != 0)
                foreach (IDetalleRecibo detalle in recibo.DetalleRecibos)
                {
                    subTo += detalle.Importe;
                }
            recibo.ImporteTotal = subTo;
        }
    }
}
