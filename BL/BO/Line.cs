﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class Line
    {
        public int Id { get; set; }
        public int Code { get; set; }
        public Areas Area { get; set; }
        public IEnumerable<StationOfLine> StationsInLine { get; set; }
        public override string ToString() => this.ToStringProperty();
    }
}