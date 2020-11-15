using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotNet5781_02_5828_6440
{
   // enum area { General = 1, North, South,Center, Jerusalem, East, West, Exit };
    class BusLine
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



        public bool checkStation(BusStation station)
        {
            bool flag = false;
            if(station.)
            return flag;
        }

}

}
}
