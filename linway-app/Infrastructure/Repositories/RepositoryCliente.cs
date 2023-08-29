using Infrastructure.Repositories.DbContexts;
using Infrastructure.Repositories.Interfaces;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.Repositories
{
    public class RepositoryCliente : IRepository<Cliente>
    {
        private readonly LinwayDbContext _context;
        public RepositoryCliente()
        {
            _context = new LinwayDbContext();
        }
        public bool Add(Cliente t)
        {
            try
            {
                _context.Set<Cliente>().Add(t);
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
        public bool AddMany(ICollection<Cliente> t)
        {
            try
            {
                _context.Set<Cliente>().AddRange(t);
                _context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
        public bool Edit(Cliente t)
        {
            using var context = new LinwayDbContext();
            try
            {
                _context.Set<Cliente>().Update(t);
                _context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
        public bool EditMany(ICollection<Cliente> t)
        {
            using var context = new LinwayDbContext();
            try
            {
                _context.Set<Cliente>().UpdateRange(t);
                _context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
        public Cliente Get(long id)
        {
            try
            {
                return _context.Set<Cliente>().Find(id);
            }
            catch
            {
                return default;
            }
        }
        public List<Cliente> GetAll()
        {
            try
            {
                List<Cliente> lista = _context.Set<Cliente>().ToList();
                return lista;
            }
            catch
            {
                return default;
            }
        }
    }
}
