using Models.OModel;

namespace Models
{
    public partial class DetalleRecibo : ObjModel
    {
        public long ReciboId { get; set; }
        public string Detalle { get; set; }  // nombre del producto al momento de la creación del recibo... no se guarda el ID del producto
        public decimal Importe { get; set; }  // precio del producto al momento de la creación del recibo
        public virtual Recibo Recibo { get; set; }
    }
}
