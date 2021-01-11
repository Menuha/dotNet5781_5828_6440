﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
    public class Bus
    {
        public int LicenseNum { get; set; }
        public DateTime FromDate { get; set; }
        public float TotalTrip { get; set; }
        public float FuelRemain { get; set; }
        public BusStatus Status { get; set; }
        public override string ToString() => this.ToStringProperty();
    }
}
