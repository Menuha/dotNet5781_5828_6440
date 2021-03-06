using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLAPI
{
    public interface IBL
    {
        #region Line
        BO.Line GetLine(int id);
        IEnumerable<BO.Line> GetAllLines();
        int AddLine(int number, BO.Areas newArea, int firstStationCode, int lastStationCode);
        void UpdateLine(BO.Line line);
        void DeleteLine(int id);
        //IEnumerable<BO.Line> GetAllLinesBy(Predicate<BO.Line> predicate);
        //void UpdateLine(int id, Action<BO.Line> update); //method that knows to update specific fields in Line
        #endregion

        #region Station
        BO.Station GetStation(int code);
        IEnumerable<BO.Station> GetAllStations();
        void AddStation(BO.Station station);
        void UpdateStation(BO.Station station);
        void DeleteStation(int code);
        //IEnumerable<BO.Station> GetAllStationsBy(Predicate<BO.Station> predicate);
        //void UpdateStation(int code, Action<BO.Station> update); //method that knows to update specific fields in Station
        #endregion

        #region StationOfLine
        IEnumerable<BO.StationOfLine> GetAllStationsOfLine(int id);
        void AddStationOfLine(int lineID, int stationCode);
        void DeleteStationOfLine(int lineID, int stationCode);
        void UpdateStationIndexInLine(int lineID, int stationCode, int newIndex);
        #endregion

        #region LineOfStation
        IEnumerable<BO.LineOfStation> GetAllLinesOfStation(int code);

        IEnumerable<BO.LineOfStation> GetAllLinesOfStationSorted(int stationCode);
        #endregion

        #region AdjacentStations
        IEnumerable<BO.AdjacentStations> GetMyAdjacentStations(int stationCode);
        void AddAdjacentStations(int station1Code, int station2Code);
        #endregion

        #region LineTrip

        IEnumerable<BO.LineTrip> GetAllLineTrips(int lineID);
        int AddLineTrip(int lineID, TimeSpan startAt);
        void DeleteLineTrip(int lineTripID);
        //IEnumerable<BO.LineTrip> GetAllLinesTrips();
        //IEnumerable<BO.LineTrip> GetAllLinesTripsBy(Predicate<BO.Line> predicate);
        //void UpdateLineTrip(BO.LineTrip lineTrip);

        #endregion

        #region LineTiming
        IEnumerable<BO.LineTiming> GetAllLinesTimingOfStation(int stationCode, TimeSpan time);
        #endregion
    }
}
