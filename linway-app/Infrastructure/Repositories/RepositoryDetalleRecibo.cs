using Infrastructure.Repositories.DbContexts;
using Infrastructure.Repositories.Interfaces;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.Repositories
{
    public class RepositoryDetalleRecibo : IRepository<DetalleRecibo>
    {
        private readonly LinwayDbContext _context;
        public RepositoryDetalleRecibo()
        {
            _context = new LinwayDbContext();
        }
        public bool Add(DetalleRecibo t)
        {
            try
            {
                _context.Set<DetalleRecibo>().Add(t);
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
        public bool AddMany(ICollection<DetalleRecibo> t)
        {
            try
            {
                _context.Set<DetalleRecibo>().AddRange(t);
                _context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
        public bool Edit(DetalleRecibo t)
        {
            using var context = new LinwayDbContext();
            try
            {
                _context.Set<DetalleRecibo>().Update(t);
                _context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
        public bool EditMany(ICollection<DetalleRecibo> t)
        {
            using var context = new LinwayDbContext();
            try
            {
                _context.Set<DetalleRecibo>().UpdateRange(t);
                _context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
        public DetalleRecibo Get(long id)
        {
            try
            {
                return _context.Set<DetalleRecibo>().Find(id);
            }
            catch
            {
                return default;
            }
        }
        public List<DetalleRecibo> GetAll()
        {
            try
            {
                List<DetalleRecibo> lista = _context.Set<DetalleRecibo>().ToList();
                return lista;
            }
            catch
            {
                return default;
            }
        }
    }
}
