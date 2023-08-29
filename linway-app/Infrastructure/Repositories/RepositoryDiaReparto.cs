using Infrastructure.Repositories.DbContexts;
using Infrastructure.Repositories.Interfaces;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.Repositories
{
    public class RepositoryDiaReparto : IRepository<DiaReparto>
    {
        private readonly LinwayDbContext _context;
        public RepositoryDiaReparto()
        {
            _context = new LinwayDbContext();
        }
        public bool Add(DiaReparto t)
        {
            try
            {
                _context.Set<DiaReparto>().Add(t);
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
        public bool AddMany(ICollection<DiaReparto> t)
        {
            try
            {
                _context.Set<DiaReparto>().AddRange(t);
                _context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
        public bool Edit(DiaReparto t)
        {
            using var context = new LinwayDbContext();
            try
            {
                _context.Set<DiaReparto>().Update(t);
                _context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
        public bool EditMany(ICollection<DiaReparto> t)
        {
            using var context = new LinwayDbContext();
            try
            {
                _context.Set<DiaReparto>().UpdateRange(t);
                _context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
        public DiaReparto Get(long id)
        {
            try
            {
                return _context.Set<DiaReparto>().Find(id);
            }
            catch
            {
                return default;
            }
        }
        public List<DiaReparto> GetAll()
        {
            try
            {
                List<DiaReparto> lista = _context.Set<DiaReparto>().ToList();
                return lista;
            }
            catch
            {
                return default;
            }
        }
    }
}
