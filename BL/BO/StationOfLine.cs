using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    /// <summary>
    /// BO entity with line station information.
    /// </summary>
    public class StationOfLine
    {
        public int StationCode { get; set; }
        public string StationName { get; set; }
        public int StationIndexInLine { get; set; }
        public double DistanceFromPre { get; set; }
        public TimeSpan TimeFromPre { get; set; }
        public override string ToString() => this.ToStringProperty();
    }
}
