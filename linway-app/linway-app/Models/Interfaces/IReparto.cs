using System.Collections.Generic;

namespace linway_app.Models
{
    public interface IReparto
    {
        DiaReparto DiaReparto { get; set; }
        long DiaRepartoId { get; set; }
        long Id { get; set; }
        string Nombre { get; set; }
        ICollection<Pedido> Pedidos { get; set; }
        long Ta { get; set; }
        long Tae { get; set; }
        long Td { get; set; }
        long Te { get; set; }
        long Tl { get; set; }
        long TotalB { get; set; }
        long Tt { get; set; }
    }
}