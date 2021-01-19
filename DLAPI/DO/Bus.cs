using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
    /// <summary>
    /// Bonus. DO entity with bus information.
    /// </summary>
    public class Bus
    {
        public int LicenseNum { get; set; } //Key
        public DateTime FromDate { get; set; }
        public float TotalTrip { get; set; }
        public float FuelRemain { get; set; }
        public BusStatus Status { get; set; }
        public override string ToString() => this.ToStringProperty();
    }
}
