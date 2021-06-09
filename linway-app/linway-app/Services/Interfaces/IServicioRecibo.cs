using linway_app.Models;
using System.Collections.Generic;

namespace linway_app.Services.Interfaces
{
    public interface IServicioRecibo
    {
        bool Add(Recibo recibo);
        bool Delete(Recibo recibo);
        bool Edit(Recibo recibo);
        Recibo Get(long id);
    }
}