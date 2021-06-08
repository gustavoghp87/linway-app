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
        public bool Add(ProdVendido prodVendido)
        {
            prodVendido.Estado = "Activo";
            return _unitOfWork.RepoProdVendido.Add(prodVendido);
        }
        public bool Delete(ProdVendido prodVendido)
        {
            prodVendido.Estado = "Eliminado";
            return _unitOfWork.RepoProdVendido.Edit(prodVendido);
        }
        public bool Edit(ProdVendido prodVendido)
        {
            return _unitOfWork.RepoProdVendido.Edit(prodVendido);
        }
        public ProdVendido Get(long id)
        {
            ProdVendido prodVendido = _unitOfWork.RepoProdVendido.Get(id);
            return prodVendido == null || prodVendido.Estado == null || prodVendido.Estado == "Eliminado"
                ? null : prodVendido;
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
