using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace dotNet5781_02_5828_6440
{
    public class BusStation
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
            set 
            {
                Random r2 = new random();
                longitude = r2.Next(343, 355);
                //longitude = longitude / 10;
            }
        }
        public override string ToString()
        {
            return "Bus Station Code: " + busStationKey + ", " + latitude + "°N ," + longitude + "°E";
        }
    }

}
