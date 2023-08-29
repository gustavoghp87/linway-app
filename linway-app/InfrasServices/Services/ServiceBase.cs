using Infrastructure.Repositories.Interfaces;
using linway_app.Services.Interfaces;
using Models;
using Models.OModel;
using System.Collections.Generic;
using System.Linq;

namespace linway_app.Services
{
    public class ServiceBase<T> : IServiceBase<T> where T : ObjModel
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<T> _repository;
        public ServiceBase(IUnitOfWork unitOfWork, IRepository<T> repository)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
            if (typeof(T) == typeof(Cliente)) _repository = (IRepository<T>)_unitOfWork.RepoCliente;
            if (typeof(T) == typeof(DetalleRecibo)) _repository = (IRepository<T>)_unitOfWork.RepoDetalleRecibo;
            if (typeof(T) == typeof(DiaReparto)) _repository = (IRepository<T>)_unitOfWork.RepoDiaReparto;
            if (typeof(T) == typeof(NotaDeEnvio)) _repository = (IRepository<T>)_unitOfWork.RepoNotaDeEnvio;
            if (typeof(T) == typeof(Pedido)) _repository = (IRepository<T>)_unitOfWork.RepoPedido;
            if (typeof(T) == typeof(Producto)) _repository = (IRepository<T>)_unitOfWork.RepoProducto;
            if (typeof(T) == typeof(ProdVendido)) _repository = (IRepository<T>)_unitOfWork.RepoProdVendido;
            if (typeof(T) == typeof(Recibo)) _repository = (IRepository<T>)_unitOfWork.RepoRecibo;
            if (typeof(T) == typeof(Reparto)) _repository = (IRepository<T>)_unitOfWork.RepoReparto;
            if (typeof(T) == typeof(Venta)) _repository = (IRepository<T>)_unitOfWork.RepoVenta;
        }
        public bool Add(T t)
        {
            t.Estado = "Activo";
            bool response = _repository.Add(t);
            return response;
        }
        public bool AddMany(ICollection<T> t)
        {
            foreach (T item in t)
            {
                item.Estado = "Activo";
            }
            bool success = _repository.AddMany(t);
            return success;
        }
        public bool Delete(T t)
        {
            t.Estado = "Eliminado";
            bool success = Edit(t);
            return success;
        }
        public bool DeleteMany(ICollection<T> t)
        {
            foreach (T item in t)
            {
                item.Estado = "Eliminado";
            }
            return EditMany(t);
        }
        public bool Edit(T t)
        {
            bool success = _repository.Edit(t);
            return success;
        }
        public bool EditMany(ICollection<T> t)
        {
            bool success = _repository.EditMany(t);
            return success;
        }
        public T Get(long id)
        {
            T t = (T)_repository.Get(id);
            T response = t == null || t.Estado == null || t.Estado == "Eliminado" ? default : t;
            return response;
        }
        public List<T> GetAll()
        {
            List<T> lst = _repository.GetAll();
            if (lst == null || lst.Count == 0) return null;
            lst = (from x
                   in lst
                   where x.Estado != null && x.Estado != "Eliminado"
                   select x).ToList();
            if (lst == null || lst.Count == 0) return null;
            return lst;
        }
    }
}
