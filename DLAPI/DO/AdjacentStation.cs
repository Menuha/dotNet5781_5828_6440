﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
    public class AdjacentStation
    {
        public int Station1Code { get; set; }
        public int Station2Code { get; set; }
        public double Distance { get; set; }
        public TimeSpan Time { get; set; }
    }
}
