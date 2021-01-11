using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
    public class Trip
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public int LineId { get; set; }
        public int InStationCode { get; set; }
        public TimeSpan InAt { get; set; }
        public int OutStationCode { get; set; }
        public TimeSpan OutAt { get; set; }
    }
}
