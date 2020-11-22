using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotNet5781_02_5828_6440
{
    enum Area { General = 1, North, South, East, West, Jerusalem, Center, Exit };

    /// <summary>
    /// Department for the representation of a single bus line (route of various stations of the bus line)
    /// </summary>
    class BusLine
    {
        /// <summary>
        /// A list containing the route of the various stations of a bus line
        /// </summary>
        private List<BusLineStation> stations;
        private string busCode;
        private BusLineStation firstStation;
        private BusLineStation lastStation;
        private Area busArea;
        
        /// <summary>
        /// Constractor of a bus line
        /// </summary>
        /// <param name="busCode">bus code</param>
        /// <param name="firstStation">first station of the bus</param>
        /// <param name="lastStation">last station of the bus</param>
        /// <param name="busArea">area of the bus</param>
        public BusLine(string busCode, BusLineStation firstStation, BusLineStation lastStation, Area busArea)
        {
            BusCode = busCode;
            FirstStation = firstStation;
            LastStation = lastStation;
            BusArea = busArea;
        }

        /// <summary>
        /// The bus code
        /// </summary>
        public string BusCode
        {
            get => busCode;

            private set => busCode = value;
        }

        /// <summary>
        /// The first station in the route of the bus
        /// </summary>
        public BusLineStation FirstStation
        { get => firstStation; private set => firstStation = value; }

        /// <summary>
        /// The last station in the route of the bus
        /// </summary>
        public BusLineStation LastStation
        { get => lastStation; private set => lastStation = value; }

        /// <summary>
        /// The area to which the bus is associated
        /// </summary>
        public Area BusArea
        { get => busArea; private set => busArea = value; }

        /// <summary>
        /// Adding a station to the line route
        /// </summary>
        /// <param name="station"></param>
        public void AddStation(BusStation station)
        {

        }

        /// <summary>
        /// Deleting a station from the line route
        /// </summary>
        /// <param name="station"></param>
        public void DeleteStation(BusStation station)
        {

        }

        /// <summary>
        /// Searching a station in the line route
        /// </summary>
        /// <param name="station">the station you want to search</param>
        /// <returns>"true" if the given station was found</returns>
        public bool SearchStation(BusStation station)
        {
            bool flag = false;
            if (station.)
                return flag;
        }

        /// <summary>
        /// Checking the distance between 2 stations that are on the line
        /// </summary>
        /// <param name="station1">station number 1</param>
        /// <param name="station2">station number 2</param>
        /// <returns>the distance between the stations</returns>
        public double Distance2S(BusStation station1, BusStation station2)
        {

        }

        /// <summary>
        /// Checking the travel time between 2 stations that are on the line
        /// </summary>
        /// <param name="station1">station number 1</param>
        /// <param name="station2">station number 2</param>
        /// <returns>the travel time between the stations</returns>
        public TimeSpan TravelTime2S(BusStation station1, BusStation station2)
        {

        }

        /// <summary>
        /// A method that returns a sub-trajectory of the line
        /// </summary>
        /// <param name="station1">station number 1</param>
        /// <param name="station2">station number 2</param>
        /// <returns>sub-trajectory of the line</returns>
        public BusLine SubLine(BusStation station1, BusStation station2)
        {

        }

        /// <summary>
        /// Comparison of 2 sub-routes of lines by comparing their time of travel between 2 given station
        /// </summary>
        /// <param name="otherLine">the second line</param>
        /// <param name="sourceStation">the source station</param>
        /// <param name="targetStation">the target station</param>
        /// <returns>the line where the travel time is shorter</returns>
        public BusLine Compare(BusLine otherLine, BusStation sourceStation, BusStation targetStation)
        {

        }

        /// <summary>
        /// A method that displays the bus line parameters
        /// </summary>
        /// <returns>Bus line parameters</returns>
        public override string ToString()
        {
            return "Number of bus: " + busCode + ", area: " + busArea + " , Stations: " + busLine;
        }
    }
}

