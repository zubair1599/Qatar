using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QatarRacing.EF.Model
{
    public class Jockey
    {
        public int JockeyID { get; set; }
        public string Name { get; set; }
        public decimal Weight { get; set; }

        public virtual ICollection<Runner> Runners { get; set; }
    }
}
