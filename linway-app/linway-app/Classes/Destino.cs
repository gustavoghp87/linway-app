using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;


namespace linway_app
{
    [Serializable]

    public class Destino
    {
        public bool Entregar { get; set; }
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
            Direccion = cliente;
            Entregar = false;
            L = 0;
            Productos = "";
            A = 0;
            E = 0;
            D = 0;
            T = 0;
            AE = 0;
        }

        public void Limpiar()
        {
            Entregar = false;
            L = 0;
            Productos = "";
            A = 0;
            E = 0;
            D = 0;
            T = 0;
            AE = 0;
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
                sumarPolvo(cant, prod);
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
                    Productos = Productos + cant.ToString() + " " + prod.Substring(0, prod.IndexOf('(')) + ". ";
                }
                else
                {
                    if (prod.Contains("-"))
                    {
                        if (esPolvo(prod))
                        {
                            Productos = Productos + (cant / 20).ToString() + "x20 " + prod.Substring(0, prod.IndexOf('-')) + ". ";
                        }
                        else
                        {
                            Productos = Productos + cant.ToString() + " " + prod.Substring(0, prod.IndexOf('-')) + ". ";
                        }
                    }
                    else
                    {
                        Productos = Productos + cant.ToString() + " " + prod + ". ";
                    }
                }
            }
            Entregar = true;
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
                T += (c / 20);
            }
            if (cadena.Contains("san - p"))
            {
                D += (c / 20);
            }
            if (cadena.Contains("bón - p"))
            {
                E += (c / 20);
            }
            if (cadena.Contains("son - p"))
            {
                A += (c / 20);
            }
            if (cadena.Contains("ial - p"))
            {
                AE += (c / 20);
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
            if ((cadena.Contains("pendiente")) || (cadena.Contains("BONIFI")) || (cadena.Contains("actura")) || (cadena.Contains("favor")) || (cadena.Contains("escuento")))
            {
                es = true;
            }
            return es;
        }

        private void sumarLiquido(int c)
        {
            L += c;
        }
    }
}
