using linway_app.Models;
using linway_app.Repositories.Interfaces;
using linway_app.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace linway_app.Services
{
    public class ServicioProdVendido : IServicioProdVendido
    {
        private readonly IUnitOfWork _unitOfWork;

        public ServicioProdVendido(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public bool Add(ProdVendido producto)
        {
            return _unitOfWork.RepoProdVendido.Add(producto);
        }
        public bool Delete(ProdVendido producto)
        {
            producto.Estado = "Eliminado";
            return _unitOfWork.RepoProdVendido.Edit(producto);
        }
        public bool Edit(ProdVendido producto)
        {
            return _unitOfWork.RepoProdVendido.Edit(producto);
        }
        public ProdVendido Get(long id)
        {
            ProdVendido producto = _unitOfWork.RepoProdVendido.Get(id);
            return producto == null || producto.Estado == null || producto.Estado == "Eliminado"
                ? null : producto;
        }
        public List<ProdVendido> GetAll()
        {
            List<ProdVendido> lst = _unitOfWork.RepoProdVendido.GetAll();
            lst = (from x
                   in lst
                   where x.Estado != null && x.Estado != "Eliminado"
                   select x).ToList();
            return lst;
        }
    }
}
