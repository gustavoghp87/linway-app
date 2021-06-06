using linway_app.Models;
using linway_app.Repositories.Interfaces;
using linway_app.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace linway_app.Services
{
    public class ServicioProducto : IServicioProducto
    {
        private readonly IUnitOfWork _unitOfWork;

        public ServicioProducto(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public bool Add(Producto producto)
        {
            producto.Precio = Math.Truncate(producto.Precio * 100) / 100;
            return _unitOfWork.RepoProducto.Add(producto);
        }
        public bool Delete(Producto producto)
        {
            producto.Estado = "Eliminado";
            return _unitOfWork.RepoProducto.Edit(producto);
        }
        public bool Edit(Producto producto)
        {
            producto.Precio = Math.Truncate(producto.Precio * 100) / 100;
            return _unitOfWork.RepoProducto.Edit(producto);
        }
        public Producto Get(long id)
        {
            Producto producto = _unitOfWork.RepoProducto.Get(id);
            try { producto.Precio = Math.Truncate(producto.Precio * 100) / 100; } catch { };
            return producto == null || producto.Estado == null || producto.Estado == "Eliminado"
                ? null : producto;
        }
        public List<Producto> GetAll()
        {
            List<Producto> lst = _unitOfWork.RepoProducto.GetAll();
            lst = (from x
                   in lst
                   where x.Estado != null && x.Estado != "Eliminado"
                   select x).ToList();
            foreach (var producto in lst)
            {
                producto.Precio = Math.Truncate(producto.Precio * 100) / 100;
            }
            return lst;
        }
    }
}
