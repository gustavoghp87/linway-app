using System.Threading;
using System.Threading.Tasks;

namespace linway_app.Services.Interfaces
{
    public interface ISavingServices
    {
        Task<bool> SaveAsync(CancellationToken ct = default);
    }
}
