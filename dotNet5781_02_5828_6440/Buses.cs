using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotNet5781_02_5828_6440
{
    class Buses
    {
        private List<BusLine> busesList;

        public Buses()
        {
            BusesList = new List<BusLine>();
        }

        public List<BusLine> BusesList 
        { get => busesList; private set => busesList = value; }

        public void Add(BusLine bus)
        {
            //בדיקות

            BusesList.Add(bus);
        }

        public bool Remove(BusLine bus)
        {
            bool flag = false;
            //בדיקות

            BusesList.Remove(bus);

            return flag;
        }

        public List<BusLine> LinesForStation(string station)
        {
            if (station.Length > 6)
                throw new FormatException("WRONG BUS STATION CODE");
            List<BusLine> subLines = new List<BusLine>();
            foreach (BusLine item in BusesList)
            {
                if (BusesList[0].StationsList.Exists(x => x.Station.StationCode == station))
                {
                    subLines.Add(item);
                }
            }
            if (subLines.Count == 0)
                throw new ArgumentException("There are no lines in this station");
            return subLines;
        }
   
        public List<BusLine> Sort()
        {
            List<BusLine> subLines = BusesList;
            subLines.Sort();
            return subLines;
        }

        public BusLine this[string index]
        {
            get
            {
                for (int i = 0; i < BusesList.Count; i++)
                {
                    if (BusesList[i].BusCode == index)
                        return BusesList[i];
                }
                throw new ArgumentException("There is no such a bus in my list");
            }
        }
    }
}
