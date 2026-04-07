using Models.OModel;
using System.Collections.Generic;

namespace Models
{
    public partial class Pedido : ObjModel
    {
        public Pedido()
        {
            ProdVendidos = new HashSet<ProdVendido>();
        }
        public long ClienteId { get; set; }
        public long RepartoId { get; set; }
        public long Entregar { get; set; }
        public long Orden { get; set; }
        public virtual Cliente Cliente { get; set; }
        public virtual Reparto Reparto { get; set; }
        public virtual ICollection<ProdVendido> ProdVendidos { get; set; }
    }
}
