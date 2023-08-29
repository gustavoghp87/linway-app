using Infrastructure.Repositories.DbContexts;
using Infrastructure.Repositories.Interfaces;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.Repositories
{
    public class RepositoryReparto : IRepository<Reparto>
    {
        private readonly LinwayDbContext _context;
        public RepositoryReparto()
        {
            _context = new LinwayDbContext();
        }
        public bool Add(Reparto t)
        {
            using var context = new LinwayDbContext();
            try
            {
                context.Set<Reparto>().Add(t);
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
        public bool AddMany(ICollection<Reparto> t)
        {
            //using var context = new LinwayDbContext();
            try
            {
                _context.Set<Reparto>().AddRange(t);
                _context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
        public bool Edit(Reparto t)
        {
            //using var context = new LinwayDbContext();
            try
            {
                _context.Set<Reparto>().Update(t);
                _context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
        public bool EditMany(ICollection<Reparto> t)
        {
            //using var context = new LinwayDbContext();
            try
            {
                _context.Set<Reparto>().UpdateRange(t);
                _context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
        public Reparto Get(long id)
        {
            using var context = new LinwayDbContext();
            try
            {
                Reparto reparto = _context.Set<Reparto>().Find(id);
                return reparto;
            }
            catch
            {
                return default;
            }
        }
        public List<Reparto> GetAll()
        {
            //using var context = new LinwayDbContext();
            try
            {
                List<Reparto> lista = _context.Set<Reparto>().ToList();
                return lista;
            }
            catch
            {
                return default;
            }
        }
    }
}
