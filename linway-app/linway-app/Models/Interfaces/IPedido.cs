using System.Collections.Generic;

namespace linway_app.Models.Interfaces
{
    public interface IPedido
    {
        long A { get; set; }
        long Ae { get; set; }
        Cliente Cliente { get; set; }
        long ClienteId { get; set; }
        long D { get; set; }
        string Direccion { get; set; }
        long E { get; set; }
        long Entregar { get; set; }
        long Id { get; set; }
        long L { get; set; }
        string Productos { get; set; }
        ICollection<ProdVendido> ProdVendidos { get; set; }
        Reparto Reparto { get; set; }
        long RepartoId { get; set; }
        long T { get; set; }
        string Estado { get; set; }
    }
}