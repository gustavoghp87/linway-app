using Infrastructure.Repositories.DbContexts;
using Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly LinwayDbContext _context;
        public UnitOfWork(LinwayDbContext context)
        {
            _context = context;
        }
        public void Dispose()
        {
            _context.Dispose();
        }
        public ValueTask DisposeAsync()
        {
            return _context.DisposeAsync();
        }
        public void DiscardChanges()
        {
            foreach (var entry in _context.ChangeTracker.Entries().ToList())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.State = EntityState.Detached;
                        break;

                    case EntityState.Modified:
                    case EntityState.Deleted:
                        entry.State = EntityState.Unchanged;
                        break;
                }
            }
        }
        //public void ExecuteInTransaction(Action action)
        //{
        //    using var tx = _context.Database.BeginTransaction();
        //    try
        //    {
        //        action();
        //        _context.SaveChanges();
        //        tx.Commit();
        //    }
        //    catch
        //    {
        //        tx.Rollback();
        //        throw;
        //    }
        //}
        //public async Task ExecuteInTransactionAsync(Func<CancellationToken, Task> action, CancellationToken cancellationToken = default)
        //{
        //    await using var tx = await _context.Database.BeginTransactionAsync(cancellationToken);
        //    try
        //    {
        //        await action(cancellationToken);
        //        await _context.SaveChangesAsync(cancellationToken);
        //        await tx.CommitAsync(cancellationToken);
        //    }
        //    catch
        //    {
        //        await tx.RollbackAsync(cancellationToken);
        //        throw;
        //    }
        //}
        public Task<int> SaveAsync(CancellationToken cancellationToken = default)
        {
            return _context.SaveChangesAsync(cancellationToken);
        }
    }
}
