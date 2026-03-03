using Models.OModel;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.Interfaces
{
    public interface IUnitOfWork : IDisposable, IAsyncDisposable
    {
        //int Save();
        Task<int> SaveAsync(CancellationToken cancellationToken = default);
        void ExecuteInTransaction(Action action);
        Task ExecuteInTransactionAsync(Func<CancellationToken, Task> action, CancellationToken cancellationToken = default);
    }
}
