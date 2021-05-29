using System;
using System.Collections.Generic;
using System.ComponentModel;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace linway_app.Models
{
    public partial class ProdVendido : IProdVendido
    {
        public long Id { get; set; }
        public long ProductoId { get; set; }
        public long? NotaDeEnvioId { get; set; }
        public long? RegistroVentaId { get; set; }
        public long? PedidoId { get; set; }
        public long Cantidad { get; set; }

        [DisplayName("Detalle")]
        public string Descripcion { get; set; }
        public double Precio { get; set; }

        public virtual NotaDeEnvio NotaDeEnvio { get; set; }
        public virtual Pedido Pedido { get; set; }
        public virtual Producto Producto { get; set; }
        public virtual RegistroVenta RegistroVenta { get; set; }
    }
}
