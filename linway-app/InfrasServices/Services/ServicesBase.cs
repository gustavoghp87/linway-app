using Infrastructure.Repositories.Interfaces;
using linway_app.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Models.OModel;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace linway_app.Services
{
    public class ServicesBase<T> : IServicesBase<T> where T : ObjModel
    {
        private readonly IRepository<T> _repository;
        public ServicesBase(IRepository<T> repository)
        {
            _repository = repository;
        }
        public void Add(T entity, CancellationToken ct = default)
        {
            entity.Estado = "Activo";
            _repository.Add(entity);
        }
        public void AddMany(IEnumerable<T> entities, CancellationToken ct = default)
        {
            foreach (var entity in entities)
            {
                entity.Estado = "Activo";
            }
            _repository.AddMany(entities);
        }
        public void Edit(T entity, CancellationToken ct = default)
        {
            _repository.Edit(entity);
        }
        public void EditMany(IEnumerable<T> entities, CancellationToken ct = default)
        {
            _repository.EditMany(entities);
        }
        public void Delete(T entity, CancellationToken ct = default)
        {
            _repository.Delete(entity);
        }
        public void DeleteMany(IEnumerable<T> entities, CancellationToken ct = default)
        {
            _repository.DeleteMany(entities);
        }
        public async Task<T> GetAsync(long id, CancellationToken ct = default)
        {
            return await _repository
               .Query()
               .Where(x => x.Id == id && x.Estado != "Eliminado")
               .FirstOrDefaultAsync(ct);
        }
        public async Task<List<T>> GetAllAsync(CancellationToken ct = default)
        {
            return await _repository
                .Query()
                .Where(x => x.Estado != "Eliminado")
                .ToListAsync(ct);
        }
    }
}
