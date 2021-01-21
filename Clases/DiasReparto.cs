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
}
