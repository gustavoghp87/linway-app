using System.ComponentModel;

namespace Models.Entities
{
    public class EVenta
    {
        [DisplayName("Id")]
        public int ProductoId { get; set; }
        public string Detalle { get; set; }

        [DisplayName("Cant")]
        public long Cantidad { get; set; }
    }
}
