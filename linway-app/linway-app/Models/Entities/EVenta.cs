using System.ComponentModel;

namespace linway_app.Models.Entities
{
    public class EVenta
    {
        public string Detalle { get; set; }

        [DisplayName("Cant")]
        public long Cantidad { get; set; }
    }
}
