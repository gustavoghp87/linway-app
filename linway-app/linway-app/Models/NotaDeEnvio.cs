using linway_app.Models.OModel;
using System.Collections.Generic;

namespace linway_app.Models
{
    public partial class NotaDeEnvio : ObjModel
    {
        public NotaDeEnvio()
        {
            ProdVendidos = new HashSet<ProdVendido>();
        }
        public long ClienteId { get; set; }
        public string Fecha { get; set; }
        public long Impresa { get; set; }
        public string Detalle { get; set; }
        public decimal ImporteTotal { get; set; }
        public virtual Cliente Cliente { get; set; }
        public virtual ICollection<ProdVendido> ProdVendidos { get; set; }
    }
}
