using Infrastructure.Repositories.DbContexts;
using Infrastructure.Repositories.Interfaces;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.Repositories
{
    public class RepositoryRegistroVenta : IRepository<RegistroVenta>
    {
        private readonly LinwayDbContext _context;
        public RepositoryRegistroVenta()
        {
            _context = new LinwayDbContext();
        }
        public bool Add(RegistroVenta t)
        {
            try
            {
                _context.Set<RegistroVenta>().Add(t);
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
        public bool AddMany(ICollection<RegistroVenta> t)
        {
            try
            {
                _context.Set<RegistroVenta>().AddRange(t);
                _context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
        public bool Edit(RegistroVenta t)
        {
            using var context = new LinwayDbContext();
            try
            {
                _context.Set<RegistroVenta>().Update(t);
                _context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
        public bool EditMany(ICollection<RegistroVenta> t)
        {
            using var context = new LinwayDbContext();
            try
            {
                _context.Set<RegistroVenta>().UpdateRange(t);
                _context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
        public RegistroVenta Get(long id)
        {
            try
            {
                return _context.Set<RegistroVenta>().Find(id);
            }
            catch
            {
                return default;
            }
        }
        public List<RegistroVenta> GetAll()
        {
            try
            {
                List<RegistroVenta> lista = _context.Set<RegistroVenta>().ToList();
                return lista;
            }
            catch
            {
                return default;
            }
        }
    }
}
