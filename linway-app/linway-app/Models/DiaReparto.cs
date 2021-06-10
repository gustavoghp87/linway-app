using linway_app.Models.Interfaces;
using linway_app.Models.OModel;
using System.Collections.Generic;

namespace linway_app.Models
{
    public partial class DiaReparto : ObjModel, IDiaReparto
    {
        public DiaReparto()
        {
            Reparto = new HashSet<Reparto>();
        }
        public string Dia { get; set; }
        public virtual ICollection<Reparto> Reparto { get; set; }
    }
}
