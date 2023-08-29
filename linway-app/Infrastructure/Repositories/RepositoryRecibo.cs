using Infrastructure.Repositories.DbContexts;
using Infrastructure.Repositories.Interfaces;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.Repositories
{
    public class RepositoryRecibo : IRepository<Recibo>
    {
        private readonly LinwayDbContext _context;
        public RepositoryRecibo()
        {
            _context = new LinwayDbContext();
        }
        public bool Add(Recibo t)
        {
            try
            {
                _context.Set<Recibo>().Add(t);
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
        public bool AddMany(ICollection<Recibo> t)
        {
            try
            {
                _context.Set<Recibo>().AddRange(t);
                _context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
        public bool Edit(Recibo t)
        {
            using var context = new LinwayDbContext();
            try
            {
                _context.Set<Recibo>().Update(t);
                _context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
        public bool EditMany(ICollection<Recibo> t)
        {
            using var context = new LinwayDbContext();
            try
            {
                _context.Set<Recibo>().UpdateRange(t);
                _context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
        public Recibo Get(long id)
        {
            try
            {
                return _context.Set<Recibo>().Find(id);
            }
            catch
            {
                return default;
            }
        }
        public List<Recibo> GetAll()
        {
            try
            {
                List<Recibo> lista = _context.Set<Recibo>().ToList();
                return lista;
            }
            catch
            {
                return default;
            }
        }
    }
}
