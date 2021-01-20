using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    /// <summary>
    /// BO entity with information on adjacent stations
    /// </summary>
    public class AdjacentStations
    {
        public int Station1Code { get; set; }
        public int Station2Code { get; set; }
        public double Distance { get; set; }
        public TimeSpan AvgTime { get; set; }
        public override string ToString() => this.ToStringProperty();
    }
}
