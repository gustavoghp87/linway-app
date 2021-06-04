using linway_app.Models;
using linway_app.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace linway_app.Repositories
{
    public class RepositoryVenta : IRepository<Venta>
    {
        private readonly LinwaydbContext _context;
        private readonly DbSet<Venta> _entities;
        public RepositoryVenta(LinwaydbContext context)
        {
            _context = context;
            _entities = context.Set<Venta>();
        }
        public bool Add(Venta venta)
        {
            try
            {
                _entities.Add(venta);
                _context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
        public bool Delete(Venta venta)
        {
            venta.Estado = "Eliminado";
            return Edit(venta);
        }
        public bool Edit(Venta venta)
        {
            try
            {
                _entities.Update(venta);
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
            var response = _entities.Find(id);
            if (response == null || response.Estado == null || response.Estado == "Eliminado") return null;
            return response;
        }
        public List<Venta> GetAll()
        {
            var lstSinFiltr = _entities.ToList();
            var lst = lstSinFiltr.Where(x => x.Estado != null && x.Estado != "Eliminado").ToList();
            return lst;
        }
    }
}
