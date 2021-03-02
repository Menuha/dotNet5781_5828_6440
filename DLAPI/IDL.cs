using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLAPI
{
    public interface IDL
    {
        #region Station
        IEnumerable<DO.Station> GetAllStations();
        DO.Station GetStation(int code);
        void AddStation(DO.Station station);
        void UpdateStation(DO.Station station);
        void DeleteStation(int code);
        //IEnumerable<DO.Station> GetAllStationsBy(Predicate<DO.Station> predicate);
        //void UpdateStation(int code, Action<DO.Station> update); //method that knows to update specific fields in Station
        #endregion

        #region Line
        IEnumerable<DO.Line> GetAllLines();        
        DO.Line GetLine(int id);
        int AddLine(int number, DO.Areas newArea, int firstStationCode, int lastStationCode);
        void UpdateLine(DO.Line line);
        void DeleteLine(int id);
        //IEnumerable<DO.Line> GetAllLinesBy(Predicate<DO.Line> predicate);
        //void UpdateLine(int id, Action<DO.Line> update); //method that knows to update specific fields in Line
        #endregion

        #region StationOfLine        
        IEnumerable<DO.StationOfLine> GetAllStationsOfLinesBy(Predicate<DO.StationOfLine> predicate);
        IEnumerable<DO.StationOfLine> GetAllStationsOfLine(int lineID);
        DO.StationOfLine GetStationOfLine(int lineID, int stationCode);
        DO.StationOfLine GetPrevSol(int lineID, int stationCode);
        DO.StationOfLine GetNextSol(int lineID, int stationCode);
        void AddStationOfLine(int lineID, int stationCode);
        void DeleteStationOfLine(int lineID, int stationCode);
        void DeleteSolByLine(int lineID);
        void UpdateStationIndexInLine(int lineID, int stationCode, int newIndex);
        //IEnumerable<DO.StationOfLine> GetAllStationsOfLines();
        //void AddStationOfLine(DO.StationOfLine stationOfLine);
        //void DeleteSolByStation(int stationCode);
        //void DeleteStationsOfLinesBy(Predicate<DO.StationOfLine> predicate);
        //void UpdateStationOfLine(DO.StationOfLine stationOfLine);
        //void UpdateStationOfLine(int lineID, int stationCode, Action<DO.StationOfLine> update); //method that knows to update specific fields in LineStation
        #endregion

        #region AdjacentStations
        IEnumerable<DO.AdjacentStations> GetAllAdjacentStations();
        DO.AdjacentStations GetAdjacentStations(int station1Code, int station2Code);
        IEnumerable<DO.AdjacentStations> GetMyAdjacentStations(int stationCode);
        void AddAdjacentStations(int station1Code, int station2Code);
        void DeleteAdjacentStationsByStation(int stationCode);
        void UpdateAdjacentStations(DO.Station stationDO);
        #endregion

    }
}
