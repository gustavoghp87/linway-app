using linway_app.Models;
using linway_app.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace linway_app.Repositories
{
    public class RepositoryProducto : IRepository<Producto>
    {
        private readonly LinwaydbContext _context;
        private readonly DbSet<Producto> _entities;
        public RepositoryProducto(LinwaydbContext context)
        {
            _context = context;
            _entities = context.Set<Producto>();
        }
        public bool Add(Producto producto)
        {
            try
            {
                producto.Precio = Math.Truncate(producto.Precio * 100) / 100;
                _entities.Add(producto);
                _context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
        public bool Delete(Producto producto)
        {
            producto.Estado = "Eliminado";
            return Edit(producto);
        }
        public bool Edit(Producto producto)
        {
            try
            {
                producto.Precio = Math.Truncate(producto.Precio * 100) / 100;
                using (var context = new LinwaydbContext())
                {
                    context.Producto.Update(producto);
                    context.SaveChanges();
                }
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
        public Producto Get(long id)
        {
            var response = _entities.Find(id);
            if (response == null || response.Estado == null || response.Estado == "Eliminado") return null;
            response.Precio = Math.Truncate(response.Precio * 100) / 100;
            return response;
        }
        public List<Producto> GetAll()
        {
            using (var context = new LinwaydbContext())
            {
                var lstSinFiltr = context.Producto.ToList();
                var lst = lstSinFiltr.Where(x => x.Estado != null && x.Estado != "Eliminado").ToList();
                foreach (var producto in lst)
                {
                    producto.Precio = Math.Truncate(producto.Precio * 100) / 100;
                }
                return lst;
            }
        }
    }
}
