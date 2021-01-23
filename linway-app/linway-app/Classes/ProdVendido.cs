using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            this.Descripcion = desc;
            this.Cantidad = cant;
            this.Subtotal = prec;
        }

        public ProdVendido()
        {

        }

        public void cargarPV(string desc, int cant, float prec)
        {
            this.Descripcion = desc;
            this.Cantidad = cant;
            this.Subtotal = prec;
        }
    }
}
