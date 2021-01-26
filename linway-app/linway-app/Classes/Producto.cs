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
            this.Nombre = "producto";
            this.Precio = 0;
        }

        public Producto(int cod, string nom, float pre)
        {
            this.Codigo = cod;
            this.Nombre = nom;
            this.Precio = pre;
        }

        void CargarDatos(string nom, float pre)
        {
            this.Nombre = nom;
            this.Precio = pre;
        }

        void ModNombre(string nom)
        {
            this.Nombre = nom;
        }

        void ModPrecio(float costo)
        {
            this.Precio = costo;
        }

        string DarNombre()
        {
            return this.Nombre;
        }

        float DarPrecio()
        {
            return this.Precio;
        }
    }
}
