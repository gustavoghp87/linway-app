using System;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.Interfaces
{
    public interface IUnitOfWork : IDisposable, IAsyncDisposable
    {
        void DiscardChanges();
        //void ExecuteInTransaction(Action action);
        //Task ExecuteInTransactionAsync(Func<CancellationToken, Task> action, CancellationToken cancellationToken = default);
        Task<int> SaveAsync(CancellationToken cancellationToken = default);
    }
}
