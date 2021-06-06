using linway_app.Models.Interfaces;
using linway_app.Models.OModel;
using System.Collections.Generic;
using System.ComponentModel;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace linway_app.Models
{
    public partial class NotaDeEnvio : ObjModel
    {
        public NotaDeEnvio()
        {
            ProdVendidos = new HashSet<ProdVendido>();
        }

        public long Id { get; set; }
        public long ClientId { get; set; }
        public string Fecha { get; set; }
        public long Impresa { get; set; }
        public string Detalle { get; set; }

        [DisplayName("Total")]
        public double ImporteTotal { get; set; }
        public string Estado { get; set; }
        public virtual Cliente Client { get; set; }
        public virtual ICollection<ProdVendido> ProdVendidos { get; set; }
    }
}
