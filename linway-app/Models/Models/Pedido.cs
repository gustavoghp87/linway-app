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
        public string Direccion { get; set; }
        public long ClienteId { get; set; }
        public long RepartoId { get; set; }
        public long Entregar { get; set; }
        public string ProductosText { get; set; }
        public long L { get; set; }
        public long A { get; set; }
        public long E { get; set; }
        public long D { get; set; }
        public long T { get; set; }
        public long Ae { get; set; }
        public long Orden { get; set; }
        public virtual Cliente Cliente { get; set; }
        public virtual Reparto Reparto { get; set; }
        public virtual ICollection<ProdVendido> ProdVendidos { get; set; }
    }
}
