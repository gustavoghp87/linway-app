using linway_app.Models.Interfaces;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace linway_app.Models
{
    public partial class Pedido : Model, IPedido
    {
        public Pedido()
        {
            ProdVendidos = new HashSet<ProdVendido>();
        }

        public long Id { get; set; }
        public string Direccion { get; set; }
        public long ClienteId { get; set; }
        public long RepartoId { get; set; }
        public long Entregar { get; set; }
        public long L { get; set; }
        public long A { get; set; }
        public long E { get; set; }
        public long D { get; set; }
        public long T { get; set; }
        public long Ae { get; set; }
        public string Productos { get; set; }
        public string Estado { get; set; }

        public virtual Cliente Cliente { get; set; }
        public virtual Reparto Reparto { get; set; }
        public virtual ICollection<ProdVendido> ProdVendidos { get; set; }
    }
}
