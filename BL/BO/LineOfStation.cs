using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    /// <summary>
    /// BO entity with information about a line passing through a particular station.
    /// </summary>
    public class LineOfStation
    {
        public int ID { get; set; }
        public int Number { get; set; }
        public Areas Area { get; set; }
        public int StationIndexInLine { get; set; }
        public override string ToString() => $"Line number: {Number}, Area: {Area}, Station index in line: {StationIndexInLine}.";
    }
}
