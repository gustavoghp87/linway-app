using linway_app.Models;
using linway_app.Models.Interfaces;
using linway_app.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace linway_app.Repositories
{
    public class RepositoryBase<T> : IRepository<T> where T : Model
    {
        private readonly LinwaydbContext _context;
        private readonly DbSet<T> _entities;
        public RepositoryBase(LinwaydbContext context)
        {
            _context = context;
            _entities = context.Set<T>();
        }
        public bool Add(T t)
        {
            try
            {
                _entities.Add(t);
                _context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
        public bool Delete(T t)
        {
            return Edit(t);
        }
        public bool Edit(T t)
        {
            try
            {
                _entities.Update(t);
                _context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
        public T Get(long id)
        {
            using (var context = new LinwaydbContext())
            {
                return context.Set<T>()?.Find(id);
            }
        }
        public List<T> GetAll()
        {
            using (var context = new LinwaydbContext())
            {
                return context.Set<T>()?.ToList();
            }
        }
    }
}
