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
        private double distanceFromPre;
        private TimeSpan travelTimeFromPre;

        /// <summary>
        /// Constractor of a bus line station
        /// </summary>
        /// <param name="station">Data of a bus line station</param>
        /// <param name="distanceFromPre">Distance from the previous station</param>
        /// <param name="travelTimeFromPre">Travel time from previous station</param>
        public BusLineStation(BusStation station, double distanceFromPre, TimeSpan travelTimeFromPre)
        {
            Station = station;
            DistanceFromPre = distanceFromPre;
            TravelTimeFromPre = travelTimeFromPre;
        }

        /// <summary>
        /// Data of a bus line station 
        /// </summary>
        public BusStation Station 
        { get => station; private set => station = value; }

        /// <summary>
        /// Distance from the previous station
        /// </summary>
        public double DistanceFromPre
        {
            get { return distanceFromPre; }
            set { distanceFromPre = value; }
        }

        /// <summary>
        /// Travel time from previous station
        /// </summary>
        public TimeSpan TravelTimeFromPre
        {
            get { return travelTimeFromPre; }
            set { travelTimeFromPre = value; }
        }
    }
}
