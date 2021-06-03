using System.Collections.Generic;

namespace linway_app.Models
{
    public interface IRegistroVenta
    {
        Cliente Cliente { get; set; }
        long ClienteId { get; set; }
        string Fecha { get; set; }
        long Id { get; set; }
        string NombreCliente { get; set; }
        ICollection<ProdVendido> ProdVendido { get; set; }
        string Estado { get; set; }
    }
}