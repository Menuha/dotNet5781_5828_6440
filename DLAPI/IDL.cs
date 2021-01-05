using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLAPI
{
    public interface IDL
    {
        #region Bus
        IEnumerable<DO.Bus> GetAllBuses();
        IEnumerable<DO.Bus> GetAllBusesBy(Predicate<DO.Bus> predicate);
        DO.Bus GetBus(int licenseNum);
        void AddBus(DO.Bus bus);
        void UpdateBus(DO.Bus bus);
        void UpdateBus(int licenseNum, Action<DO.Bus> update); //method that knows to update specific fields in Bus
        void DeleteBus(int licenseNum);
        #endregion

        #region Station
        IEnumerable<DO.Station> GetAllStations();
        IEnumerable<DO.Station> GetAllStationsBy(Predicate<DO.Station> predicate);
        DO.Station GetStation(int code);
        void AddStation(DO.Station station);
        void UpdateStation(DO.Station station);
        void UpdateStation(int code, Action<DO.Station> update); //method that knows to update specific fields in Station
        void DeleteStation(int code);
        #endregion

        #region Line
        IEnumerable<DO.Line> GetAllLines();
        IEnumerable<DO.Line> GetAllLinesBy(Predicate<DO.Line> predicate);
        DO.Line GetLine(int id);
        void AddLine(DO.Line line);
        void UpdateLine(DO.Line line);
        void UpdateLine(int id, Action<DO.Line> update); //method that knows to update specific fields in Line
        void DeleteLine(int id);
        #endregion

        #region StationOfLine
        IEnumerable<DO.StationOfLine> GetAllStationsOfLine();
        IEnumerable<DO.StationOfLine> GetAllStationsOfLineBy(Predicate<DO.StationOfLine> predicate);
        DO.StationOfLine GetStationOfLine(int lineId, int stationCode);
        void AddStationOfLine(DO.StationOfLine lineStation);
        void UpdateStationOfLine(DO.StationOfLine lineStation);
        void UpdateStationOfLine(int lineId, int stationCode, Action<DO.StationOfLine> update); //method that knows to update specific fields in LineStation
        void DeleteStationOfLine(int lineId, int stationCode);
        #endregion
    }
}
