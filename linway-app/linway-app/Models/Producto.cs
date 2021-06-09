using linway_app.Models.OModel;
using System.Collections.Generic;

namespace linway_app.Models
{
    public partial class Producto : ObjModel
    {
        public Producto()
        {
            ProdVendido = new HashSet<ProdVendido>();
            Venta = new HashSet<Venta>();
        }
        public string Nombre { get; set; }
        public double Precio { get; set; }
        public virtual ICollection<ProdVendido> ProdVendido { get; set; }
        public virtual ICollection<Venta> Venta { get; set; }
    }
}
