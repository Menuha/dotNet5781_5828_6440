using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class AdjacentStations
    {
        public StationOfLine Station1 { get; set; }
        public StationOfLine Station2 { get; set; }
        public double Distance { get; set; }
        public TimeSpan Time { get; set; }
        public override string ToString() => this.ToStringProperty();
    }
}
