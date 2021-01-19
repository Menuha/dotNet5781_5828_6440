using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
    /// <summary>
    /// DO entity that describes the departure of a bus line for travel. 
    /// Each line can have several departures at different hours.
    /// </summary>
    public class LineTrip
    {
        public int LineID { get; set; } //Part1 of the key
        public TimeSpan StartAt { get; set; } //Bonus. Part2 of the key
        public TimeSpan Frequency { get; set; } //Bonus
        public TimeSpan FinishAt { get; set; } //Bonus
        public override string ToString() => this.ToStringProperty();
    }
}
