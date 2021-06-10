using System.Collections.Generic;

namespace linway_app.Models.Interfaces
{
    public interface IProducto
    {
        string Nombre { get; set; }
        decimal Precio { get; set; }
        ICollection<ProdVendido> ProdVendido { get; set; }
        ICollection<Venta> Venta { get; set; }
    }
}