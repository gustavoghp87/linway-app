using linway_app.Models.Interfaces;
using linway_app.Models.OModel;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace linway_app.Models
{
    public partial class DetalleRecibo : ObjModel
    {
        public long Id { get; set; }
        public long ReciboId { get; set; }
        public string Detalle { get; set; }
        public double Importe { get; set; }
        public string Estado { get; set; }
        public virtual Recibo Recibo { get; set; }
    }
}
