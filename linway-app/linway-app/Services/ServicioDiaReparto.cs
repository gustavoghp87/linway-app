using linway_app.Models;
using linway_app.Repositories.Interfaces;
using linway_app.Services.Interfaces;
using System.Collections.Generic;

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
            return _unitOfWork.RepoDiaReparto.Delete(diaReparto);
        }
        public bool Edit(DiaReparto diaReparto)
        {
            return _unitOfWork.RepoDiaReparto.Edit(diaReparto);
        }
        public DiaReparto Get(long id)
        {
            return _unitOfWork.RepoDiaReparto.Get(id);
        }
        public List<DiaReparto> GetAll()
        {
            return _unitOfWork.RepoDiaReparto.GetAll();
        }
    }
}
