﻿using System;
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
        //IEnumerable<DO.Station> GetAllStationsBy(Predicate<DO.Station> predicate);
        DO.Station GetStation(int code);
        void AddStation(DO.Station station);
        void UpdateStation(DO.Station station);
        //void UpdateStation(int code, Action<DO.Station> update); //method that knows to update specific fields in Station
        void DeleteStation(int code);
        #endregion

        #region Line
        IEnumerable<DO.Line> GetAllLines();
        //IEnumerable<DO.Line> GetAllLinesBy(Predicate<DO.Line> predicate);
        DO.Line GetLine(int id);
        void AddLine(DO.Line line);
        void UpdateLine(DO.Line line);
        //void UpdateLine(int id, Action<DO.Line> update); //method that knows to update specific fields in Line
        void DeleteLine(int id);
        #endregion

        #region StationOfLine
        //IEnumerable<DO.StationOfLine> GetAllStationsOfLines();
        IEnumerable<DO.StationOfLine> GetAllStationsOfLinesBy(Predicate<DO.StationOfLine> predicate);
        IEnumerable<DO.StationOfLine> GetAllStationsOfLine(int lineID);
        //DO.StationOfLine GetStationOfLine(int lineID, int stationCode);
        void AddStationOfLine(int lineId, int stationCode);
        //void AddStationOfLine(DO.StationOfLine stationOfLine);
        void DeleteStationOfLine(int lineID, int stationCode);
        void DeleteSolByLine(int lineID);
        void DeleteSolByStation(int stationCode);
        //void DeleteStationsOfLinesBy(Predicate<DO.StationOfLine> predicate);
        void UpdateStationOfLine(DO.StationOfLine stationOfLine);
        //void UpdateStationOfLine(int lineID, int stationCode, Action<DO.StationOfLine> update); //method that knows to update specific fields in LineStation
        #endregion

        #region AdjacentStations
        void AddAdjacentStations(int station1Code, int station2Code);
        IEnumerable<DO.AdjacentStations> GetAllAdjacentStations();
        #endregion

    }
}
