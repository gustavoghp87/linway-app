using Models.OModel;
using System.Collections.Generic;

namespace Models
{
    public partial class DiaReparto : ObjModel
    {
        public DiaReparto()
        {
            Reparto = new HashSet<Reparto>();
        }
        public string Dia { get; set; }
        public virtual ICollection<Reparto> Reparto { get; set; }
    }
}
