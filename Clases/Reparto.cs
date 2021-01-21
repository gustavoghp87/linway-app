using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;


namespace WindowsFormsApplication1
{
    [Serializable]

    public class Reparto
    {
        public string Nombre { get; set; }
        public List<Destino> Destinos { get; set; }
        public int TA { get; set; }
        public int TE { get; set; }
        public int TD { get; set; }
        public int TT { get; set; }
        public int TAE { get; set; }
        public int TotalB { get; set; }
        public int TL { get; set; }

        public Reparto(string _name)
        {
            this.Nombre = _name;
            this.TA = 0;
            this.TE = 0;
            this.TD = 0;
            this.TT = 0;
            this.TAE = 0;
            this.TotalB = 0;
            this.TL = 0;
            Destinos = new List<Destino>();
        }

        public void LimpiarDatos()
        {
            this.TA = 0;
            this.TE = 0;
            this.TD = 0;
            this.TT = 0;
            this.TAE = 0;
            this.TotalB = 0;
            this.TL = 0;
            foreach (Destino dActual in Destinos)
            {
                dActual.Limpiar();
            }
        }

        public void agregarDestino(Destino dest)
        {
            this.Destinos.Add(dest);
        }

        public void cargarPorVenta(List<Venta> lVenta, string dire)
        {
            if (!Destinos.Exists(x => x.Direccion == dire))
            {
                Destinos.Add(new Destino(dire));
            }
            Destinos.Find(x => x.Direccion == dire).ModificarPorV(lVenta);
            foreach (Venta vActual in lVenta)
            {
                modificarContadores(vActual.Cantidad, vActual.Producto);
            }
        }

        public void cargarPorNota(List<ProdVendido> lVenta, string dire)
        {
            if (!Destinos.Exists(x => x.Direccion == dire))
            {
                Destinos.Add(new Destino(dire));
            }
            Destinos.Find(x => x.Direccion == dire).ModificarPorN(lVenta);
            foreach (ProdVendido pvActual in lVenta)
            {
                modificarContadores(pvActual.Cantidad, pvActual.Descripcion);
            }
        }

        private void modificarContadores(int cant, string producto)
        {
            if (esPolvo(producto))
            {
                sumarPolvo(cant, producto);
            }
            if ((!esPolvo(producto)) && (!esOtro(producto)))
            {
                sumarLiquido(cant);
            }
        }

        private bool esPolvo(string cadena)
        {
            bool es = false;
            if ((cadena.Contains("pol - p")) || (cadena.Contains("san - p")) || (cadena.Contains("bón - p")) || (cadena.Contains("son - p")) || (cadena.Contains("ial - p")))
            {
                es = true;
            }

            return es;
        }

        private void sumarPolvo(int c, string cadena)
        {
            if (cadena.Contains("pol - p"))
            {
                this.TT += (c / 20);
            }
            if (cadena.Contains("san - p"))
            {
                this.TD += (c / 20);
            }
            if (cadena.Contains("bón - p"))
            {
                this.TE += (c / 20);
            }
            if (cadena.Contains("son - p"))
            {
                this.TA += (c / 20);
            }
            if (cadena.Contains("ial - p"))
            {
                this.TAE += (c / 20);
            }
            this.TotalB = this.TT + this.TD + this.TE + this.TA + this.TAE;
        }

        private bool esOtro(string cadena)
        {
            bool es = false;
            if ((cadena.Contains("olsas")) || (cadena.Contains("cobrar")) || (cadena.Contains("Tal/")) || (cadena.Contains("queador")) || (cadena.Contains("BONIFI")) || (cadena.Contains("pendiente")) || (cadena.Contains("actura")) || (cadena.Contains("favor")) || (cadena.Contains("escuento")))
            {
                es = true;
            }
            return es;
        }

        private void sumarLiquido(int c)
        {
            this.TL += c;
        }
    }
}
