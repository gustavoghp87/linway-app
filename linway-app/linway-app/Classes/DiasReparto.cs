using System;
using System.Collections.Generic;


namespace linway_app
{
    [Serializable()]

    public class DiasReparto
    {
        public string Dia { get; set; }
        public List<Reparto> Reparto = new List<Reparto>();

        public DiasReparto(string _dia)
        {
            Dia = _dia;
        }

        public void AgregarReparto(Reparto rep)
        {
            Reparto.Add(rep);
        }
    }
}
