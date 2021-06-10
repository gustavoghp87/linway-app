using linway_app.Models.Interfaces;
using linway_app.Models.OModel;
using System.Collections.Generic;

namespace linway_app.Models
{
    public partial class Cliente : ObjModel, ICliente
    {
        public Cliente()
        {
            NotaDeEnvio = new HashSet<NotaDeEnvio>();
            Pedido = new HashSet<Pedido>();
            Recibo = new HashSet<Recibo>();
            RegistroVenta = new HashSet<RegistroVenta>();
        }
        public string Direccion { get; set; }
        public string CodigoPostal { get; set; }
        public string Telefono { get; set; }
        public string Nombre { get; set; }
        public string Cuit { get; set; }
        public string Tipo { get; set; }
        public virtual ICollection<NotaDeEnvio> NotaDeEnvio { get; set; }
        public virtual ICollection<Pedido> Pedido { get; set; }
        public virtual ICollection<Recibo> Recibo { get; set; }
        public virtual ICollection<RegistroVenta> RegistroVenta { get; set; }
    }
}
