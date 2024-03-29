﻿using Models.OModel;

namespace Models
{
    public partial class DetalleRecibo : ObjModel
    {
        public long ReciboId { get; set; }
        public string Detalle { get; set; }
        public decimal Importe { get; set; }
        public virtual Recibo Recibo { get; set; }
    }
}
