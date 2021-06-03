using linway_app.Models;
using System.Collections.Generic;

namespace linway_app.Services.Interfaces
{
    public interface IServicioPedido
    {
        bool Add(Pedido reparto);
        bool Delete(Pedido reparto);
        bool Edit(Pedido reparto);
        Pedido Get(long id);
        List<Pedido> GetAll();
        bool AgregarDesdeNota(string diaDeReparto, string nombreReparto, long notaDeEnvioId);
    }
}