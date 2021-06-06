using linway_app.Models.Interfaces;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace linway_app.Models
{
    public partial class Reparto : Model, IReparto
    {
        public Reparto()
        {
            Pedidos = new HashSet<Pedido>();
        }

        public long Id { get; set; }
        public string Nombre { get; set; }
        public long DiaRepartoId { get; set; }
        public long Ta { get; set; }
        public long Te { get; set; }
        public long Td { get; set; }
        public long Tt { get; set; }
        public long Tae { get; set; }
        public long TotalB { get; set; }
        public long Tl { get; set; }
        public string Estado { get; set; }

        public virtual DiaReparto DiaReparto { get; set; }
        public virtual ICollection<Pedido> Pedidos { get; set; }
    }
}
