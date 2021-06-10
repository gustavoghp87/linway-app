using linway_app.Models.Interfaces;
using linway_app.Models.OModel;
using System.Collections.Generic;

namespace linway_app.Models
{
    public partial class RegistroVenta : ObjModel, IRegistroVenta
    {
        public RegistroVenta()
        {
            ProdVendido = new HashSet<ProdVendido>();
        }
        public long ClienteId { get; set; }
        public string NombreCliente { get; set; }
        public string Fecha { get; set; }
        public virtual Cliente Cliente { get; set; }
        public virtual ICollection<ProdVendido> ProdVendido { get; set; }
    }
}
