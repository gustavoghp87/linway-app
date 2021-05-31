using linway_app.Models;
using System.Collections.Generic;

namespace linway_app.Services.Interfaces
{
    public interface IServicioRegistroVenta
    {
        bool Add(RegistroVenta registroVenta);
        bool Delete(RegistroVenta registroVenta);
        bool Edit(RegistroVenta registroVenta);
        RegistroVenta Get(long id);
        List<RegistroVenta> GetAll();
        bool ModificarClienteId(long clienteId, RegistroVenta registroVenta);
    }
}