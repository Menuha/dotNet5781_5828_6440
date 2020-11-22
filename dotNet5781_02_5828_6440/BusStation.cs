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
        private const double minLatitude = 31;
        private const double maxLatitude = 33.3;
        private const double minLongitude = 34.3;
        private const double maxLongitude = 31.5;

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

        /// <summary>
        /// The station code
        /// </summary>
        public string StationCode
        {
            get
            {
                return stationCode;
            }
            private set
            {
                if (value.Length <= 6)
                {
                    stationCode = value;
                }
                else
                {
                    throw new ArgumentException("WRONG BUS STATION CODE");
                }
            }
        }

        /// <summary>
        /// The station latitude
        /// </summary>
        public double Latitude
        {
            get 
            {
                return latitude;
            }
            private set
            {
                if (value < 31 || value > 33.3)
                {
                    //A random number in the width area of the State of Israel
                    Random random = new Random(DateTime.Now.Millisecond);
                    latitude = random.NextDouble() * (maxLatitude - minLatitude) + minLatitude;
                    return;
                }
                latitude = value;
            }
        }

        /// <summary>
        /// The station longitude
        /// </summary>
        public double Longitude
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


        /// <summary>
        /// Calculate air distance between 2 stations
        /// </summary>
        /// <param name="station1">station number 1</param>
        /// <param name="station2">station number 2</param>
        /// <returns>Air distance betweeen 2 stations</returns>
        public static double Gap2S(BusStation station1, BusStation station2)
        {
            //חריגות
            return Math.Sqrt(Math.Pow(station1.Latitude - station2.Latitude, 2) + Math.Pow(station1.Longitude - station2.Longitude, 2));
        }
    }
}