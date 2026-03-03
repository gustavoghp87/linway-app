using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.Interfaces
{
    public interface IRepository<T>
    {
        void Add(T entity);
        void AddMany(IEnumerable<T> entities);
        void Edit(T entity);
        void EditMany(IEnumerable<T> entities);
        T Get(long id);
        IQueryable<T> Query();
        Task<T> FirstOrDefaultAsync(IQueryable<T> query, CancellationToken cancellationToken = default);
        Task<List<T>> ToListAsync(IQueryable<T> query, CancellationToken cancellationToken = default);
    }
}
