using System;


namespace linway_app
{
    [Serializable()]

    public class Venta
    {
        public string Producto { get; set; }
        public int Cantidad { get; set; }

        public Venta(string prod)
        {
            Producto = prod;
            Cantidad = 0;
        }

        public void RealizarVenta(int cant)
        {
            Cantidad += cant;
        }
    }
}
