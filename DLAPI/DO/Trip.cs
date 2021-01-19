using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
    /// <summary>
    /// Bonus. DO entity with user travel information.
    /// </summary>
    public class Trip
    {
        public int ID { get; set; } //Key - Auto-runner number.
        public string UserName { get; set; }
        public int LineID { get; set; }
        public int InStationCode { get; set; }
        public TimeSpan InAt { get; set; }
        public int OutStationCode { get; set; }
        public TimeSpan OutAt { get; set; }
    }
}
