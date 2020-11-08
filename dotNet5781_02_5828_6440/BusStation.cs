using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace dotNet5781_02_5828_6440
{
    class BusStation
    {
        public int busStationKey;
        public float latitude;
        public float longitude;

        public BusStation(int buskode, float latitude, float longitude)
        {
            this.busStationKey = buskode;
            this.latitude = latitude;
            this.longitude = longitude;
        }
        public int BusStationKey
        {
            get
            {
                return busStationKey;
            }
            set
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
            set
            {
                Random r1 = new random();
                latitude = r1.Next(31, (int)33.3);
            }
        }
        public float Longitude
        {
            get 
            {
                return longitude;
            }
            set 
            {
                Random r2 = new random();
                longitude = r2.Next(34.3, 35.5);
            }
        }
        public override string ToString()
        {
            return "Bus Station Code: " + busStationKey + ", " + latitude + " ," + longitude;
        }
    }
}
