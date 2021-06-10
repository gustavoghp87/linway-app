using linway_app.Models.Interfaces;
using linway_app.Models.OModel;
using System.Collections.Generic;

namespace linway_app.Models
{
    public partial class Producto : ObjModel, IProducto
    {
        public Producto()
        {
            ProdVendido = new HashSet<ProdVendido>();
            Venta = new HashSet<Venta>();
        }
        public string Nombre { get; set; }
        public decimal Precio { get; set; }
        public virtual ICollection<ProdVendido> ProdVendido { get; set; }
        public virtual ICollection<Venta> Venta { get; set; }
    }
}
