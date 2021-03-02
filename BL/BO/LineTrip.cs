﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class LineTrip
    {
        public int LineTripID { get; set; } //Key - automatic number.
        public int LineID { get; set; } //Part1 of the key
        public TimeSpan StartAt { get; set; } //Bonus. Part2 of the key
        //public TimeSpan Frequency { get; set; } //Bonus
        //public TimeSpan FinishAt { get; set; } //Bonus
        public override string ToString() => $"Line ID: {LineID}, Start At: {StartAt}";
    }
}

