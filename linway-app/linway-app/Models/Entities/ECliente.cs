using System.ComponentModel;

namespace linway_app.Models.Entities
{
    public class ECliente
    {
        [DisplayName("Id:")]
        public long Id { get; set; }

        [DisplayName("Dirección - Localidad:")]
        public string Direccion { get; set; }
        
        [DisplayName("Nombre")]
        public string Nombre { get; set; }

        [DisplayName("CP")]
        public string CodigoPostal { get; set; }

        [DisplayName("Teléfono")]
        public string Telefono { get; set; }

        [DisplayName("CUIT")]
        public string Cuit { get; set; }

        [DisplayName("Tipo R.")]
        public string Tipo { get; set; }
    }
}
