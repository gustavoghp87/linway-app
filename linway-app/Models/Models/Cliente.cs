using Models.OModel;
using System.Collections.Generic;

namespace Models
{
    public partial class Cliente : ObjModel
    {
        public Cliente()
        {
            NotaDeEnvios = new HashSet<NotaDeEnvio>();
            Pedidos = new HashSet<Pedido>();
            Recibos = new HashSet<Recibo>();
            RegistroVentas = new HashSet<RegistroVenta>();
        }
        public string Direccion { get; set; }
        public string CodigoPostal { get; set; }
        public string Telefono { get; set; }
        public string Nombre { get; set; }
        public string Cuit { get; set; }
        public string Tipo { get; set; }
        public virtual ICollection<NotaDeEnvio> NotaDeEnvios { get; set; }
        public virtual ICollection<Pedido> Pedidos { get; set; }
        public virtual ICollection<Recibo> Recibos { get; set; }
        public virtual ICollection<RegistroVenta> RegistroVentas { get; set; }
    }
}
