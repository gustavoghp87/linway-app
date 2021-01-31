using System;


namespace linway_app
{
    [Serializable()]

    public class DetalleRecibo
    {
        public string Detalle { get; set; }
        public float Importe { get; set; }

        public DetalleRecibo(string det, float imp)
        {
            Detalle = det;
            Importe = imp;
        }
    }
}
