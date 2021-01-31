using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;


namespace linway_app
{
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
        public List<DetalleRecibo> detalle = new List<DetalleRecibo>();

        public Recibo(int cod, string clie, List<DetalleRecibo> listaD)
        {
            Codigo = cod;
            Fecha = DateTime.Today.ToString().Substring(0, 10);
            Cliente = clie;
            float subTo = 0;
            foreach (DetalleRecibo dActual in listaD)
            {
                subTo += dActual.Importe;
            }
            detalle.AddRange(listaD);
            ImporteTotal = subTo;
            Impresa = false;
        }
    }
}
