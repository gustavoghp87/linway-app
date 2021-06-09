using linway_app.Models;
using System.Collections.Generic;

namespace linway_app.Services.Interfaces
{
    public interface IServicioNotaDeEnvio
    {
        bool Add(NotaDeEnvio notaDeEnvio);
        bool Delete(NotaDeEnvio notaDeEnvio);
        bool Edit(NotaDeEnvio notaDeEnvio);
        NotaDeEnvio Get(long id);
        List<NotaDeEnvio> GetAll();
    }
}