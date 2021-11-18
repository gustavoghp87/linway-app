using Models.OModel;
using System.Collections.Generic;

namespace Models
{
    public partial class Recibo : ObjModel
    {
        public Recibo()
        {
            DetalleRecibos = new HashSet<DetalleRecibo>();
        }
        public long ClienteId { get; set; }
        public string Fecha { get; set; }
        public long Impreso { get; set; }
        public string DireccionCliente { get; set; }
        public decimal ImporteTotal { get; set; }
        public virtual Cliente Cliente { get; set; }
        public virtual ICollection<DetalleRecibo> DetalleRecibos { get; set; }
    }
}
