using Infrastructure.Repositories.DbContexts;
using Infrastructure.Repositories.Interfaces;
using Models.OModel;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.Repositories
{
    public class RepositoryBase<T> : IRepository<T> where T : ObjModel
    {
        private readonly LinwayDbContext _context;
        public RepositoryBase(LinwayDbContext context)
        {
            _context = context;
        }
        public void Add(T t)
        {
             _context.Set<T>().AddAsync(t);
        }
        public void AddMany(IEnumerable<T> t)
        {
            _context.Set<T>().AddRangeAsync(t);
        }
        public void Delete(T t)
        {
            _context.Set<T>().Remove(t);
        }
        public void DeleteMany(IEnumerable<T> t)
        {
            _context.Set<T>().RemoveRange(t);
        }
        public void Edit(T t)
        {
            _context.Set<T>().Update(t);
        }
        public void EditMany(IEnumerable<T> t)
        {
            _context.Set<T>().UpdateRange(t);
        }
        //public T Get(long id)
        //{
        //    return _context.Set<T>().Find(id);
        //}
        //public IQueryable<T> GetAll()  // no se usa, se usa Query
        //{
        //    return _context.Set<T>();
        //}
        public IQueryable<T> Query()
        {
            return _context.Set<T>();
        }
        //public Task<T> FirstOrDefaultAsync(IQueryable<T> query, CancellationToken cancellationToken = default)
        //{
        //    return query.FirstOrDefaultAsync(cancellationToken);
        //}
        //public Task<List<T>> ToListAsync(IQueryable<T> query, CancellationToken cancellationToken = default)
        //{
        //    return query.ToListAsync(cancellationToken);
        //}
    }
}
