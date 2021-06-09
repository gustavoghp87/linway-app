using linway_app.Models.OModel;

namespace linway_app.Models.Entities
{
    public partial class EPedido
    {
        public long Id { get; set; }
        public string Direccion { get; set; }
        //public long ClienteId { get; set; }
        //public long RepartoId { get; set; }
        public string ProductosText { get; set; }
        public string Entregar { get; set; }
        public long L { get; set; }
        public long A { get; set; }
        public long E { get; set; }
        public long D { get; set; }
        public long T { get; set; }
        public long Ae { get; set; }
    }
}
