using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;


namespace linway_app
{
    [Serializable()]

    public class Producto
    {
        [DisplayName("Cod:")]
        public int Codigo { get; set; }
        [DisplayName("Producto")]
        public string Nombre { get; set; }
        [DisplayName("Precio($)")]
        public float Precio { get; set; }

        public Producto()
        {
            Nombre = "producto";
            Precio = 0;
        }

        public Producto(int cod, string nom, float pre)
        {
            Codigo = cod;
            Nombre = nom;
            Precio = pre;
        }

        void CargarDatos(string nom, float pre)
        {
            Nombre = nom;
            Precio = pre;
        }

        void ModNombre(string nom)
        {
            Nombre = nom;
        }

        void ModPrecio(float costo)
        {
            Precio = costo;
        }

        string DarNombre()
        {
            return Nombre;
        }

        float DarPrecio()
        {
            return Precio;
        }
    }
}
