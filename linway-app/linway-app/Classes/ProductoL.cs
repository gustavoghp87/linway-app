using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;


namespace linway_app
{
    [Serializable()]

    public class ProductoL
    {
        [DisplayName("Cod:")]
        public int Codigo { get; set; }
        [DisplayName("Producto")]
        public string Nombre { get; set; }
        [DisplayName("Precio($)")]
        public float Precio { get; set; }

        public ProductoL()
        {
            this.Nombre = "producto";
            this.Precio = 0;
        }
        public ProductoL(int cod, string nom, float pre)
        {
            this.Codigo = cod;
            this.Nombre = nom;
            this.Precio = pre;
        }

        void cargarDatos(string nom, float pre)
        {
            this.Nombre = nom;
            this.Precio = pre;
        }

        //////////Modificar datos////////////////////
        void modNombre(string nom)
        {
            this.Nombre = nom;
        }

        void modPrecio(float costo)
        {
            this.Precio = costo;
        }

        /////////obtener Datos ////////////////////////
        string darNombre()
        {
            return this.Nombre;
        }

        float darPrecio()
        {
            return this.Precio;
        }

        public virtual bool mismoProductoQue(ProductoL otroProducto)
        {
            return false;
        }

        public virtual void darTipoDeProducto(Enum unEnum)
        {
        }

        public virtual Enum obtenerTipoDeProducto()
        {
            return null;
        }
    }

    public enum TipoLiquido { Suavizante, Desmanchador, JabonLiquido, Auxiliar, Perfume };

    [Serializable()]

    public class Liquido : ProductoL
    {
        private TipoLiquido tipoDeLiquido;

        public override bool mismoProductoQue(ProductoL otroProducto)
        {
            return false;
        }

        public override void darTipoDeProducto(Enum unEnum)
        {
            tipoDeLiquido = (TipoLiquido)unEnum;
        }

        public override Enum obtenerTipoDeProducto()
        {
            return this.tipoDeLiquido;
        }

    }

    public enum TipoPolvo { Texapol, Alison, AlisonEspecial, Dispersan, Eslabon, Blanqueador };

    [Serializable()]
    public class Polvo : ProductoL
    {
        private TipoPolvo tipoDePolvo;

        public override bool mismoProductoQue(ProductoL otroProducto)
        {
            return false;
        }

        public override void darTipoDeProducto(Enum unEnum)
        {
            this.tipoDePolvo = (TipoPolvo)unEnum;
        }

        public override Enum obtenerTipoDeProducto()
        {
            return this.tipoDePolvo;
        }
    }

    [Serializable()]
    public class Unidades : ProductoL
    {
        public override bool mismoProductoQue(ProductoL otroProducto)
        {
            return false;
        }

        public override void darTipoDeProducto(Enum unEnum)
        {
        }
        public override Enum obtenerTipoDeProducto()
        {
            return null;
        }
    }

    [Serializable()]
    public class Otros : ProductoL
    {
        public override bool mismoProductoQue(ProductoL otroProducto)
        {
            return false;
        }

        public override void darTipoDeProducto(Enum unEnum)
        {
        }

        public override Enum obtenerTipoDeProducto()
        {
            return null;
        }
    }

}
