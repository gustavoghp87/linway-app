using linway_app.Models;
using linway_app.Repositories.Interfaces;
using linway_app.Services.Interfaces;
using System.Collections.Generic;

namespace linway_app.Services
{
    public class ServicioProducto : IServicioProducto
    {
        private readonly IUnitOfWork _unitOfWork;

        public ServicioProducto(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public List<Producto> GetAll()
        {
            return _unitOfWork.RepoProducto.GetAll();
        }
        public Producto Get(long id)
        {
            return _unitOfWork.RepoProducto.Get(id);
        }
        public bool Add(Producto producto)
        {
            return _unitOfWork.RepoProducto.Add(producto);
        }
        public bool Delete(Producto producto)
        {
            return _unitOfWork.RepoProducto.Delete(producto);
        }
        public bool Edit(Producto producto)
        {
            return _unitOfWork.RepoProducto.Edit(producto);
        }
    }
}
