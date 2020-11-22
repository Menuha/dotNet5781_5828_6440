using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotNet5781_02_5828_6440
{
   // enum area { General = 1, North, South,Center, Jerusalem, East, West, Exit };
    class BusLine:IComparable
    {
        //public List<BusLineStation>;
        public List<T> busLine;
        private int busCode;
        private BusLineStation firstStation;
        private BusLineStation lasttStation;
        private string area;

        public int BusCode 
        {  
            get => busCode;
          
            set => busCode = value;
        }
        public BusLineStation StartStation { get => startStation; set => startStation = value; }
        public BusLineStation FinaltStation { get => finaltStation; set => finaltStation = value; }
        public string Area { get => area; set => area = value; }


        public override string ToString()
        {
            return "Number of bus: " + busCode + ", area: " + area + " , Stations: " + busLine;
        }
        public BusLine AddStation(BusStation sta)
        {
            Console.WriteLine("Press 1 to add the station to the first statio");
            busLine.Add(sta)
            Console.WriteLine("Press 2 to add the station to the last statio");
            busLine.Add(sta)
        }
        public BusLine DeleteStation()
        {

        }
        public bool CheckStation(BusStation station)
        {
            bool flag = false;
            if()
            return flag;
        }
        public double DisBetStations(BusLineStation a, BusLineStation b)
        {
            //חריגות שאכן נמצאות

            return;
        }
    public int CompareTo(object obj)
        {
            BusLine s = (BusLine)obj;
            if (travelTime == s.travelTime)
                return 0;
            else if (travelTime > s.travelTime)
                return 1;
            else if (travelTime < s.travelTime)
                return -1;
        }
    }

}

