using linway_app.Models.Interfaces;
using linway_app.Models.OModel;
using System.Collections.Generic;
using System.ComponentModel;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace linway_app.Models
{
    public partial class Cliente : ObjModel
    {
        public Cliente()
        {
            NotaDeEnvio = new HashSet<NotaDeEnvio>();
            Pedido = new HashSet<Pedido>();
            Recibo = new HashSet<Recibo>();
            RegistroVenta = new HashSet<RegistroVenta>();
        }

        [DisplayName("Cod:")]
        public long Id { get; set; }

        [DisplayName("Dirección - Localidad:")]
        public string Direccion { get; set; }

        [DisplayName("CP")]
        public string CodigoPostal { get; set; }

        [DisplayName("Teléfono")]
        public string Telefono { get; set; }

        [DisplayName("Nombre del Cliente")]
        public string Name { get; set; }

        [DisplayName("CUIT")]
        public string Cuit { get; set; }

        [DisplayName("Tipo R.")]
        public string Tipo { get; set; }

        public string Estado { get; set; }

        public virtual ICollection<NotaDeEnvio> NotaDeEnvio { get; set; }
        public virtual ICollection<Pedido> Pedido { get; set; }
        public virtual ICollection<Recibo> Recibo { get; set; }
        public virtual ICollection<RegistroVenta> RegistroVenta { get; set; }
    }
}
