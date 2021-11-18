using System.ComponentModel;

namespace Models.Entities
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
        
        [DisplayName("Cant")]
        public long Cantidad { get; set; }
        public decimal Precio { get; set; }

        [DisplayName("Total($)")]
        public decimal Total { get; set; }
    }
}
