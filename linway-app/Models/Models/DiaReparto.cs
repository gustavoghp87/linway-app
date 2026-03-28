using Models.OModel;
using System.Collections.Generic;

namespace Models
{
    public partial class DiaReparto : ObjModel
    {
        public DiaReparto()
        {
            Repartos = new HashSet<Reparto>();
        }
        public string Dia { get; set; }
        public virtual ICollection<Reparto> Repartos { get; set; }
    }
}
