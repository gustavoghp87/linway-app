using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;


namespace linway_app
{
    public enum TipoR { Inscripto, Monotributo };

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

        public Cliente(int num, string dire, int cod, int tel, string nom, string cui, TipoR tip)
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
}
