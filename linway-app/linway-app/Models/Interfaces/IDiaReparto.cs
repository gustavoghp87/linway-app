using System.Collections.Generic;

namespace linway_app.Models.Interfaces
{
    public interface IDiaReparto
    {
        string Dia { get; set; }
        long Id { get; set; }
        ICollection<Reparto> Reparto { get; set; }
    }
}