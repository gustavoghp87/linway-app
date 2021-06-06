using System.Collections.Generic;

namespace linway_app.Models.Interfaces
{
    public interface IProducto
    {
        long Id { get; set; }
        string Nombre { get; set; }
        double Precio { get; set; }
        ICollection<ProdVendido> ProdVendido { get; set; }
        ICollection<Venta> Venta { get; set; }
        string Estado { get; set; }
    }
}