using System.Collections.Generic;

namespace linway_app.Models.Interfaces
{
    public interface INotaDeEnvio
    {
        Cliente Cliente { get; set; }
        long ClienteId { get; set; }
        string Detalle { get; set; }
        string Fecha { get; set; }
        decimal ImporteTotal { get; set; }
        long Impresa { get; set; }
        ICollection<ProdVendido> ProdVendidos { get; set; }
    }
}