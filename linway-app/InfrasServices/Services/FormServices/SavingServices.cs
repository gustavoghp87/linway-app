using linway_app.Services.Interfaces;
using Infrastructure.Repositories.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace linway_app.Services.FormServices
{
    public class SavingServices : ISavingServices
    {
        protected readonly IUnitOfWork _unitOfWork;
        public SavingServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public void DiscardChanges(CancellationToken ct = default)
        {
            _unitOfWork.DiscardChanges();
        }
        public async Task<bool> SaveAsync(CancellationToken ct = default)
        {
            int entries = await _unitOfWork.SaveAsync(ct);
            return entries > 0;
        }
    }
}
