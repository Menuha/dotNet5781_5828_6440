using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    /// <summary>
    /// BO entity with information about a bus line.
    /// </summary>
    public class Line
    {
        public int ID { get; set; }
        public int Code { get; set; }
        public Areas Area { get; set; }
        public IEnumerable<StationOfLine> StationsInLine { get; set; }
        public override string ToString() => $"Code = {Code}, Area = {Area}";
    }
}