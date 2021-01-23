using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;


namespace linway_app
{
    [Serializable()]

    public class DetalleRecibo
    {
        public string detalle { get; set; }
        public float importe { get; set; }

        public DetalleRecibo(string det, float imp)
        {
            this.detalle = det;
            this.importe = imp;
        }
    }
}
