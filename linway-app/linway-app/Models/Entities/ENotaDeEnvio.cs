using linway_app.Models.OModel;
using System.ComponentModel;

namespace linway_app.Models.Entities
{
    public partial class ENotaDeEnvio : ObjModel
    {
        public long Id { get; set; }
        public string Fecha { get; set; }
        public string Impresa { get; set; }
        public string Detalle { get; set; }

        [DisplayName("Total")]
        public double ImporteTotal { get; set; }
        public Cliente Cliente { get; set; }
    }
}
