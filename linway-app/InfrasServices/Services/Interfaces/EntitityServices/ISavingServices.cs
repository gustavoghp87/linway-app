using System.Threading;
using System.Threading.Tasks;

namespace linway_app.Services.Interfaces
{
    public interface ISavingServices
    {
        void DiscardChanges(CancellationToken ct = default);
        Task<bool> SaveAsync(CancellationToken ct = default);
    }
}
