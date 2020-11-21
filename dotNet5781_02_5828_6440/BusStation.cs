//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Security.Cryptography;
//using System.Text;
//using System.Threading.Tasks;

//namespace dotNet5781_02_5828_6440
//{
//    public class BusStation
//    {
//        public int busStationKey;
//        public float latitude;
//        public float longitude;

//        public BusStation(int buskode, float latitude, float longitude)
//        {
//            this.busStationKey = buskode;
//            this.latitude = latitude;
//            this.longitude = longitude;
//        }
//        public int BusStationKey
//        {
//            get
//            {
//                return busStationKey;
//            }
//            set
//            {
//                busStationKey = value;
//            }
//        }
//        public float Latitude
//        {
//            get 
//            {
//                return latitude;
//            }
//            set
//            {
//                Random r1 = new random();
//                latitude = r1.Next(310, 333);
//                //latitude = latitude / 10;
//            }
//        }
//        public float Longitude
//        {
//            get 
//            {
//                return longitude;
//            }
//            set 
//            {
//                Random r2 = new random();
//                longitude = r2.Next(343, 355);
//                //longitude = longitude / 10;
//            }
//        }
//        public override string ToString()
//        {
//            return "Bus Station Code: " + busStationKey + ", " + latitude + "°N ," + longitude + "°E";
//        }
//    }

//}
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
            set
            {
                if (value.Length != 6)
                {
                    //חריגה
                    Console.WriteLine("WRONG BUS STATION CODE");
                    return;
                }
                stationCode = value;
            }
        }

        public double Latitude
        {
            get
            {
                return latitude;
            }
            set
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

        public double Longitude
        {
            get
            {
                return longitude;
            }
            set
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
