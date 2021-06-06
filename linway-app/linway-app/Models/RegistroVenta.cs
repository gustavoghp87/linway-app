using linway_app.Models.Interfaces;
using System.Collections.Generic;
using System.ComponentModel;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace linway_app.Models
{
    public partial class RegistroVenta : Model, IRegistroVenta
    {
        public RegistroVenta()
        {
            ProdVendido = new HashSet<ProdVendido>();
        }

        public long Id { get; set; }
        public long ClienteId { get; set; }

        [DisplayName("CLIENTE")]
        public string NombreCliente { get; set; }

        [DisplayName("FECHA")]
        public string Fecha { get; set; }
        public string Estado { get; set; }

        public virtual Cliente Cliente { get; set; }
        public virtual ICollection<ProdVendido> ProdVendido { get; set; }
    }
}
