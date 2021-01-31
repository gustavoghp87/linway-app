using System;
using System.ComponentModel;


namespace linway_app
{
    [Serializable()]

    public class ProdVendido
    {
        public int Cantidad { get; set; }
        [DisplayName("Detalle")]
        public string Descripcion { get; set; }
        public float Subtotal { get; set; }

        public ProdVendido(string desc, int cant, float prec)
        {
            Descripcion = desc;
            Cantidad = cant;
            Subtotal = prec;
        }

        public ProdVendido()
        {

        }

        public void CargarPV(string desc, int cant, float prec)
        {
            Descripcion = desc;
            Cantidad = cant;
            Subtotal = prec;
        }
    }
}
