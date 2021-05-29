using linway_app.Models;
using System.Collections.Generic;

namespace linway_app.Services.Interfaces
{
    public interface IServicioDiaReparto
    {
        bool Add(DiaReparto diaReparto);
        bool Delete(DiaReparto diaReparto);
        bool Edit(DiaReparto diaReparto);
        DiaReparto Get(long id);
        List<DiaReparto> GetAll();
    }
}