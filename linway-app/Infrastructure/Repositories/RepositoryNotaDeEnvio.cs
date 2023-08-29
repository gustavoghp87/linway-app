using Infrastructure.Repositories.DbContexts;
using Infrastructure.Repositories.Interfaces;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.Repositories
{
    public class RepositoryNotaDeEnvio : IRepository<NotaDeEnvio>
    {
        private readonly LinwayDbContext _context;
        public RepositoryNotaDeEnvio()
        {
            _context = new LinwayDbContext();
        }
        public bool Add(NotaDeEnvio t)
        {
            try
            {
                _context.Set<NotaDeEnvio>().Add(t);
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
        public bool AddMany(ICollection<NotaDeEnvio> t)
        {
            try
            {
                _context.Set<NotaDeEnvio>().AddRange(t);
                _context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
        public bool Edit(NotaDeEnvio t)
        {
            using var context = new LinwayDbContext();
            try
            {
                _context.Set<NotaDeEnvio>().Update(t);
                _context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
        public bool EditMany(ICollection<NotaDeEnvio> t)
        {
            using var context = new LinwayDbContext();
            try
            {
                _context.Set<NotaDeEnvio>().UpdateRange(t);
                _context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
        public NotaDeEnvio Get(long id)
        {
            try
            {
                return _context.Set<NotaDeEnvio>().Find(id);
            }
            catch
            {
                return default;
            }
        }
        public List<NotaDeEnvio> GetAll()
        {
            try
            {
                List<NotaDeEnvio> lista = _context.Set<NotaDeEnvio>().ToList();
                return lista;
            }
            catch
            {
                return default;
            }
        }
    }
}
