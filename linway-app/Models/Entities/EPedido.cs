using Models.OModel;
using System.ComponentModel;

namespace Models.Entities
{
    public partial class EPedido
    {
        public long Id { get; set; }

        [DisplayName("Clien")]
        public long ClienteId { get; set; }

        [DisplayName("Dirección")]
        public string Direccion { get; set; }

        //public long RepartoId { get; set; }

        [DisplayName("Productos")]
        public string ProductosText { get; set; }
        public string Entregar { get; set; }

        [DisplayName("Ltr")]
        public long L { get; set; }
        public long A { get; set; }
        public long E { get; set; }
        public long D { get; set; }
        public long T { get; set; }
        public long Ae { get; set; }
        public long Orden { get; set; }
    }
}
