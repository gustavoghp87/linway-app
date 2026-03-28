using Models.OModel;
using System.Collections.Generic;

namespace Models
{
    public partial class NotaDeEnvio : ObjModel
    {
        public NotaDeEnvio()
        {
            ProdVendidos = new HashSet<ProdVendido>();
        }
        public long ClienteId { get; set; }  // no debería tener una columna DireccionCliente para guardar la dirección del momento de la creación de la nota?
        public string Fecha { get; set; }
        public long Impresa { get; set; }
        public string Detalle { get; set; }
        public decimal ImporteTotal { get; set; }
        public virtual Cliente Cliente { get; set; }
        public virtual ICollection<ProdVendido> ProdVendidos { get; set; }
    }
}
