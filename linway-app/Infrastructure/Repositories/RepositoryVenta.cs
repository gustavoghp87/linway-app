using Infrastructure.Repositories.DbContexts;
using Infrastructure.Repositories.Interfaces;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.Repositories
{
    public class RepositoryVenta : IRepository<Venta>
    {
        private readonly LinwayDbContext _context;
        public RepositoryVenta()
        {
            _context = new LinwayDbContext();
        }
        public bool Add(Venta t)
        {
            try
            {
                _context.Set<Venta>().Add(t);
                int id = _context.SaveChanges();
                if (id != 1) return false;
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
        public bool AddMany(ICollection<Venta> t)
        {
            try
            {
                _context.Set<Venta>().AddRange(t);
                _context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
        public bool Edit(Venta t)
        {
            using var context = new LinwayDbContext();
            try
            {
                _context.Set<Venta>().Update(t);
                _context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
        public bool EditMany(ICollection<Venta> t)
        {
            using var context = new LinwayDbContext();
            try
            {
                _context.Set<Venta>().UpdateRange(t);
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
            try
            {
                return _context.Set<Venta>().Find(id);
            }
            catch
            {
                return default;
            }
        }
        public List<Venta> GetAll()
        {
            try
            {
                List<Venta> lista = _context.Set<Venta>().ToList();
                return lista;
            }
            catch
            {
                return default;
            }
        }
    }
}
