using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace linway_app.Services.Interfaces
{
    public interface IServicesBase<T>
    {
        void Add(T entity, CancellationToken cancellationToken = default);
        void AddMany(IEnumerable<T> entities, CancellationToken cancellationToken = default);
        void Edit(T entity, CancellationToken cancellationToken = default);
        void EditMany(IEnumerable<T> entities, CancellationToken cancellationToken = default);
        void Delete(T entity, CancellationToken cancellationToken = default);
        void DeleteMany(IEnumerable<T> entities, CancellationToken cancellationToken = default);
        Task<T> GetAsync(long id, CancellationToken cancellationToken = default);
        Task<List<T>> GetAllAsync(CancellationToken cancellationToken = default);
    }
}
