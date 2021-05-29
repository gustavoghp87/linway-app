using System.Collections.Generic;

namespace linway_app.Repositories
{
    public interface IRepository<T>
    {
        List<T> GetAll();
        T Get(long id);
        bool Add(T t);
        bool Delete(T t);
        bool Edit(T t);

        //Task<IEnumerable<T>> GetAll();
        //Task<T> GetById(int id);
        //Task Add(T entity);
        //Task Update(T entity);
        //Task Delete(int id);
    }
}