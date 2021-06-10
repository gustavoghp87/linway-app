using linway_app.Models.Interfaces;
using linway_app.Models.OModel;
using System.Collections.Generic;

namespace linway_app.Models
{
    public partial class Recibo : ObjModel, IRecibo
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
