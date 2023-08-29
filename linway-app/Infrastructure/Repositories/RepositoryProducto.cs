using Infrastructure.Repositories.DbContexts;
using Infrastructure.Repositories.Interfaces;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.Repositories
{
    public class RepositoryProducto : IRepository<Producto>
    {
        private readonly LinwayDbContext _context;
        public RepositoryProducto()
        {
            _context = new LinwayDbContext();
        }
        public bool Add(Producto t)
        {
            try
            {
                _context.Set<Producto>().Add(t);
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
        public bool AddMany(ICollection<Producto> t)
        {
            try
            {
                _context.Set<Producto>().AddRange(t);
                _context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
        public bool Edit(Producto t)
        {
            using var context = new LinwayDbContext();
            try
            {
                _context.Set<Producto>().Update(t);
                _context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
        public bool EditMany(ICollection<Producto> t)
        {
            using var context = new LinwayDbContext();
            try
            {
                _context.Set<Producto>().UpdateRange(t);
                _context.SaveChangesAsync();
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
            try
            {
                return _context.Set<Producto>().Find(id);
            }
            catch
            {
                return default;
            }
        }
        public List<Producto> GetAll()
        {
            try
            {
                List<Producto> lista = _context.Set<Producto>().ToList();
                return lista;
            }
            catch
            {
                return default;
            }
        }
    }
}
