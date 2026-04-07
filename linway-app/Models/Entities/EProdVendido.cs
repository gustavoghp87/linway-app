using System.ComponentModel;

namespace Models.Entities
{
    public class EProdVendido
    {
        [DisplayName("Id")]
        public long ProductoId { get; set; }

        [DisplayName("Nombre")]
        public string Descripcion { get; set; }
        
        [DisplayName("Cant")]
        public long Cantidad { get; set; }
        public decimal Precio { get; set; }

        [DisplayName("Total($)")]
        public decimal Total { get; set; }
    }
}
