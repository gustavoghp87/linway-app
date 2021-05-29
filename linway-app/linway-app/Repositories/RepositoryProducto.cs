using linway_app.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace linway_app.Repositories
{
    public class RepositoryProducto : IRepository<Producto>
    {
        private readonly LinwaydbContext _context;
        public RepositoryProducto(LinwaydbContext context)
        {
            _context = context;
        }
        public bool Add(Producto producto)
        {
            double precio = Math.Truncate(producto.Precio * 100) / 100;
            string commandText = $"INSERT INTO Producto(Nombre, Precio) " +
                                 $"VALUES ('{producto.Nombre}', '{precio}')";
            return SQLiteCommands.Execute(commandText);
        }
        public bool Edit(Producto producto)
        {
            double precio = Math.Truncate(producto.Precio * 100) / 100;
            string commandText = $"UPDATE Producto " +
                                 $"SET Nombre='{producto.Nombre}', Precio='{precio}' " +
                                 $"WHERE Id={producto.Id}";
            return SQLiteCommands.Execute(commandText);
        }
        public bool Delete(Producto producto)
        {
            try
            {
                _context.Producto.Remove(producto);
                _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public Producto Get(long id)
        {
            var producto = _context.Producto.Find(id);
            if (producto == null) return null;
            producto.Precio = Math.Truncate(producto.Precio * 100) / 100;
            return producto;
        }
        public List<Producto> GetAll()
        {
            var lstProductos = _context.Producto.ToList();
            foreach (var producto in lstProductos)
            {
                producto.Precio = Math.Truncate(producto.Precio * 100) / 100;
            }
            return lstProductos;
        }
    }
}
