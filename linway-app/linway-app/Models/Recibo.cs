using linway_app.Models.Interfaces;
using linway_app.Models.OModel;
using System.Collections.Generic;
using System.ComponentModel;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace linway_app.Models
{
    public partial class Recibo : ObjModel
    {
        public Recibo()
        {
            DetalleRecibos = new HashSet<DetalleRecibo>();
        }

        public long Id { get; set; }
        public long ClienteId { get; set; }
        public string Fecha { get; set; }
        public long Impreso { get; set; }
        public string DireccionCliente { get; set; }

        [DisplayName("Total")]
        public double ImporteTotal { get; set; }
        public string Estado { get; set; }
        public virtual Cliente Cliente { get; set; }
        public virtual ICollection<DetalleRecibo> DetalleRecibos { get; set; }
    }
}
