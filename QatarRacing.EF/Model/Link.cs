using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QatarRacing.EF.Model
{
    public class Link
    {
        public int LinkID { get; set; }
        public string URL { get; set; }

        //public int? RaceID { get; set; }
        public virtual ICollection<Race> Races { get; set; }

    }
}
