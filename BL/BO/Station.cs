using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    /// <summary>
    /// BO entity with information on physical station.
    /// </summary>
    public class Station
    {
        public int Code { get; set; }
        public string Name { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }
        public IEnumerable<LineOfStation> LinesInStation { get; set; }
        public override string ToString() => this.ToStringProperty();
    }
}
