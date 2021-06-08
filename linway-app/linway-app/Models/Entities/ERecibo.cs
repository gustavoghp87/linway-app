using linway_app.Models.OModel;
using System.ComponentModel;

namespace linway_app.Models.Entities
{
    public partial class ERecibo : ObjModel
    {
        public long Id { get; set; }

        [DisplayName("Cliente")]
        public long ClienteId { get; set; }

        [DisplayName("Dirección")]
        public string DireccionCliente { get; set; }
        public string Fecha { get; set; }
        public string Impreso { get; set; }

        [DisplayName("Total")]
        public double ImporteTotal { get; set; }
    }
}
