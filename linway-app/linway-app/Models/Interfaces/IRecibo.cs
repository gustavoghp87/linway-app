using System.Collections.Generic;

namespace linway_app.Models.Interfaces
{
    public interface IRecibo
    {
        Cliente Cliente { get; set; }
        long ClienteId { get; set; }
        ICollection<DetalleRecibo> DetalleRecibos { get; set; }
        string DireccionCliente { get; set; }
        string Fecha { get; set; }
        decimal ImporteTotal { get; set; }
        long Impreso { get; set; }
    }
}