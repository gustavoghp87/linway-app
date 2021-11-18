using Models.OModel;

namespace Models
{
    public partial class ProdVendido : ObjModel
    {
        public long ProductoId { get; set; }
        public long? NotaDeEnvioId { get; set; }
        public long? RegistroVentaId { get; set; }
        public long? PedidoId { get; set; }
        public long Cantidad { get; set; }
        public string Descripcion { get; set; }
        public decimal Precio { get; set; }
        public virtual NotaDeEnvio NotaDeEnvio { get; set; }
        public virtual Pedido Pedido { get; set; }
        public virtual Producto Producto { get; set; }
        public virtual RegistroVenta RegistroVenta { get; set; }
    }
}
