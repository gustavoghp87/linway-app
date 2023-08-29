using Infrastructure.Repositories.DbContexts;
using Infrastructure.Repositories.Interfaces;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.Repositories
{
    public class RepositoryPedido : IRepository<Pedido>
    {
        private readonly LinwayDbContext _context;
        public RepositoryPedido()
        {
            _context = new LinwayDbContext();
        }
        public bool Add(Pedido t)
        {
            try
            {
                _context.Set<Pedido>().Add(t);
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
        public bool AddMany(ICollection<Pedido> t)
        {
            try
            {
                _context.Set<Pedido>().AddRange(t);
                _context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
        public bool Edit(Pedido t)
        {
            using var context = new LinwayDbContext();
            try
            {
                _context.Set<Pedido>().Update(t);
                _context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
        public bool EditMany(ICollection<Pedido> t)
        {
            using var context = new LinwayDbContext();
            try
            {
                _context.Set<Pedido>().UpdateRange(t);
                _context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
        public Pedido Get(long id)
        {
            try
            {
                return _context.Set<Pedido>().Find(id);
            }
            catch
            {
                return default;
            }
        }
        public List<Pedido> GetAll()
        {
            try
            {
                List<Pedido> lista = _context.Set<Pedido>().ToList();
                return lista;
            }
            catch
            {
                return default;
            }
        }
    }
}
