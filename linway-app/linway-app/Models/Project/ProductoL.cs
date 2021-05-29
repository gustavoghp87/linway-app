using linway_app.Models.Enums;
using System;
using System.ComponentModel;

namespace linway_app.Models
{                                                                            // clases no usadas
    [Serializable()]
    public class ProductoL
    {
        [DisplayName("Cod:")]
        public int Id { get; private set; }

        [DisplayName("Producto")]
        public string Nombre { get; set; }

        [DisplayName("Precio($)")]
        public float Precio { get; set; }

        public ProductoL()
        {
            Nombre = "producto";
            Precio = 0;
        }

        public ProductoL(int id, string nom, float pre)
        {
            Id = id;
            Nombre = nom;
            Precio = pre;
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
            tipoDeLiquido = (TipoLiquido)unEnum;
        }

        public override Enum ObtenerTipoDeProducto()
        {
            return tipoDeLiquido;
        }
    }

    

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
