using linway_app.Repositories.Interfaces;
using linway_app.Models.OModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace linway_app.Services
{
    abstract public class ServicioBase<T>
    {
        private readonly IUnitOfWork _unitOfWork;
        public ServicioBase(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        //public bool Add(T t)
        //{
        //    return _unitOfWork.RepoBase.Add(t);
        //}
        //public bool Delete(T t)
        //{
        //    t.Estado = "Eliminado";
        //    return _unitOfWork.RepoBase.Edit(t);
        //}
        //public bool Edit(T t)
        //{
        //    return _unitOfWork.RepoBase.Edit(t);
        //}
        //public T Get(long id)
        //{
        //    T t = (T)_unitOfWork.RepoBase.Get(id);
        //    return t == null || t.Estado == null || t.Estado == "Eliminado"
        //        ? null : t;
        //}
        //public List<T> GetAll()
        //{
        //    var lst = _unitOfWork.RepoBase.GetAll();
        //    if (lst == null || lst.Count == 0) return null;
        //    lst = (from x
        //           in lst
        //           where x.Estado != null && x.Estado != "Eliminado"
        //           select x).ToList();
        //    return lst;
        //}
    }
}
