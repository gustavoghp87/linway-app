﻿using System.Collections.Generic;

namespace linway_app.Models
{
    public interface INotaDeEnvio
    {
        Cliente Client { get; set; }
        long ClientId { get; set; }
        string Detalle { get; set; }
        string Fecha { get; set; }
        long Id { get; set; }
        double ImporteTotal { get; set; }
        long Impresa { get; set; }
        ICollection<ProdVendido> ProdVendidos { get; set; }
    }
}