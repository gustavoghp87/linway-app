using System.ComponentModel;

namespace AppServices.DTOs
{
    public class EProducto
    {
        [DisplayName("Id")]
        public long Id { get; set; }

        [DisplayName("Producto")]
        public string Nombre { get; set; }

        [DisplayName("Precio($)")]
        public decimal Precio { get; set; }
        public string Tipo { get; set; }
        public string SubTipo { get; set; }
    }
}
