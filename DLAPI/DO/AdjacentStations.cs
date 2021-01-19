using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
    /// <summary>
    /// DO entity with information on adjacent stations
    /// </summary>
    public class AdjacentStations
    {
        public int Station1Code { get; set; } //Key1
        public int Station2Code { get; set; } //Key2
        public double Distance { get; set; }
        public TimeSpan AvgTime { get; set; }
        public override string ToString() => this.ToStringProperty();
    }
}
