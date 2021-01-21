using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace WindowsFormsApplication1
{
    public enum TipoR {Inscripto,Monotributo};
    [Serializable()]
    public class Cliente 
    {
        [DisplayName("Cod:")]
        public int Numero { get; set; }
        [DisplayName("Dirección - Localidad:")]
        public string Direccion { get; set; }
        [DisplayName("CP")]
        public int CodigoPostal { get; set; }
        [DisplayName("Telefono")]
        public int Telefono { get; set; }
        [DisplayName("Nombre y Apellido")]
        public string Nombre { get; set; }
        public string CUIT { get; set; }
        [DisplayName("Tipo R.")]
        public TipoR Tipo { get; set; }

        public Cliente()
        {
        }
        public Cliente(int num,string dire, int cod, int tel, string nom, string cui, TipoR tip)
        {
            this.Numero = num;
            this.Direccion = dire;
            this.CodigoPostal = cod;
            this.Telefono = tel;
            this.Nombre = nom;
            this.CUIT = cui;
            this.Tipo = tip;
        }
        /////////////////Cargar todos los datos sin constructor
        public void CargarDatos(string dire, int cod, int tel, string nom, string cui, TipoR tip)
        {
            this.Direccion = dire;
            this.CodigoPostal = cod;
            this.Telefono = tel;
            this.Nombre = nom;
            this.CUIT = cui;
            this.Tipo = tip;
        }
        
        ///////////////////Modificar////////////////////////
        
        public void modDireccion(string dire)
        {
            this.Direccion = dire;
        }
        public void modCodigo(int codig)
        {
            this.CodigoPostal = codig;
        }
        public void modTelefono(int tel)
        {
            this.Telefono = tel;
        }
        public void modNombre(string nom)
        {
            this.Nombre = nom;
        }
        public void modCuit(string cui)
        {
            this.CUIT = cui;
        }
        public void modTipo(TipoR tip)
        {
            this.Tipo = tip;
        }
        /////////////////////Mostrar/////////////////////////

        public int darCodigoPostal()
        {
            return this.CodigoPostal;
        }
        public int darTelefono()
        {
            return this.Telefono;
        }
        public string darDireccion()
        {
            return this.Direccion;
        }
        public string darNombre()
        {
            return this.Nombre;
        }
        public string darCUIT()
        {
            return this.CUIT;
        }
        public TipoR darTipo()
        {
            return this.Tipo;
        }
        ///////////////////////////////////////////////////////

    }


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
        /////////////Cargar todos datos///////////////////
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
        //////////////////////////////////////////////////
    }

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


    [Serializable()]
    public class NotaDeEnvio
    {
       [DisplayName("N°:")]
       public int Codigo { get; set; }
       public String Fecha { get; set; }
       public string Cliente { get; set; }
       public List<ProdVendido> Productos = new List<ProdVendido>();
       //public ProdVendido[] Productos = new ProdVendido[12] ;
       public string Detalle { get; set; }
        [DisplayName("Total")]
       public float ImporteTotal { get; set; }
       public bool Impresa { get; set; }

       public NotaDeEnvio(int cod, string clie, List<ProdVendido> listaP,bool siono)
       {
           this.Codigo = cod;
           this.Fecha = DateTime.Today.ToString().Substring(0, 10);
           this.Cliente = clie;
           this.Productos.AddRange(listaP);
           float subTo = 0;
           string deta = "";
           foreach (ProdVendido vActual in listaP)
           {
               subTo += vActual.Subtotal;
               deta =deta + vActual.Cantidad.ToString() + "x "+ vActual.Descripcion+". ";
           }
           this.ImporteTotal = subTo;
           this.Detalle = deta;
           this.Impresa = siono;
       }

       public void Modificar(List<ProdVendido> listaP)
       {
           
           this.Productos = listaP;
           float subTo = 0;
           string deta = "";
           foreach (ProdVendido vActual in listaP)
           {
               subTo += vActual.Subtotal;
               deta = deta + vActual.Cantidad.ToString() + "x " + vActual.Descripcion + ". ";
           }
           this.ImporteTotal = subTo;
           this.Detalle = deta;
           this.Impresa = false;
       }

    }

    [Serializable()]
    public class Venta
    {
        public string Producto { get; set; }
        public int Cantidad {get;set;}

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


    [Serializable()]
    public class detalleRecibo
    {
        public string detalle { get; set; }
        public float importe { get; set; }

        public detalleRecibo(string det, float imp)
        {
            this.detalle = det;
            this.importe = imp;
        }
    }


    [Serializable()]
    public class Recibo
    {
        [DisplayName("N°:")]
        public int Codigo { get; set; }
        public String Fecha { get; set; }
        public string Cliente { get; set; }
        [DisplayName("Total")]
        public float ImporteTotal { get; set; }
        public bool Impresa { get; set; }
        public List<detalleRecibo> detalle = new List<detalleRecibo>();

        public Recibo(int cod, string clie, List<detalleRecibo> listaD)
        {
            this.Codigo = cod;
            this.Fecha = DateTime.Today.ToString().Substring(0, 10);
            this.Cliente = clie;
            float subTo = 0;
            foreach (detalleRecibo dActual in listaD)
            {
                subTo += dActual.importe;
            }
            this.detalle.AddRange(listaD);
            this.ImporteTotal = subTo;
            this.Impresa = false;
        }
    }
}
