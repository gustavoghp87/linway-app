using System.Collections.Generic;

namespace linway_app.Models
{
    public interface IRecibo
    {
        Cliente Cliente { get; set; }
        long ClienteId { get; set; }
        ICollection<DetalleRecibo> DetalleRecibos { get; set; }
        string DireccionCliente { get; set; }
        string Fecha { get; set; }
        long Id { get; set; }
        double ImporteTotal { get; set; }
        long Impresa { get; set; }
    }
}