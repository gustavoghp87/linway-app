using System.Collections.Generic;

namespace linway_app.Models.Interfaces
{
    public interface IRegistroVenta
    {
        Cliente Cliente { get; set; }
        long ClienteId { get; set; }
        string Fecha { get; set; }
        string NombreCliente { get; set; }
        ICollection<ProdVendido> ProdVendido { get; set; }
    }
}