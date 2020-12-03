using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotNet5781_02_5828_6440
{
    enum Area { General = 1, North, South, East, West, Jerusalem, Center };

    /// <summary>
    /// Class for the representation of a single bus line (route of various stations of the bus line)
    /// </summary>
    class BusLine : IComparable<BusLine>
    {
        private const int maxSecond = 10800;

        private List<BusLineStation> stationsList;
        private int busCode;
        private BusStation firstStation;
        private BusStation lastStation;
        private Area busArea;

        /// <summary>
        /// Constractor of a bus line
        /// </summary>
        /// <param name="busCode">bus code</param>
        /// <param name="firstStation">first station of the bus</param>
        /// <param name="lastStation">last station of the bus</param>
        /// <param name="busArea">area of the bus</param>
        public BusLine(int busCode, BusStation firstStation, BusStation lastStation, Area busArea)
        {
            StationsList = new List<BusLineStation>();
            BusCode = busCode;
            FirstStation = firstStation;
            LastStation = lastStation;
            BusArea = busArea;
        }

        /// <summary>
        /// A list containing the route of the various stations of a bus line
        /// </summary>
        public List<BusLineStation> StationsList
        { get => stationsList; set => stationsList = value; }

        /// <summary>
        /// The bus code
        /// </summary>
        public int BusCode
        {
            get => busCode;
            private set => busCode = value;
        }

        /// <summary>
        /// The first station in the route of the bus
        /// </summary>
        public BusStation FirstStation
        {
            get => firstStation;
            private set
            {
                Insert(0, value);
            }
        }

        /// <summary>
        /// The last station in the route of the bus
        /// </summary>
        public BusStation LastStation
        {
            get => lastStation;
            private set
            {
                Insert(StationsList.Count, value);
            }
        }

        /// <summary>
        /// The area to which the bus is associated
        /// </summary>
        public Area BusArea
        { get => busArea; private set => busArea = value; }

        /// <summary>
        /// Inserting a station to the line route
        /// </summary>
        /// <param name="index">the index of the new station</param>
        /// <param name="station"></param>
        public void Insert(int index, BusStation station)
        {
            if (index < 0 || index > StationsList.Count)
                throw new ArgumentOutOfRangeException();
            if (Exists(station))
                throw new ArgumentException("This station already exsist in this bus line");
            double distanceFromPre = 0;
            TimeSpan travelTimeFromPre = new TimeSpan(0, 0, 0);
            if (index > 0)
            {
                distanceFromPre = BusStation.Gap2S(StationsList[index - 1].Station, station);
                travelTimeFromPre = new TimeSpan(0, 0, new Random().Next(maxSecond));
            }
            StationsList.Insert(index, new BusLineStation(station, distanceFromPre, travelTimeFromPre));
            //Update distances, if there is another station after the new station  
            if ((index + 1) < StationsList.Count)
            {
                StationsList[index + 1].DistanceFromPre = BusStation.Gap2S(station, StationsList[index + 1].Station);
                StationsList[index + 1].TravelTimeFromPre = new TimeSpan(0, 0, new Random().Next(maxSecond));
            }
            firstStation = StationsList[0].Station;
            lastStation = StationsList[StationsList.Count - 1].Station;
        }

        /// <summary>
        /// Removing a station from the line route
        /// </summary>
        /// <param name="station"> the station to remove</param>
        /// <returns> true if item is successfully removed; otherwise, false.</returns>
        public bool Remove(BusStation station)
        {
            if (StationsList.Count == 2)
                throw new ArgumentException("Removing a station can't be done, the bus has only 2 stations");
            int index = StationsList.FindIndex(x => x.Station == station);
            bool flag = StationsList.Remove(StationsList.Find(x => x.Station == station));
            if (index == 0 && StationsList.Count < 0)
            {
                firstStation = StationsList[0].Station;
                StationsList[0].DistanceFromPre = 0;
                StationsList[0].TravelTimeFromPre = new TimeSpan(0, 0, 0);
            }
            else if(index == StationsList.Count)
            {
                lastStation = StationsList[StationsList.Count - 1].Station;
            }
            return flag;
        }

        /// <summary>
        /// Searching a station in the line route
        /// </summary>
        /// <param name="station">the station you want to search</param>
        /// <returns>"true" if the given station was found</returns>
        public bool Exists(BusStation station)
        {
            return StationsList.Exists(x => x.Station == station);
        }

        /// <summary>
        /// Calculating the distance between 2 stations that are on the line
        /// </summary>
        /// <param name="station1">station number 1</param>
        /// <param name="station2">station number 2</param>
        /// <returns>the distance between the stations</returns>
        public double Distance2S(BusStation station1, BusStation station2)
        {
            double distance = 0;
            int index1 = StationsList.FindIndex(x => x.Station == station1);
            int index2 = StationsList.FindIndex(x => x.Station == station2);
            if(index1 > index2)
            {
                int tmp = index1;
                index1 = index2;
                index2 = tmp;
            }
            index1++;
            for(; index1 < index2; index1++)
            {
                distance += StationsList[index1].DistanceFromPre;
            }
            return distance;
        }

        /// <summary>
        /// Calculating the travel time between 2 stations that are on the line
        /// </summary>
        /// <param name="station1">station number 1</param>
        /// <param name="station2">station number 2</param>
        /// <returns>the travel time between the stations</returns>
        public TimeSpan TravelTime2S(BusStation station1, BusStation station2)
        {
            TimeSpan timeBetween = new TimeSpan(0, 0, 0);
            int index1 = StationsList.FindIndex(x => x.Station == station1);
            int index2 = StationsList.FindIndex(x => x.Station == station2);
            if (index1 > index2)
            {
                int tmp = index1;
                index1 = index2;
                index2 = tmp;
            }
            index1++;
            for (; index1 < index2; index1++)
            {
                timeBetween += StationsList[index1].TravelTimeFromPre;
            }
            return timeBetween;
        }

        /// <summary>
        /// A method that returns a sub-trajectory of the line
        /// </summary>
        /// <param name="station1">station number 1</param>
        /// <param name="station2">station number 2</param>
        /// <returns>sub-trajectory of the line</returns>
        public BusLine SubLine(BusStation station1, BusStation station2)
        {
            BusLine subLine = new BusLine(BusCode, station1, station2, BusArea);
            int index1 = StationsList.FindIndex(x => x.Station == station1);
            int index2 = StationsList.FindIndex(x => x.Station == station2);
            if (index1 > index2)
            {
                int tmp = index1;
                index1 = index2;
                index2 = tmp;
            }
            List<BusLineStation> subLineStations = StationsList.GetRange(index1, index2 - index1 + 1);
            subLine.StationsList = subLineStations;
            return subLine;
        }

        /// <summary>
        /// A method that displays the bus line parameters
        /// </summary>
        /// <returns>Bus line parameters</returns>
        public override string ToString()
        {
            string myToString = "Number of bus: " + busCode + ", area: " + busArea + " , Stations:\n";
            for (int i = 0; i < StationsList.Count; i++)
                myToString += StationsList[i].Station.StationCode + "\n";
            return myToString;
        }

        /// <summary>
        /// Comparison of 2 sub-routes of lines by comparing their time of travel
        /// </summary>
        /// <param name="other">the second line</param>
        /// <returns>the line where the travel time is shorter</returns>
        public int CompareTo(BusLine other)
        {
            return TravelTime2S(FirstStation, LastStation).CompareTo(other.TravelTime2S(FirstStation, LastStation));
        }
    }

   
}


