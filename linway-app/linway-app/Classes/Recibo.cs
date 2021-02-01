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
        public string Fecha { get; set; }
        public string Cliente { get; set; }
        [DisplayName("Total")]
        public float ImporteTotal { get; set; }
        public bool Impresa { get; set; }
        public List<DetalleRecibo> listaDetalles = new List<DetalleRecibo>();

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
            listaDetalles.AddRange(listaD);
            ImporteTotal = subTo;
            Impresa = false;
        }

        public Recibo(int cod, string clie, List<DetalleRecibo> listaD, string fecha, bool impresa)  // para importar
        {
            Codigo = cod;
            Fecha = fecha;
            Cliente = clie;
            float subTo = 0;
            foreach (DetalleRecibo dActual in listaD)
            {
                subTo += dActual.Importe;
            }
            listaDetalles.AddRange(listaD);
            ImporteTotal = subTo;
            Impresa = impresa;
        }
    }
}
