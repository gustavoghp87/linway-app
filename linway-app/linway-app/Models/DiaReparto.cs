using linway_app.Models.Interfaces;
using linway_app.Models.OModel;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace linway_app.Models
{
    public partial class DiaReparto : ObjModel
    {
        public DiaReparto()
        {
            Reparto = new HashSet<Reparto>();
        }

        public long Id { get; set; }
        public string Dia { get; set; }
        public string Estado { get; set; }
        public virtual ICollection<Reparto> Reparto { get; set; }
    }
}
