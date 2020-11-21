using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace dotNet5781_02_5828_6440
{
    /// <summary>
    /// A class that will represent a bus station
    /// </summary>
    public class BusStation
    {
        public const double minLatitude = 31;
        public const double maxLatitude = 33.3;
        public const double minLongitude = 34.3;
        public const double maxLongitude = 31.5;

        /// <summary>
        /// The code of this station
        /// </summary>
        private string stationCode = null;
        /// <summary>
        /// latitude = The latitude where the station is located
        /// </summary>
        private double latitude;
        /// <summary>
        /// longitude = The longitude where the station is located
        /// </summary>
        private double longitude;

        /// <summary>
        /// BusStation constructor with 3/2/1 parameters
        /// </summary>
        /// <param name="stationCode">The station code</param>
        /// <param name="latitude">Station latitude</param>
        /// <param name="longitude">Station longtitude</param>
        public BusStation(string stationCode, double latitude = 0, double longitude = 0)
        {
            StationCode = stationCode;
            Latitude = latitude;
            Longitude = longitude;
        }

        public string StationCode
        {
            get
            {
                return stationCode;
            }
            private set
            {
                busStationKey = value;
            }
        }
        public float Latitude
        {
            get
            {
                return latitude;
            }
            private set
            {
                Random r1 = new random();
                latitude = r1.Next(310, 333);
                //latitude = latitude / 10;
            }
        }
        public float Longitude
        {
            get
            {
                return longitude;
            }
            private set
            {
                if (value < 34.3 || value > 35.5)
                {
                    //A random number in the longitudinal area of the State of Israel
                    Random random = new Random(DateTime.Now.Millisecond);
                    longitude = random.NextDouble() * (maxLongitude - minLongitude) + minLongitude;
                    return;
                }
                longitude = value;
            }
        }

        /// <summary>
        /// A method that displays the station parameters
        /// </summary>
        /// <returns>Station parameters</returns>
        public override string ToString()
        {
            return "Bus Station Code: " + stationCode + ", " + latitude + "°N " + longitude + "°E";
        }
    }
}
