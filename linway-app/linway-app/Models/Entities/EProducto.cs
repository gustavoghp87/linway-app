using System.ComponentModel;

namespace linway_app.Models.Entities
{
    public class EProducto
    {
        public long Id { get; set; }

        [DisplayName("Producto")]
        public string Nombre { get; set; }

        [DisplayName("Precio($)")]
        public decimal Precio { get; set; }
        public string Tipo { get; set; }
        public string SubTipo { get; set; }
    }
}
