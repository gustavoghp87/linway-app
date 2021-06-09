using linway_app.Models.OModel;

namespace linway_app.Models
{
    public partial class Venta : ObjModel
    {
        public long ProductoId { get; set; }
        public long Cantidad { get; set; }
        public virtual Producto Producto { get; set; }
    }
}
