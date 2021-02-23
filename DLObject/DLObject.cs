using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DLAPI;
using DS;

namespace DL
{
    sealed class DLObject : IDL
    {
        #region singelton
        static readonly DLObject instance = new DLObject();
        static DLObject() { }// static ctor to ensure instance init is done just before first usage
        DLObject() { } // default => private
        public static DLObject Instance { get => instance; }// The public Instance property to use
        #endregion

        #region Station
        public IEnumerable<DO.Station> GetAllStations()
        {
            return from station in DataSource.ListStations
                   select station.Clone();
        }

        public IEnumerable<DO.Station> GetAllStationsBy(Predicate<DO.Station> predicate)
        {
            throw new NotImplementedException();
        }

        public DO.Station GetStation(int code)
        {
            DO.Station sta = DataSource.ListStations.Find(s => s.Code == code);

            if (sta != null)
                return sta.Clone();
            else
                throw new DO.BadStationCodeException(code, $"bad station code: {code}");
        }

        public void AddStation(DO.Station station)
        {
            if (DataSource.ListStations.FirstOrDefault(s => s.Code == station.Code) != null)
                throw new DO.BadStationCodeException(station.Code, "Duplicate station code");
            DataSource.ListStations.Add(station.Clone());
        }

        public void DeleteStation(int code)
        {
            DO.Station sta = DataSource.ListStations.Find(s => s.Code == code);

            if (sta != null)
            {
                DataSource.ListStations.Remove(sta);
            }
            else
                throw new DO.BadStationCodeException(code, $"bad station code: {code}");
        }

        public void UpdateStation(DO.Station station)
        {
            DO.Station sta = DataSource.ListStations.Find(s => s.Code == station.Code);

            if (sta != null)
            {
                DataSource.ListStations.Remove(sta);
                DataSource.ListStations.Add(station.Clone());
            }
            else
                throw new DO.BadStationCodeException(station.Code, $"bad station code: {station.Code}");
        }

        public void UpdateStation(int code, Action<DO.Station> update)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Line
        public IEnumerable<DO.Line> GetAllLines()
        {
            return from line in DataSource.ListLines
                   select line.Clone();
        }

        public IEnumerable<DO.Line> GetAllLinesBy(Predicate<DO.Line> predicate)
        {
            throw new NotImplementedException();
        }

        public DO.Line GetLine(int id)
        {
            DO.Line li = DataSource.ListLines.Find(l => l.ID == id);

            if (li != null)
                return li.Clone();
            else
                throw new DO.BadLineIDException(id, $"bad line id: {id}");
        }

        public void AddLine(DO.Line line)
        {
            if (DataSource.ListLines.FirstOrDefault(l => l.ID == line.ID) != null)
                throw new DO.BadLineIDException(line.ID, "Duplicate line id");
            DataSource.ListLines.Add(line.Clone());
        }

        public void DeleteLine(int id)
        {
            DO.Line li = DataSource.ListLines.Find(l => l.ID == id);

            if (li != null)
            {
                DataSource.ListLines.Remove(li);
            }
            else
                throw new DO.BadLineIDException(id, $"bad line id: {id}");
        }

        public void UpdateLine(DO.Line line)
        {
            DO.Line li = DataSource.ListLines.Find(l => l.ID == line.ID);

            if (li != null)
            {
                DataSource.ListLines.Remove(li);
                DataSource.ListLines.Add(line.Clone());
            }
            else
                throw new DO.BadLineIDException(line.ID, $"bad line id: {line.ID}");
        }

        public void UpdateLine(int id, Action<DO.Line> update)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region StationOfLine
        public IEnumerable<DO.StationOfLine> GetAllStationsOfLines()
        {
            return from station in DataSource.ListStationsOfLines
                   select station.Clone();
        }

        public IEnumerable<DO.StationOfLine> GetAllStationsOfLinesBy(Predicate<DO.StationOfLine> predicate)
        {
            return from sol in DataSource.ListStationsOfLines
                   where predicate(sol)
                   select sol.Clone();
        }

        public IEnumerable<DO.StationOfLine> GetAllStationsOfLine(int lineID)
        {
            return from sol in DataSource.ListStationsOfLines
                   where sol.LineID == lineID
                   select sol.Clone();
        }

        public DO.StationOfLine GetStationOfLine(int lineID, int stationCode)
        {
            DO.StationOfLine sol = DataSource.ListStationsOfLines.Find(sl => sl.LineID == lineID && sl.StationCode == stationCode);

            if (sol != null)
                return sol.Clone();
            else
                throw new DO.BadLineIDStationCodeException(lineID, stationCode, "bad station code or line id");
        }
        public void AddStationOfLine(DO.StationOfLine stationOfLine)
        {

            DO.StationOfLine sol = DataSource.ListStationsOfLines.Find(sl => sl.LineID == stationOfLine.LineID && sl.StationCode == stationOfLine.StationCode);
            if (sol != null)
                throw new DO.BadLineIDStationCodeException(stationOfLine.LineID, stationOfLine.StationCode, "Duplicate station code");
            DataSource.ListStationsOfLines.Add(stationOfLine.Clone());
        }
        public void AddStationOfLine(int lineId, int stationCode)
        {
            if (DataSource.ListStationsOfLines.FirstOrDefault(sols => (sols.LineID == lineId && sols.StationCode == stationCode)) != null)
                throw new DO.BadLineIDStationCodeException(lineId, stationCode, "line ID is already registered to station code");
            DO.StationOfLine sol = new DO.StationOfLine() { LineID = lineId, StationCode = stationCode};
            DataSource.ListStationsOfLines.Add(sol);
        }

        public void DeleteStationOfLine(int lineID, int stationCode)
        {
            DO.StationOfLine sol = DataSource.ListStationsOfLines.Find(sl => sl.LineID == lineID && sl.StationCode == stationCode);

            if (sol != null)
            {
                DataSource.ListStationsOfLines.Remove(sol);
            }
            else
                throw new DO.BadLineIDStationCodeException(lineID, stationCode, "Worng line id or station code");
        }

        public void UpdateStationOfLine(DO.StationOfLine stationOfLine)
        {
            DO.StationOfLine sol = DataSource.ListStationsOfLines.Find(sl => sl.LineID == stationOfLine.LineID && sl.StationCode == stationOfLine.StationCode);

            if (sol != null)
            {
                DataSource.ListStationsOfLines.Remove(sol);
                DataSource.ListStationsOfLines.Add(stationOfLine.Clone());
            }
            else
                throw new DO.BadLineIDStationCodeException(stationOfLine.LineID, stationOfLine.StationCode, "Worng station of line");
        }

        public void UpdateStationOfLine(int lineID, int stationCode, Action<DO.StationOfLine> update)
        {
            throw new NotImplementedException();
        }
        #endregion

    }
}
