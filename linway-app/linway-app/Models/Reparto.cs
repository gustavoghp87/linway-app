using linway_app.Models.OModel;
using System.Collections.Generic;

namespace linway_app.Models
{
    public partial class Reparto : ObjModel
    {
        public Reparto()
        {
            Pedidos = new HashSet<Pedido>();
        }
        public string Nombre { get; set; }
        public long DiaRepartoId { get; set; }
        public long Ta { get; set; }
        public long Te { get; set; }
        public long Td { get; set; }
        public long Tt { get; set; }
        public long Tae { get; set; }
        public long Tl { get; set; }
        public long TotalB { get; set; }
        public virtual DiaReparto DiaReparto { get; set; }
        public virtual ICollection<Pedido> Pedidos { get; set; }
    }
}
