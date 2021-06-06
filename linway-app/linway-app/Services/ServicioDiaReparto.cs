using linway_app.Models;
using linway_app.Repositories.Interfaces;
using linway_app.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace linway_app.Services
{
    public class ServicioDiaReparto : IServicioDiaReparto
    {
        private readonly IUnitOfWork _unitOfWork;
        public ServicioDiaReparto(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public bool Add(DiaReparto diaReparto)
        {
            return _unitOfWork.RepoDiaReparto.Add(diaReparto);
        }
        public bool Delete(DiaReparto diaReparto)
        {
            diaReparto.Estado = "Eliminado";
            return _unitOfWork.RepoDiaReparto.Edit(diaReparto);
        }
        public bool Edit(DiaReparto diaReparto)
        {
            return _unitOfWork.RepoDiaReparto.Edit(diaReparto);
        }
        public DiaReparto Get(long id)
        {
            DiaReparto diaReparto = _unitOfWork.RepoDiaReparto.Get(id);
            return diaReparto == null || diaReparto.Estado == null || diaReparto.Estado == "Eliminado"
                ? null : diaReparto;
        }
        public List<DiaReparto> GetAll()
        {
            List<DiaReparto> lst = _unitOfWork.RepoDiaReparto.GetAll();
            lst = (from x
                   in lst
                   where x.Estado != null && x.Estado != "Eliminado"
                   select x).ToList();
            return lst;
        }
    }
}
