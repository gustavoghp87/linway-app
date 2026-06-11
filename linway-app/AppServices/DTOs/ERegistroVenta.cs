using System.ComponentModel;

namespace AppServices.DTOs
{
    public class ERegistroVenta
    {
        public long Id { get; set; }
        
        [DisplayName("CLIENTE")]
        public string NombreCliente { get; set; }

        [DisplayName("FECHA")]
        public string Fecha { get; set; }
    }
}
