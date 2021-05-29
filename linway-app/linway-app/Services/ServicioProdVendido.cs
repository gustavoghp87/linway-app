using linway_app.Models;
using linway_app.Repositories.Interfaces;
using linway_app.Services.Interfaces;
using System.Collections.Generic;

namespace linway_app.Services
{
    public class ServicioProdVendido : IServicioProdVendido
    {
        private readonly IUnitOfWork _unitOfWork;

        public ServicioProdVendido(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public List<ProdVendido> GetAll()
        {
            return _unitOfWork.RepoProdVendido.GetAll();
        }
        public ProdVendido Get(long id)
        {
            return _unitOfWork.RepoProdVendido.Get(id);
        }
        public bool Add(ProdVendido producto)
        {
            return _unitOfWork.RepoProdVendido.Add(producto);
        }
        public bool Delete(ProdVendido producto)
        {
            return _unitOfWork.RepoProdVendido.Delete(producto);
        }
        public bool Edit(ProdVendido producto)
        {
            return _unitOfWork.RepoProdVendido.Edit(producto);
        }
    }
}
