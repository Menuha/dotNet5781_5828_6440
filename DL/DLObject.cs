using DLAPI;
using DS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            DO.Station per = DataSource.ListStations.Find(p => p.Code == code);

            if (per != null)
                return per.Clone();
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
            DO.Station sta = DataSource.ListStations.Find(p => p.Code == code);

            if (sta != null)
            {
                DataSource.ListStations.Remove(sta);
            }
            else
                throw new DO.BadStationCodeException(code, $"bad person id: {code}");
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
        #endregion Station

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
            DO.Line li = DataSource.ListLines.Find(l => l.Id == id);

            if (li != null)
                return li.Clone();
            else
                throw new DO.BadLineIdException(id, $"bad line id: {id}");
        }

        public void AddLine(DO.Line line)
        {
            if (DataSource.ListLines.FirstOrDefault(l => l.Id == line.Id) != null)
                throw new DO.BadLineIdException(line.Id, "Duplicate line id");
            DataSource.ListLines.Add(line.Clone());
        }

        public void DeleteLine(int id)
        {
            DO.Line li = DataSource.ListLines.Find(l => l.Id == id);

            if (li != null)
            {
                DataSource.ListLines.Remove(li);
            }
            else
                throw new DO.BadLineIdException(id, $"bad line id: {id}");
        }

        public void UpdateLine(DO.Line line)
        {
            DO.Line li = DataSource.ListLines.Find(l => l.Id == line.Id);

            if (li != null)
            {
                DataSource.ListLines.Remove(li);
                DataSource.ListLines.Add(line.Clone());
            }
            else
                throw new DO.BadLineIdException(line.Id, $"bad line id: {line.Id}");
        }

        public void UpdateLine(int id, Action<DO.Line> update)
        {
            throw new NotImplementedException();
        }
        #endregion Line

        #region StationOfLine
        public IEnumerable<DO.StationOfLine> GetAllStationsOfLine()
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
            DO.Station per = DataSource.ListStations.Find(p => p.Code == code);

            if (per != null)
                return per.Clone();
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
            DO.Station sta = DataSource.ListStations.Find(p => p.Code == code);

            if (sta != null)
            {
                DataSource.ListStations.Remove(sta);
            }
            else
                throw new DO.BadStationCodeException(code, $"bad person id: {code}");
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
        #endregion Station

    }
}
