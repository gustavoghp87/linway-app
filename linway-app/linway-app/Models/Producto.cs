using linway_app.Models.OModel;
using System.Collections.Generic;
using System.ComponentModel;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace linway_app.Models
{
    public partial class Producto : ObjModel
    {
        public Producto()
        {
            ProdVendido = new HashSet<ProdVendido>();
            Venta = new HashSet<Venta>();
        }

        public long Id { get; set; }

        [DisplayName("Producto")]
        public string Nombre { get; set; }

        [DisplayName("Precio($)")]
        public double Precio { get; set; }
        //public string Estado { get; set; }

        public virtual ICollection<ProdVendido> ProdVendido { get; set; }
        public virtual ICollection<Venta> Venta { get; set; }
    }
}
