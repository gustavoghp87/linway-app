using System;
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
            Nombre = "producto";
            Precio = 0;
        }

        public ProductoL(int cod, string nom, float pre)
        {
            Codigo = cod;
            Nombre = nom;
            Precio = pre;
        }

        void cargarDatos(string nom, float pre)
        {
            Nombre = nom;
            Precio = pre;
        }

        void modNombre(string nom)
        {
            Nombre = nom;
        }

        void modPrecio(float costo)
        {
            Precio = costo;
        }

        string darNombre()
        {
            return Nombre;
        }

        float darPrecio()
        {
            return Precio;
        }

        public virtual bool MismoProductoQue(ProductoL otroProducto)
        {
            return false;
        }

        public virtual void DarTipoDeProducto(Enum unEnum)
        {
        }

        public virtual Enum ObtenerTipoDeProducto()
        {
            return null;
        }
    }

    public enum TipoLiquido { Suavizante, Desmanchador, JabonLiquido, Auxiliar, Perfume };

    [Serializable()]
    public class Liquido : ProductoL
    {
        private TipoLiquido tipoDeLiquido;

        public override bool MismoProductoQue(ProductoL otroProducto)
        {
            return false;
        }

        public override void DarTipoDeProducto(Enum unEnum)
        {
            tipoDeLiquido = (TipoLiquido) unEnum;
        }

        public override Enum ObtenerTipoDeProducto()
        {
            return tipoDeLiquido;
        }
    }

    public enum TipoPolvo { Texapol, Alison, AlisonEspecial, Dispersan, Eslabon, Blanqueador };

    [Serializable()]
    public class Polvo : ProductoL
    {
        private TipoPolvo tipoDePolvo;

        public override bool MismoProductoQue(ProductoL otroProducto)
        {
            return false;
        }

        public override void DarTipoDeProducto(Enum unEnum)
        {
            tipoDePolvo = (TipoPolvo) unEnum;
        }

        public override Enum ObtenerTipoDeProducto()
        {
            return tipoDePolvo;
        }
    }

    [Serializable()]
    public class Unidades : ProductoL
    {
        public override bool MismoProductoQue(ProductoL otroProducto)
        {
            return false;
        }

        public override void DarTipoDeProducto(Enum unEnum)
        {
        }
        public override Enum ObtenerTipoDeProducto()
        {
            return null;
        }
    }

    [Serializable()]
    public class Otros : ProductoL
    {
        public override bool MismoProductoQue(ProductoL otroProducto)
        {
            return false;
        }

        public override void DarTipoDeProducto(Enum unEnum)
        {
        }

        public override Enum ObtenerTipoDeProducto()
        {
            return null;
        }
    }
}
