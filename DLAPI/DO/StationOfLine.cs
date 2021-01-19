using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
    /// <summary>
    /// DO entity with line station information.
    /// </summary>
    public class StationOfLine
    {
        public int LineID { get; set; } //Part1 of the key
        public int StationCode { get; set; } //Part2 of the key
        public int StationIndexInLine { get; set; }
        public override string ToString() => this.ToStringProperty();
    }
}
