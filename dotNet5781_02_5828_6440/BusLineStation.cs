using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotNet5781_02_5828_6440
{
    /// <summary>
    /// Department for a bus line station representation
    /// </summary>
    class BusLineStation
    {
        private BusStation station;
        private double distanceFPre;
        private TimeSpan travelTimeFPre;
        private int locationRoute;

        /// <summary>
        /// Constractor of a bus line station
        /// </summary>
        /// <param name="station">Data of a bus line station</param>
        /// <param name="distanceFPre">Distance from the previous station</param>
        /// <param name="travelTimeFPre">Travel time from previous station</param>
        public BusLineStation(BusStation station, double distanceFPre, TimeSpan travelTimeFPre)
        {
            Station = station;
            DistanceFPre = distanceFPre;
            TravelTimeFPre = travelTimeFPre;
        }

        /// <summary>
        /// Data of a bus line station 
        /// </summary>
        public BusStation Station 
        { get => station; private set => station = value; }

        /// <summary>
        /// Distance from the previous station
        /// </summary>
        public double DistanceFPre
        {
            get { return distanceFPre; }
            private set { distanceFPre = value; }
        }

        /// <summary>
        /// Travel time from previous station
        /// </summary>
        public TimeSpan TravelTimeFPre
        {
            get { return travelTimeFPre; }
            private set { travelTimeFPre = value; }
        }

        public int LocationRoute { get => locationRoute; set => locationRoute = value; }
    }
}
