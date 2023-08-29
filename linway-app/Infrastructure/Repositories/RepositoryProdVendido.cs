using Infrastructure.Repositories.DbContexts;
using Infrastructure.Repositories.Interfaces;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.Repositories
{
    public class RepositoryProdVendido : IRepository<ProdVendido>
    {
        private readonly LinwayDbContext _context;
        public RepositoryProdVendido()
        {
            _context = new LinwayDbContext();
        }
        public bool Add(ProdVendido t)
        {
            try
            {
                _context.Set<ProdVendido>().Add(t);
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
        public bool AddMany(ICollection<ProdVendido> t)
        {
            try
            {
                _context.Set<ProdVendido>().AddRange(t);
                _context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
        public bool Edit(ProdVendido t)
        {
            using var context = new LinwayDbContext();
            try
            {
                _context.Set<ProdVendido>().Update(t);
                _context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
        public bool EditMany(ICollection<ProdVendido> t)
        {
            using var context = new LinwayDbContext();
            try
            {
                _context.Set<ProdVendido>().UpdateRange(t);
                _context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
        public ProdVendido Get(long id)
        {
            try
            {
                return _context.Set<ProdVendido>().Find(id);
            }
            catch
            {
                return default;
            }
        }
        public List<ProdVendido> GetAll()
        {
            try
            {
                List<ProdVendido> lista = _context.Set<ProdVendido>().ToList();
                return lista;
            }
            catch
            {
                return default;
            }
        }
    }
}
