using linway_app.Models;
using linway_app.Repositories.Interfaces;
using linway_app.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;

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
            recibo.Estado = "Activo";
            return _unitOfWork.RepoRecibo.Add(recibo);
        }
        public bool Delete(Recibo recibo)
        {
            recibo.Estado = "Eliminado";
            return _unitOfWork.RepoRecibo.Edit(recibo);
        }
        public bool Edit(Recibo recibo)
        {
            return _unitOfWork.RepoRecibo.Edit(recibo);
        }
        public Recibo Get(long id)
        {
            Recibo recibo = _unitOfWork.RepoRecibo.Get(id);
            return recibo == null || recibo.Estado == null || recibo.Estado == "Eliminado" ? null : recibo;
        }
        public List<Recibo> GetAll()
        {
            List<Recibo> lst = _unitOfWork.RepoRecibo.GetAll();
            lst = (from x
                   in lst
                   where x.Estado != null && x.Estado != "Eliminado"
                   select x).ToList();
            return lst;
        }

        public double CalcularImporteTotal(Recibo recibo)
        {
            double subTo = 0;
            if (recibo.DetalleRecibos != null && recibo.DetalleRecibos.Count != 0)
                foreach (DetalleRecibo detalle in recibo.DetalleRecibos)
                {
                    subTo += detalle.Importe;
                }
            return subTo;
        }
    }
}
