﻿using linway_app.Models.Interfaces;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace linway_app.Models
{
    public partial class Venta : Model, IVenta
    {
        public long Id { get; set; }
        public long ProductoId { get; set; }
        public long Cantidad { get; set; }
        public string Estado { get; set; }
        public virtual Producto Producto { get; set; }
    }
}
