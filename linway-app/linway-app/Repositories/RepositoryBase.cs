using linway_app.Models;
using linway_app.Models.OModel;
using linway_app.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace linway_app.Repositories
{
    public class RepositoryBase<T> : IRepository<T> where T : ObjModel
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
            using (var context = new LinwaydbContext())
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
        }
        public T Get(long id)
        {
            try
            {
                return _entities.Find(id);
            }
            catch
            {
                return null;
            }
        }
        public List<T> GetAll()
        {
            try
            {
                return _entities.ToList();
            }
            catch
            {
                return null;
            }
        }
    }
}
