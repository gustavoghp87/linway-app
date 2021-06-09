using System.ComponentModel;

namespace linway_app.Models.Entities
{
    public class EProdVendido
    {
        //public long Id { get; set; }

        [DisplayName("Id")]
        public long ProductoId { get; set; }

        //[DisplayName("Nota")]
        //public long? NotaDeEnvioId { get; set; }

        //[DisplayName("Registro")]
        //public long? RegistroVentaId { get; set; }

        //[DisplayName("Pedido")]
        //public long? PedidoId { get; set; }

        [DisplayName("Nombre")]
        public string Descripcion { get; set; }
        public long Cantidad { get; set; }
        public double Precio { get; set; }
        public double Total { get; set; }
    }
}
