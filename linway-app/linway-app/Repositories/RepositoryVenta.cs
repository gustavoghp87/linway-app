using linway_app.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace linway_app.Repositories
{
    public class RepositoryVenta : IRepository<Venta>
    {
        private readonly LinwaydbContext _context;
        public RepositoryVenta(LinwaydbContext context)
        {
            _context = context;
        }
        public bool Add(Venta venta)
        {
            string commandText = $"INSERT INTO Venta(ProductoId, Cantidad) " +
                                 $"VALUES ({venta.ProductoId}, {venta.Cantidad})";
            return SQLiteCommands.Execute(commandText);
        }
        public bool Delete(Venta venta)
        {
            try
            {
                _context.Venta.Remove(venta);
                _context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
        public bool Edit(Venta venta)
        {
            try
            {
                _context.Venta.Update(venta);
                _context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
        public Venta Get(long id)
        {
            return _context.Venta.Find(id);
        }
        public List<Venta> GetAll()
        {
            return _context.Venta.ToList();
        }
    }
}
