using System.ComponentModel;

namespace Models.Entities
{
    public class ERegistroVenta
    {
        public long Id { get; set; }
        //public long ClienteId { get; set; }
        
        [DisplayName("CLIENTE")]
        public string NombreCliente { get; set; }

        [DisplayName("FECHA")]
        public string Fecha { get; set; }
    }
}
