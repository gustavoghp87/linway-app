using System;
using System.Collections.Generic;
using System.Windows.Forms;


namespace linway_app
{
    [Serializable()]

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
            Nombre = _name;
            TA = 0;
            TE = 0;
            TD = 0;
            TT = 0;
            TAE = 0;
            TotalB = 0;
            TL = 0;
            Destinos = new List<Destino>();
        }

        public void LimpiarDatos()
        {
            TA = 0;
            TE = 0;
            TD = 0;
            TT = 0;
            TAE = 0;
            TotalB = 0;
            TL = 0;
            foreach (Destino dActual in Destinos)
            {
                dActual.Limpiar();
            }
        }

        public void AgregarDestino(Destino dest)
        {
            Destinos.Add(dest);
        }

        public void CargarPorVenta(List<Venta> lVenta, string dire)
        {
            if (!Destinos.Exists(x => x.Direccion == dire))
            {
                Destinos.Add(new Destino(dire));
            }
            Destinos.Find(x => x.Direccion == dire).ModificarPorV(lVenta);
            foreach (Venta vActual in lVenta)
            {
                ModificarContadores(vActual.Cantidad, vActual.Producto);
            }
        }

        public void CargarPorNota(List<ProdVendido> lVenta, string dire)
        {
            if (!Destinos.Exists(x => x.Direccion == dire))
            {
                Destinos.Add(new Destino(dire));
            }
            try
            {
                Destinos.Find(x => x.Direccion == dire).ModificarPorN(lVenta);
                foreach (ProdVendido pvActual in lVenta)
                {
                    ModificarContadores(pvActual.Cantidad, pvActual.Descripcion);
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show("Falla en CargarPorNota: " + exc);
            }
        }

        private void ModificarContadores(int cant, string producto)
        {
            if (EsPolvo(producto))
            {
                SumarPolvo(cant, producto);
            }
            if ((!EsPolvo(producto)) && (!EsOtro(producto)))
            {
                SumarLiquido(cant);
            }
        }

        private bool EsPolvo(string cadena)
        {
            bool es = false;
            if ((cadena.Contains("pol - p")) || (cadena.Contains("san - p")) || (cadena.Contains("bón - p")) || (cadena.Contains("son - p")) || (cadena.Contains("ial - p")))
            {
                es = true;
            }
            return es;
        }

        private void SumarPolvo(int c, string cadena)
        {
            if (cadena.Contains("pol - p"))
            {
                TT += (c / 20);
            }
            if (cadena.Contains("san - p"))
            {
                TD += (c / 20);
            }
            if (cadena.Contains("bón - p"))
            {
                TE += (c / 20);
            }
            if (cadena.Contains("son - p"))
            {
                TA += (c / 20);
            }
            if (cadena.Contains("ial - p"))
            {
                TAE += (c / 20);
            }
            TotalB = TT + TD + TE + TA + TAE;
        }

        private bool EsOtro(string cadena)
        {
            bool es = false;
            if ((cadena.Contains("olsas")) || (cadena.Contains("cobrar")) || (cadena.Contains("Tal/")) || (cadena.Contains("queador")) || (cadena.Contains("BONIFI")) || (cadena.Contains("pendiente")) || (cadena.Contains("actura")) || (cadena.Contains("favor")) || (cadena.Contains("escuento")))
            {
                es = true;
            }
            return es;
        }

        private void SumarLiquido(int c)
        {
            TL += c;
        }
    }
}
