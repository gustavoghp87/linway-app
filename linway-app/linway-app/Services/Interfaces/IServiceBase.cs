using linway_app.Models.OModel;
using System.Collections.Generic;

namespace linway_app.Services.Interfaces
{
    public interface IServiceBase<T>
    {
        bool Add(T t);
        bool Delete(T t);
        bool Edit(T t);
        T Get(long id);
        List<T> GetAll();
    }
}