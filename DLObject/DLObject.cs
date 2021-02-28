using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Device.Location;

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
            DO.StationOfLine sols = DataSource.ListStationsOfLines.Find(sl => sl.StationCode == code);
            if (sta != null && sols == null)
            {
                DataSource.ListStations.Remove(sta);
            }
            else if (sols != null)
            {
                throw new DO.BadStationCodeException(code, $"There is a line passing through the station with code: {code}");
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

        //public IEnumerable<DO.Station> GetAllStationsBy(Predicate<DO.Station> predicate)
        //{
        //    throw new NotImplementedException();
        //}
        //public void UpdateStation(int code, Action<DO.Station> update)
        //{
        //    throw new NotImplementedException();
        //}
        #endregion

        #region Line
        public IEnumerable<DO.Line> GetAllLines()
        {
            return from line in DataSource.ListLines
                   select line.Clone();
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

        //public IEnumerable<DO.Line> GetAllLinesBy(Predicate<DO.Line> predicate)
        //{
        //    throw new NotImplementedException();
        //}
        //public void UpdateLine(int id, Action<DO.Line> update)
        //{
        //    throw new NotImplementedException();
        //}
        #endregion

        #region StationOfLine
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

        public DO.StationOfLine GetPrevSol(int lineID, int stationCode)
        {
            int myIndex = GetStationOfLine(lineID, stationCode).StationIndexInLine;
            DO.StationOfLine prevSol = GetAllStationsOfLine(lineID).FirstOrDefault(sol => sol.StationIndexInLine == myIndex - 1);
            if (prevSol != null)
                return prevSol.Clone();
            else
                throw new DO.BadLineIDStationCodeException(lineID, stationCode, "this is the first station of this line");
        }

        public DO.StationOfLine GetNextSol(int lineID, int stationCode)
        {
            int myIndex = GetStationOfLine(lineID, stationCode).StationIndexInLine;
            DO.StationOfLine prevSol = GetAllStationsOfLine(lineID).FirstOrDefault(sol => sol.StationIndexInLine == myIndex + 1);
            if (prevSol != null)
                return prevSol.Clone();
            else
                throw new DO.BadLineIDStationCodeException(lineID, stationCode, "this is the last station of this line");
        }

        public void AddStationOfLine(int lineID, int stationCode)
        {
            if (DataSource.ListStationsOfLines.FirstOrDefault(sols => (sols.LineID == lineID && sols.StationCode == stationCode)) != null)
                throw new DO.BadLineIDStationCodeException(lineID, stationCode, $"line ID {lineID} is already registered to station code {stationCode}");
            int index = DataSource.ListStationsOfLines.FindAll(sols => sols.LineID == lineID).Count() + 1;
            DO.StationOfLine sol = new DO.StationOfLine() { LineID = lineID, StationCode = stationCode, StationIndexInLine = index };
            DataSource.ListStationsOfLines.Add(sol);
            DataSource.ListLines.Find(li => li.ID == lineID).LastStationCode = stationCode;
        }
       
        public void DeleteStationOfLine(int lineID, int stationCode)
        {
            DO.StationOfLine sol = DataSource.ListStationsOfLines.Find(sl => sl.LineID == lineID && sl.StationCode == stationCode);
            int routeLength = GetAllStationsOfLine(lineID).Count();
            if (routeLength == 2)
                throw new DO.BadLineIDStationCodeException(lineID, stationCode, "Line route should have at least 2 stations");
            if (sol != null)
            {
                UpdateStationIndexInLine(lineID, stationCode, routeLength);
                DataSource.ListStationsOfLines.Remove(sol);
                //Update last station of this line
                DataSource.ListLines.Find(li => li.ID == lineID).LastStationCode = DataSource.ListStationsOfLines.Find(s => s.StationIndexInLine == GetAllStationsOfLine(lineID).Count()).StationCode;
            }
            else
                throw new DO.BadLineIDStationCodeException(lineID, stationCode, "Worng line id or station code");
        }

        public void DeleteSolByLine(int lineID)
        {
            DataSource.ListStationsOfLines.RemoveAll(sol => sol.LineID == lineID);
        }

        public void UpdateStationIndexInLine(int lineID, int stationCode, int newIndex)
        {
            DO.StationOfLine sol = DataSource.ListStationsOfLines.Find(sl => sl.LineID == lineID && sl.StationCode == stationCode);
            if (sol != null)
            {
                //List of stations between the previous index and the new index
                List<DO.StationOfLine> solList = DataSource.ListStationsOfLines.FindAll(sl => sl.LineID == lineID 
                    && sl.StationIndexInLine >= Math.Min(sol.StationIndexInLine, newIndex) 
                    && sl.StationIndexInLine <= Math.Max(sol.StationIndexInLine, newIndex));
                //Update the route of the stations between the previous index and the new index
                for (int i = 0; i < solList.Count(); i++)
                {
                    if (newIndex < sol.StationIndexInLine)
                        solList[i].StationIndexInLine++;
                    else
                        solList[i].StationIndexInLine--;
                }
                sol.StationIndexInLine = newIndex;
                //Update first and last stations for this line
                DataSource.ListLines.Find(li => li.ID == lineID).FirstStationCode = DataSource.ListStationsOfLines.Find(s => s.StationIndexInLine == 1).StationCode;
                DataSource.ListLines.Find(li => li.ID == lineID).LastStationCode = DataSource.ListStationsOfLines.Find(s => s.StationIndexInLine == GetAllStationsOfLine(lineID).Count()).StationCode;
            }
            else
                throw new DO.BadLineIDStationCodeException(lineID, stationCode, "Worng station of line");
        }

        //public IEnumerable<DO.StationOfLine> GetAllStationsOfLines()
        //{
        //    return from station in DataSource.ListStationsOfLines
        //           select station.Clone();
        //}


        //public void AddStationOfLine(DO.StationOfLine stationOfLine)
        //{
        //    DO.StationOfLine sol = DataSource.ListStationsOfLines.Find(sl => sl.LineID == stationOfLine.LineID && sl.StationCode == stationOfLine.StationCode);
        //    if (sol != null)
        //        throw new DO.BadLineIDStationCodeException(stationOfLine.LineID, stationOfLine.StationCode, "Duplicate station code");
        //    DataSource.ListStationsOfLines.Add(stationOfLine.Clone());
        //}
        //public void DeleteSolByStation(int stationCode)
        //{
        //    DataSource.ListStationsOfLines.RemoveAll(sol => sol.StationCode == stationCode);
        //}

        //public void DeleteStationsOfLinesBy(Predicate<DO.StationOfLine> predicate)
        //{
        //    DataSource.ListStationsOfLines.RemoveAll(sol => predicate(sol));
        //}
        //public void UpdateStationOfLine(int lineID, int stationCode, Action<DO.StationOfLine> update)
        //{
        //    throw new NotImplementedException();
        //}

        //public void UpdateStationOfLine(DO.StationOfLine stationOfLine)
        //{
        //    DO.StationOfLine sol = DataSource.ListStationsOfLines.Find(sl => sl.LineID == stationOfLine.LineID && sl.StationCode == stationOfLine.StationCode);
        //    if (sol != null)
        //    {
        //        DataSource.ListStationsOfLines.Remove(sol);
        //        DataSource.ListStationsOfLines.Add(stationOfLine.Clone());
        //    }
        //    else
        //        throw new DO.BadLineIDStationCodeException(stationOfLine.LineID, stationOfLine.StationCode, "Worng station of line");
        //}
        #endregion

        #region AdjacentStations
        public IEnumerable<DO.AdjacentStations> GetAllAdjacentStations()
        {
            return from adjSt in DataSource.ListAdjacentStations
                   select adjSt.Clone();
        }

        public DO.AdjacentStations GetAdjacentStations(int station1Code, int station2Code)
        {
            DO.AdjacentStations adjSta = DataSource.ListAdjacentStations.Find(adjS => (adjS.Station1Code == station1Code && adjS.Station2Code == station2Code) || (adjS.Station2Code == station1Code && adjS.Station1Code == station2Code));
            if (adjSta != null)
                return adjSta.Clone();
            else
                throw new DO.BadStationCodeException(station1Code, "Adjacent stations is not found");

        }

        public IEnumerable<DO.AdjacentStations> GetMyAdjacentStations(int stationCode)
        {
            return from adjSt in GetAllAdjacentStations()
                   where adjSt.Station1Code == stationCode || adjSt.Station2Code == stationCode
                   select adjSt.Clone();
        }

        public void AddAdjacentStations(int station1Code, int station2Code)
        {
            if (DataSource.ListStations.FirstOrDefault(sta => (sta.Code == station1Code)) == null)
                throw new DO.BadStationCodeException(station1Code, $"station code {station1Code} is not found");
            if (DataSource.ListStations.FirstOrDefault(sta => (sta.Code == station2Code)) == null)
                throw new DO.BadStationCodeException(station2Code, $"station code {station2Code} is not found");
            if (DataSource.ListAdjacentStations.FirstOrDefault(adjSt => (adjSt.Station1Code == station1Code && adjSt.Station2Code == station2Code) || (adjSt.Station1Code == station2Code && adjSt.Station2Code == station1Code)) == null)
            {
                DO.Station station1 = GetStation(station1Code);
                DO.Station station2 = GetStation(station2Code);
                var sCoord = new GeoCoordinate(station1.Longitude, station1.Latitude);
                var eCoord = new GeoCoordinate(station2.Longitude, station2.Latitude);
                var distance = sCoord.GetDistanceTo(eCoord);
                
                DO.AdjacentStations adjSt = new DO.AdjacentStations() {
                    Station1Code = station1Code, 
                    Station2Code = station2Code,
                    Distance = 1.5 * distance,
                    //speed = 10 m/s
                    AvgTime = new TimeSpan(0, 0, Convert.ToInt32(1.5 * distance / 10))
                };
                DataSource.ListAdjacentStations.Add(adjSt);
            }
            //
        }

        public void DeleteAdjacentStationsByStation(int stationCode)
        {
            DataSource.ListAdjacentStations.RemoveAll(adjS => adjS.Station1Code == stationCode || adjS.Station2Code == stationCode);
        }

        public void UpdateAdjacentStations(DO.Station stationDO)
        {
            List<DO.AdjacentStations> myAdjacentStations = DataSource.ListAdjacentStations.FindAll(adjS => adjS.Station1Code == stationDO.Code || adjS.Station2Code == stationDO.Code);
            DataSource.ListAdjacentStations.RemoveAll(adjS => adjS.Station1Code == stationDO.Code || adjS.Station2Code == stationDO.Code);
            for (int i = 0; i < myAdjacentStations.Count(); i++)
            {
                AddAdjacentStations(myAdjacentStations[i].Station1Code, myAdjacentStations[i].Station2Code);
            }
        }
        #endregion

    }
}
