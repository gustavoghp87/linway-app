using System.ComponentModel;

namespace linway_app.Models.Entities
{
    public partial class ENotaDeEnvio
    {
        public long Id { get; set; }
        public string Fecha { get; set; }

        [DisplayName("Dirección")]
        public string Direccion { get; set; }
        public string Detalle { get; set; }

        [DisplayName("Total")]
        public decimal ImporteTotal { get; set; }
        public string Impresa { get; set; }
    }
}
