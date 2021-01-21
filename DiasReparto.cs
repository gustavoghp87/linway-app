using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace WindowsFormsApplication1
{
    [Serializable]
    public class DiasReparto
    {
        public string Dia { get; set; }
        public List<Reparto> Reparto = new List<Reparto>();

        public DiasReparto(string _dia)
        {
            this.Dia = _dia;
        }
        public void agregarReparto(Reparto rep)
        {
            this.Reparto.Add(rep);
        }
        
    }



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

        private void modificarContadores(int cant , string producto)
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


    [Serializable]
    public class Destino
    {
        public bool Entregar {get;set;}
        public string Direccion { get; set; }
        public int L { get; set; }
        public string Productos { get; set; }
        public int A { get; set; }
        public int E { get; set; }
        public int D { get; set; }
        public int T { get; set; }
        public int AE { get; set; }

        public Destino(string cliente)
        {
            this.Direccion = cliente;
            this.Entregar = false;
            this.L = 0;
            this.Productos = "";
            this.A = 0;
            this.E = 0;
            this.D = 0;
            this.T = 0;
            this.AE = 0;
        }

        public void Limpiar()
        {
            this.Entregar = false;
            this.L = 0;
            this.Productos = "";
            this.A = 0;
            this.E = 0;
            this.D = 0;
            this.T = 0;
            this.AE = 0;
        }

        public void ModificarPorV(List<Venta> ventasL)
        {
            foreach (Venta vActual in ventasL)
            {
                ModificarRenglon(vActual.Cantidad, vActual.Producto);
            }
        }

        public void ModificarPorN(List<ProdVendido> ventasL)
        {
            foreach (ProdVendido pvActual in ventasL)
            {
                ModificarRenglon(pvActual.Cantidad, pvActual.Descripcion);
            }
        }

        private void ModificarRenglon(int cant, string prod)
        {
            if (esPolvo(prod))
            {
                sumarPolvo(cant ,prod);
            }
            if ((!esPolvo(prod)) && (!esOtro(prod)) && !(prod.Contains("cobrar")))
            {
                sumarLiquido(cant);
            }
            if (!esProduco(prod))
            {
                //Aca agregar para cada producto ( si es polvo, liquido, bolsas o talonario con funciones bool)
                if (prod.Contains("olsas"))
                {
                    this.Productos = this.Productos + cant.ToString() + " " + prod.Substring(0, prod.IndexOf('(')) + ". ";
                }
                else
                {
                    if (prod.Contains("-"))
                    {
                        if(esPolvo(prod))
                        {
                            this.Productos = this.Productos + (cant / 20).ToString() + "x20 " + prod.Substring(0, prod.IndexOf('-')) + ". ";
                        }
                        else
                        {
                             this.Productos = this.Productos + cant.ToString() + " " + prod.Substring(0, prod.IndexOf('-')) + ". ";
                        }
                    }
                    else
                    {
                        this.Productos = this.Productos + cant.ToString() + " " + prod + ". ";
                    }
                }
            }
            this.Entregar = true;
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
                this.T += (c / 20);
            }
            if (cadena.Contains("san - p"))
            {
                this.D += (c / 20);
            }
            if (cadena.Contains("bón - p"))
            {
                this.E += (c / 20);
            }
            if (cadena.Contains("son - p"))
            {
                this.A += (c / 20);
            }
            if (cadena.Contains("ial - p"))
            {
                this.AE += (c / 20);
            }
        }
        private bool esOtro(string cadena)
        {
            bool es = false;
            if ((cadena.Contains("olsas")) || (cadena.Contains("BONIFI")) || (cadena.Contains("Tal/")) || (cadena.Contains("queador")) || (cadena.Contains("pendiente")) || (cadena.Contains("actura")) || (cadena.Contains("favor")) || (cadena.Contains("escuento")))
            {
                es = true;
            }
            return es;
        }
        private bool esProduco(string cadena)
        {
            bool es = false;
            if ((cadena.Contains("pendiente")) || (cadena.Contains("BONIFI")) ||(cadena.Contains("actura")) || (cadena.Contains("favor")) || (cadena.Contains("escuento")))
            {
                es = true;
            }
            return es;
        }
        private void sumarLiquido(int c)
        {
            this.L += c;
        }
    }




}
