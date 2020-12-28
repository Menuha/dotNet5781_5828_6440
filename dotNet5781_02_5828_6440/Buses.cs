using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotNet5781_02_5828_6440
{
    class Buses :IEnumerable
    {
        private int counter = 0;
        private List<BusLine> busesList;
        
        public Buses()
        {
            BusesList = new List<BusLine>();
        }

        public List<BusLine> BusesList 
        { get => busesList; private set => busesList = value; }

        public void Add(BusLine bus)
        {
            int index = -1;
            for (int i = 0; i < busesList.Count; i++)
            {
             //check only if the bus code not exist more than 2 times and not first nd last stations because they are different.
                if (busesList[i].BusCode == bus.BusCode)
                {
                    counter++;
                    index = i;
                }
            }
            if (counter>2)
            {
                throw new ArgumentException("This bus already exist");
            }
            else if (counter == 1 && busesList[index].BusArea != bus.BusArea) 
            {
                throw new ArgumentException("Worng bus area");
            }
            else
                BusesList.Add(bus);
            counter = 0;
        }

        public bool Remove(int busCode)
        {
            bool flag = BusesList.Remove(Find(busCode));
            if (flag == false)
            {
                throw new ArgumentException("This bus doesn't exist in this list.");
            }
            return flag;
        }

        public BusLine Find(int busCode)
        {
            return BusesList.Find(x => x.BusCode == busCode);
        }

        public Buses LinesForStation(int station)
        {
            if (station >= 1000000)
                throw new FormatException("WRONG BUS STATION CODE");
            List<BusLine> subLines = new List<BusLine>();
            for (int i = 0; i < busesList.Count; i++) 
            {
                if (busesList[i].StationsList.Exists(x => x.Station.StationCode == station))
                {
                    subLines.Add(busesList[i]);
                }
            }
            if (subLines.Count == 0)
                throw new ArgumentException("There are no lines in this station");
            Buses subBuses = new Buses();
            subBuses.BusesList = subLines;
            return subBuses;
        }
   
        public List<BusLine> Sort()
        {
            List<BusLine> sortedList = new List<BusLine>();
            for (int i = 0; i < busesList.Count; i++)
            {
                sortedList[i] = busesList[i];
            }
            sortedList.Sort();
            return sortedList;
        }

        public BusLine this[int busCode]
        {
            get
            {
                for (int i = 0; i < BusesList.Count; i++)
                {
                    if (BusesList[i].BusCode == busCode)
                        return BusesList[i];
                }
                throw new ArgumentException("There is no such a bus in my list");
            }
        }

        public IEnumerator GetEnumerator()
        {
            return busesList.GetEnumerator();
        }

        public override string ToString()
        {
            string myToString = null;
            for (int i = 0; i < BusesList.Count; i++)
                myToString += BusesList[i].ToString() + "\n";
            return myToString;
        }
    }
}
