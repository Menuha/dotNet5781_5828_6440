using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
    /// <summary>
    /// DO entity with information on physical station.
    /// </summary>
    public class Station
    {
        public int Code { get; set; } //Key
        public string Name { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }
        public override string ToString() => this.ToStringProperty();
    }
}
