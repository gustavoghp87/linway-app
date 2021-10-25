using linway_app.Models;
using linway_app.Models.OModel;
using linway_app.Repositories.Interfaces;
using linway_app.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace linway_app.Services
{
    public class ServiceBase<T> : IServiceBase<T> where T : ObjModel
    {
        private readonly IUnitOfWork _unitOfWork;
        private IRepository<T> _service;
        public ServiceBase(IUnitOfWork unitOfWork, IRepository<T> service)
        {
            _unitOfWork = unitOfWork;
            _service = service;
            if (typeof(T) == typeof(Cliente)) _service = (IRepository<T>)_unitOfWork.RepoCliente;
            if (typeof(T) == typeof(DetalleRecibo)) _service = (IRepository<T>)_unitOfWork.RepoDetalleRecibo;
            if (typeof(T) == typeof(DiaReparto)) _service = (IRepository<T>)_unitOfWork.RepoDiaReparto;
            if (typeof(T) == typeof(NotaDeEnvio)) _service = (IRepository<T>)_unitOfWork.RepoNotaDeEnvio;
            if (typeof(T) == typeof(Pedido)) _service = (IRepository<T>)_unitOfWork.RepoPedido;
            if (typeof(T) == typeof(Producto)) _service = (IRepository<T>)_unitOfWork.RepoProducto;
            if (typeof(T) == typeof(ProdVendido)) _service = (IRepository<T>)_unitOfWork.RepoProdVendido;
            if (typeof(T) == typeof(Recibo)) _service = (IRepository<T>)_unitOfWork.RepoRecibo;
            if (typeof(T) == typeof(Reparto)) _service = (IRepository<T>)_unitOfWork.RepoReparto;
            if (typeof(T) == typeof(Venta)) _service = (IRepository<T>)_unitOfWork.RepoVenta;
        }
        public bool Add(T t)
        {
            t.Estado = "Activo";
            return _service.Add(t);
        }
        public bool Delete(T t)
        {
            t.Estado = "Eliminado";
            return _service.Edit(t);
        }
        public bool Edit(T t)
        {
            return _service.Edit(t);
        }
        public T Get(long id)
        {
            T t = (T)_service.Get(id);
            return t == null || t.Estado == null || t.Estado == "Eliminado" ? default : t;
        }
        public List<T> GetAll()
        {
            List<T> lst = _service.GetAll();
            if (lst == null || lst.Count == 0) return null;
            lst = (from x
                   in lst
                   where x.Estado != null && x.Estado != "Eliminado"
                   select x).ToList();
            return lst;
        }
    }
}
