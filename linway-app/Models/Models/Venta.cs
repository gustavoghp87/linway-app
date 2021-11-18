using Models.OModel;

namespace Models
{
    public partial class Venta : ObjModel
    {
        public long ProductoId { get; set; }
        public long Cantidad { get; set; }
        public virtual Producto Producto { get; set; }
    }
}
