using Models.OModel;
using System.Collections.Generic;

namespace Models
{
    public partial class Producto : ObjModel
    {
        public Producto()
        {
            ProdVendido = new HashSet<ProdVendido>();
            Venta = new HashSet<Venta>();
        }
        public string Nombre { get; set; }
        public decimal Precio { get; set; }
        public string Tipo { get; set; }
        public string SubTipo { get; set; }
        public virtual ICollection<ProdVendido> ProdVendido { get; set; }
        public virtual ICollection<Venta> Venta { get; set; }
    }
}
