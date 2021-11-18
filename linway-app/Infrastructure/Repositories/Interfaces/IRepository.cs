using System.Collections.Generic;

namespace Infrastructure.Repositories.Interfaces
{
    public interface IRepository<T>
    {
        bool Add(T t);
        bool Delete(T t);
        bool Edit(T t);
        T Get(long id);
        List<T> GetAll();
    }
}