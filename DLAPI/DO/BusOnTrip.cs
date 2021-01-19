using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
    /// <summary>
    /// Bonus. DO entity with information about a traveling bus.
    /// </summary>
    public class BusOnTrip
    {
        public int ID { get; set; } //Key - Auto-runner number.
        public int LicenseNum { get; set; } //Part1 of the key
        public int LineID { get; set; } //Part2 of the key
        public TimeSpan PlannedTakeOff { get; set; } //Part3 of the key
        public TimeSpan ActualTakeOff { get; set; }
        public int PrevStationCode { get; set; }
        public TimeSpan PrevStationAt { get; set; }
        public TimeSpan NextStationAt { get; set; }
        public override string ToString() => this.ToStringProperty();
    }
}
