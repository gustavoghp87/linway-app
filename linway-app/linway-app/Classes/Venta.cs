using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;


namespace linway_app
{
    [Serializable()]

    public class Venta
    {
        public string Producto { get; set; }
        public int Cantidad { get; set; }

        public Venta(string prod)
        {
            this.Producto = prod;
            this.Cantidad = 0;
        }

        public void realizarVenta(int cant)
        {
            this.Cantidad += cant;
        }
    }
}
