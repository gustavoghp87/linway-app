using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;


namespace linway_app
{
    [Serializable()]

    public class NotaDeEnvio
    {
        [DisplayName("N°:")]
        public int Codigo { get; set; }
        public string Fecha { get; set; }
        public string Cliente { get; set; }
        public List<ProdVendido> Productos = new List<ProdVendido>();
        //public ProdVendido[] Productos = new ProdVendido[12];
        public string Detalle { get; set; }
        [DisplayName("Total")]
        public float ImporteTotal { get; set; }
        public bool Impresa { get; set; }

        public NotaDeEnvio(int cod, string clie, List<ProdVendido> listaP, bool siono)
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
                deta = deta + vActual.Cantidad.ToString() + "x " + vActual.Descripcion + ". ";
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
}
