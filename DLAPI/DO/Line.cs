using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
    /// <summary>
    /// DO entity with information about a bus line.
    /// </summary>
    public class Line
    {
        public int ID { get; set; } //Key - automatic number.
        public int Number { get; set; } //Part1 of the key
        public Areas Area { get; set; } //Part2 of the key
        public int FirstStationCode { get; set; }
        public int LastStationCode { get; set; }
        public override string ToString() => this.ToStringProperty();
    }
}
