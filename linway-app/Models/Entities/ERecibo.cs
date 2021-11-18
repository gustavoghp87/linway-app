using Models.OModel;
using System.ComponentModel;

namespace Models.Entities
{
    public partial class ERecibo
    {
        public long Id { get; set; }

        [DisplayName("Cliente")]
        public long ClienteId { get; set; }

        [DisplayName("Dirección")]
        public string DireccionCliente { get; set; }
        public string Fecha { get; set; }
        public string Impreso { get; set; }

        [DisplayName("Total")]
        public decimal ImporteTotal { get; set; }
    }
}
