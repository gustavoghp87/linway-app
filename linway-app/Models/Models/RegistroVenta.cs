using Models.OModel;
using System.Collections.Generic;

namespace Models
{
    public partial class RegistroVenta : ObjModel
    {
        public RegistroVenta()
        {
            ProdVendido = new HashSet<ProdVendido>();
        }
        public long ClienteId { get; set; }
        public string NombreCliente { get; set; }  // en realidad es la Dirección al momento de la venta
        public string Fecha { get; set; }
        public virtual Cliente Cliente { get; set; }
        public virtual ICollection<ProdVendido> ProdVendido { get; set; }
    }
}
