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
        IEnumerable<BO.Line> GetAllLinesBy(Predicate<BO.Line> predicate);
        void AddLine(BO.Line line);
        void UpdateLine(BO.Line line);
        void UpdateLine(int id, Action<BO.Line> update); //method that knows to update specific fields in Line
        void DeleteLine(int id);
        #endregion

        #region Station
        BO.Station GetStation(int code);
        IEnumerable<BO.Station> GetAllStations();
        IEnumerable<BO.Station> GetAllStationsBy(Predicate<BO.Station> predicate);
        void AddStation(BO.Station station);
        void UpdateStation(BO.Station station);
        void UpdateStation(int code, Action<BO.Station> update); //method that knows to update specific fields in Station
        void DeleteStation(int code);

        #endregion

        #region LineOfStation
        IEnumerable<BO.LineOfStation> GetAllLinesOfStation(int code);
        #endregion

        #region StationOfLine
        IEnumerable<BO.StationOfLine> GetAllStationsOfLine(int id);
        void DeleteStationOfLine(int lineId, int stationCode);
        #endregion
    }
}
