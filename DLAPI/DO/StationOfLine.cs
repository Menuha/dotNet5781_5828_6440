using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
    public class StationOfLine
    {
        public int LineId { get; set; }
        public int StationCode { get; set; }
        public int StationIndexInLine { get; set; }
        public int PrevStationCode { get; set; }
        public int NextStationCode { get; set; }
        public override string ToString() => this.ToStringProperty();
    }
}
