using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using DLAPI;
using DO;
//using DO;

namespace DL
{
    sealed class DLXML : IDL    //internal
    {
        #region singelton
        static readonly DLXML instance = new DLXML();
        static DLXML() { }// static ctor to ensure instance init is done just before first usage
        DLXML() { } // default => private
        public static DLXML Instance { get => instance; }// The public Instance property to use
        #endregion

        #region DS XML Files

        string StationsPath = @"StationsXml.xml"; //XElement

        string LinesPath = @"LinesXml.xml"; //XMLSerializer
        string StationOfLinesPath = @"StationOfLinesXml.xml"; //XMLSerializer
        string AdjacentStationsPath = @"AdjacentStationsXml.xml"; //XElement

        #endregion

        #region Station
        public IEnumerable<DO.Station> GetAllStations()
        {
            XElement StationsRootElem = XMLTools.LoadListFromXMLElement(StationsPath);

            return (from sta in StationsRootElem.Elements()
                    select new Station()
                    {
                        Code = Int32.Parse(sta.Element("Code").Value),
                        Name = sta.Element("Name").Value,
                        Longitude = float.Parse(sta.Element("Longitude").Value),
                        Latitude = float.Parse(sta.Element("Latitude").Value)
                    }
                   );
        }
        public DO.Station GetStation(int code)
        {
            XElement StationsRootElem = XMLTools.LoadListFromXMLElement(StationsPath);

            Station s = (from sta in StationsRootElem.Elements()
                         where int.Parse(sta.Element("Code").Value) == code
                         select new Station()
                         {
                             Code = Int32.Parse(sta.Element("Code").Value),
                             Name = sta.Element("Name").Value,
                             Longitude = float.Parse(sta.Element("Longitude").Value),
                             Latitude = float.Parse(sta.Element("Latitude").Value)
                         }
                       ).FirstOrDefault();

            if (s == null)
                throw new DO.BadStationCodeException(code, $"bad station code: {code}");

            return s;
        }
        public void AddStation(DO.Station station)
        {
            XElement StationsRootElem = XMLTools.LoadListFromXMLElement(StationsPath);

            XElement sta1 = (from s in StationsRootElem.Elements()
                             where int.Parse(s.Element("ID").Value) == station.Code
                             select s).FirstOrDefault();

            if (sta1 != null)
                throw new DO.BadStationCodeException(station.Code, "Duplicate station code");

            XElement stationElem = new XElement("Station", new XElement("Code", station.Code),
                                  new XElement("Name", station.Name),
                                  new XElement("Longitude", station.Longitude),
                                  new XElement("Latitude", station.Latitude));

            StationsRootElem.Add(stationElem);

            XMLTools.SaveListToXMLElement(StationsRootElem, StationsPath);
        }
        public void DeleteStation(int code)
        {
            XElement stationsRootElem = XMLTools.LoadListFromXMLElement(StationsPath);
            XElement stationOfLinesRootElem = XMLTools.LoadListFromXMLElement(StationOfLinesPath);


            XElement sta = (from s in stationsRootElem.Elements()
                            where int.Parse(s.Element("Code").Value) == code
                            select s).FirstOrDefault();
            XElement sol = (from so in stationOfLinesRootElem.Elements()
                            where int.Parse(so.Element("StationCode").Value) == code
                            select so).FirstOrDefault();

            if (sta != null && sol == null)
            {
                sta.Remove(); //<==>   Remove sta from stationsRootElem

                XMLTools.SaveListToXMLElement(stationsRootElem, StationsPath);
            }
            else if (sol != null)
                throw new DO.BadStationCodeException(code, $"There is a line passing through the station with code: {code}");
            else
                throw new DO.BadStationCodeException(code, $"bad station code: {code}");
        }

        public void UpdateStation(DO.Station station)
        {

            XElement stationsRootElem = XMLTools.LoadListFromXMLElement(StationsPath);

            XElement sta = (from s in stationsRootElem.Elements()
                            where int.Parse(s.Element("Code").Value) == station.Code
                            select s).FirstOrDefault();

            if (sta != null)
            {
                sta.Element("Code").Value = station.Code.ToString();
                sta.Element("Name").Value = station.Name;
                sta.Element("Latitude").Value = station.Latitude.ToString();
                sta.Element("Longitude").Value = station.Longitude.ToString();


                XMLTools.SaveListToXMLElement(stationsRootElem, StationsPath);
            }
            else
                throw new DO.BadStationCodeException(station.Code, $"bad station code: {station.Code}");
        }

        #endregion

        #region Line
        public IEnumerable<DO.Line> GetAllLines()
        {
            List<Line> ListLines = XMLTools.LoadListFromXMLSerializer<Line>(LinesPath);
            return from line in ListLines
                   select line;
        }

        public DO.Line GetLine(int id)
        {
            List<Line> ListLines = XMLTools.LoadListFromXMLSerializer<Line>(LinesPath);
            DO.Line li = ListLines.Find(l => l.ID == id);

            if (li != null)
                return li;
            else
                throw new DO.BadLineIDException(id, $"bad line id: {id}");
        }

        public void AddLine(DO.Line line)
        {
            List<Line> ListLines = XMLTools.LoadListFromXMLSerializer<Line>(LinesPath);
            if (ListLines.FirstOrDefault(l => l.ID == line.ID) != null)
                throw new DO.BadLineIDException(line.ID, "Duplicate line id");

            ListLines.Add(line);
            XMLTools.SaveListToXMLSerializer(ListLines, LinesPath);
        }

        public void DeleteLine(int id)
        {
            List<Line> ListLines = XMLTools.LoadListFromXMLSerializer<Line>(LinesPath);
            DO.Line li = ListLines.Find(l => l.ID == id);

            if (li != null)
            {
                ListLines.Remove(li);
            }
            else
                throw new DO.BadLineIDException(id, $"bad line id: {id}");

            XMLTools.SaveListToXMLSerializer(ListLines, LinesPath);
        }

        public void UpdateLine(DO.Line line)
        {
            List<Line> ListLines = XMLTools.LoadListFromXMLSerializer<Line>(LinesPath);
            DO.Line li = ListLines.Find(l => l.ID == line.ID);
            if (li != null)
            {
                ListLines.Remove(li);
                ListLines.Add(line);
            }
            else
                throw new DO.BadLineIDException(line.ID, $"bad line id: {line.ID}");

            XMLTools.SaveListToXMLSerializer(ListLines, LinesPath);
        }

        #endregion

        #region StationOfLine
        public IEnumerable<DO.StationOfLine> GetAllStationsOfLinesBy(Predicate<DO.StationOfLine> predicate)
        {
            List<StationOfLine> ListStationsOfLines = XMLTools.LoadListFromXMLSerializer<StationOfLine>(StationOfLinesPath);
            return from sol in ListStationsOfLines
                   where predicate(sol)
                   select sol;
        }

        public IEnumerable<DO.StationOfLine> GetAllStationsOfLine(int lineID)
        {
            List<StationOfLine> ListStationsOfLines = XMLTools.LoadListFromXMLSerializer<StationOfLine>(StationOfLinesPath);
            return from sol in ListStationsOfLines
                   where sol.LineID == lineID
                   select sol;
        }

        public DO.StationOfLine GetStationOfLine(int lineID, int stationCode)
        {
            List<StationOfLine> ListStationsOfLines = XMLTools.LoadListFromXMLSerializer<StationOfLine>(StationOfLinesPath);
            DO.StationOfLine sol = ListStationsOfLines.Find(sl => sl.LineID == lineID && sl.StationCode == stationCode);
            if (sol != null)
                return sol;
            else
                throw new DO.BadLineIDStationCodeException(lineID, stationCode, "bad station code or line id");
        }

        public DO.StationOfLine GetPrevSol(int lineID, int stationCode)
        {
            List<StationOfLine> ListStationsOfLines = XMLTools.LoadListFromXMLSerializer<StationOfLine>(StationOfLinesPath);
            int myIndex = GetStationOfLine(lineID, stationCode).StationIndexInLine;
            DO.StationOfLine prevSol = GetAllStationsOfLine(lineID).FirstOrDefault(sol => sol.StationIndexInLine == myIndex - 1);
            if (prevSol != null)
                return prevSol;
            else
                throw new DO.BadLineIDStationCodeException(lineID, stationCode, "this is the first station of this line");
        }

        public DO.StationOfLine GetNextSol(int lineID, int stationCode)
        {
            List<StationOfLine> ListStationsOfLines = XMLTools.LoadListFromXMLSerializer<StationOfLine>(StationOfLinesPath);
            int myIndex = GetStationOfLine(lineID, stationCode).StationIndexInLine;
            DO.StationOfLine prevSol = GetAllStationsOfLine(lineID).FirstOrDefault(sol => sol.StationIndexInLine == myIndex + 1);
            if (prevSol != null)
                return prevSol;
            else
                throw new DO.BadLineIDStationCodeException(lineID, stationCode, "this is the last station of this line");
        }

        public void AddStationOfLine(int lineID, int stationCode)
        {
            List<StationOfLine> ListStationsOfLines = XMLTools.LoadListFromXMLSerializer<StationOfLine>(StationOfLinesPath);
            List<Line> ListLines = XMLTools.LoadListFromXMLSerializer<Line>(LinesPath);
            if (ListStationsOfLines.FirstOrDefault(sols => (sols.LineID == lineID && sols.StationCode == stationCode)) != null)
                throw new DO.BadLineIDStationCodeException(lineID, stationCode, $"line ID {lineID} is already registered to station code {stationCode}");
            int index = ListStationsOfLines.FindAll(sols => sols.LineID == lineID).Count() + 1;
            DO.StationOfLine sol = new DO.StationOfLine() { LineID = lineID, StationCode = stationCode, StationIndexInLine = index };
            ListStationsOfLines.Add(sol);
            ListLines.Find(li => li.ID == lineID).LastStationCode = stationCode;
            XMLTools.SaveListToXMLSerializer(ListLines, LinesPath);
            XMLTools.SaveListToXMLSerializer(ListStationsOfLines, StationOfLinesPath);
        }

        public void DeleteStationOfLine(int lineID, int stationCode)
        {
            List<StationOfLine> ListStationsOfLines = XMLTools.LoadListFromXMLSerializer<StationOfLine>(StationOfLinesPath);
            List<Line> ListLines = XMLTools.LoadListFromXMLSerializer<Line>(LinesPath);
            DO.StationOfLine sol = ListStationsOfLines.Find(sl => sl.LineID == lineID && sl.StationCode == stationCode);
            int routeLength = GetAllStationsOfLine(lineID).Count();
            if (routeLength == 2)
                throw new DO.BadLineIDStationCodeException(lineID, stationCode, "Line route should have at least 2 stations");
            if (sol != null)
            {
                UpdateStationIndexInLine(lineID, stationCode, routeLength);
                ListStationsOfLines.Remove(sol);
                //Update last station of this line
                ListLines.Find(li => li.ID == lineID).LastStationCode = ListStationsOfLines.Find(s => s.StationIndexInLine == GetAllStationsOfLine(lineID).Count()).StationCode;
            }
            else
                throw new DO.BadLineIDStationCodeException(lineID, stationCode, "Worng line id or station code");

            XMLTools.SaveListToXMLSerializer(ListStationsOfLines, StationOfLinesPath);
            XMLTools.SaveListToXMLSerializer(ListLines, LinesPath);
        }

        public void DeleteSolByLine(int lineID)
        {
            List<StationOfLine> ListStationsOfLines = XMLTools.LoadListFromXMLSerializer<StationOfLine>(StationOfLinesPath);
            ListStationsOfLines.RemoveAll(sol => sol.LineID == lineID);
            XMLTools.SaveListToXMLSerializer(ListStationsOfLines, StationOfLinesPath);
        }

        public void UpdateStationIndexInLine(int lineID, int stationCode, int newIndex)
        {
            List<StationOfLine> ListStationsOfLines = XMLTools.LoadListFromXMLSerializer<StationOfLine>(StationOfLinesPath);
            List<Line> ListLines = XMLTools.LoadListFromXMLSerializer<Line>(LinesPath);
            DO.StationOfLine sol = ListStationsOfLines.Find(sl => sl.LineID == lineID && sl.StationCode == stationCode);
            if (sol != null)
            {
                //List of stations between the previous index and the new index
                List<DO.StationOfLine> solList = ListStationsOfLines.FindAll(sl => sl.LineID == lineID
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
                ListLines.Find(li => li.ID == lineID).FirstStationCode = ListStationsOfLines.Find(s => s.StationIndexInLine == 1).StationCode;
                ListLines.Find(li => li.ID == lineID).LastStationCode = ListStationsOfLines.Find(s => s.StationIndexInLine == GetAllStationsOfLine(lineID).Count()).StationCode;
            }
            else
                throw new DO.BadLineIDStationCodeException(lineID, stationCode, "Worng station of line");

            XMLTools.SaveListToXMLSerializer(ListStationsOfLines, StationOfLinesPath);
            XMLTools.SaveListToXMLSerializer(ListLines, LinesPath);
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
            List<AdjacentStations> ListAdjacentStations = XMLTools.LoadListFromXMLSerializer<AdjacentStations>(AdjacentStationsPath);

            return from adjacentStations in ListAdjacentStations
                   select adjacentStations; //no need to Clone()
        }

        public DO.AdjacentStations GetAdjacentStations(int station1Code, int station2Code)
        {
            List<AdjacentStations> ListAdjacentStations = XMLTools.LoadListFromXMLSerializer<AdjacentStations>(AdjacentStationsPath);
            AdjacentStations adjacentStations = ListAdjacentStations.Find(adjS => (adjS.Station1Code == station1Code && adjS.Station2Code == station2Code) || (adjS.Station2Code == station1Code && adjS.Station1Code == station2Code));
            if (adjacentStations != null)
                return adjacentStations;
            else
                throw new DO.BadStationCodeException(station1Code, "Adjacent stations is not found");
        }
        //public IEnumerable<DO.AdjacentStations> GetMyAdjacentStations(int stationCode)
        //{
        //    return from adjSt in GetAllAdjacentStations()
        //           where adjSt.Station1Code == stationCode || adjSt.Station2Code == stationCode
        //           select adjSt.Clone();
        //}
        public IEnumerable<DO.AdjacentStations> GetMyAdjacentStations(int stationCode)
        {
            List<AdjacentStations> ListAdjacentStations = XMLTools.LoadListFromXMLSerializer<AdjacentStations>(AdjacentStationsPath);
            return from adjSt in ListAdjacentStations
                   where adjSt.Station1Code == stationCode || adjSt.Station2Code == stationCode
                   select adjSt;
        }
        public void AddAdjacentStations(int station1Code, int station2Code)
        {
            List<AdjacentStations> ListAdjacentStations = XMLTools.LoadListFromXMLSerializer<AdjacentStations>(AdjacentStationsPath);
            List<Station> ListStations = XMLTools.LoadListFromXMLSerializer<Station>(StationsPath);
            if (ListStations.FirstOrDefault(sta => (sta.Code == station1Code)) == null)
                throw new DO.BadStationCodeException(station1Code, $"station code {station1Code} is not found");
            if (ListStations.FirstOrDefault(sta => (sta.Code == station2Code)) == null)
                throw new DO.BadStationCodeException(station2Code, $"station code {station2Code} is not found");
            if (ListAdjacentStations.FirstOrDefault(adjSt => (adjSt.Station1Code == station1Code && adjSt.Station2Code == station2Code) || (adjSt.Station1Code == station2Code && adjSt.Station2Code == station1Code)) == null)
            {
                DO.Station station1 = GetStation(station1Code);
                DO.Station station2 = GetStation(station2Code);
                //var sCoord = new GeoCoordinate(station1.Longitude, station1.Latitude);
                //var eCoord = new GeoCoordinate(station2.Longitude, station2.Latitude);
                //var distance = sCoord.GetDistanceTo(eCoord);

                DO.AdjacentStations adjSt = new DO.AdjacentStations()
                {
                    Station1Code = station1Code,
                    Station2Code = station2Code,
                    //Distance = 1.5 * distance,
                    Distance = 500,
                    AvgTime = new TimeSpan(0, 5, 0)
                };
                ListAdjacentStations.Add(adjSt);
                XMLTools.SaveListToXMLSerializer(ListAdjacentStations, AdjacentStationsPath);
            }
        }
        public void DeleteAdjacentStationsByStation(int stationCode)
        {
            List<AdjacentStations> ListAdjacentStations = XMLTools.LoadListFromXMLSerializer<AdjacentStations>(AdjacentStationsPath);
            ListAdjacentStations.RemoveAll(adjS => adjS.Station1Code == stationCode || adjS.Station2Code == stationCode);
            XMLTools.SaveListToXMLSerializer(ListAdjacentStations, AdjacentStationsPath);
        }

        public void UpdateAdjacentStations(DO.Station stationDO)
        {
            List<AdjacentStations> ListAdjacentStations = XMLTools.LoadListFromXMLSerializer<AdjacentStations>(AdjacentStationsPath);
            List<AdjacentStations> myAdjacentStations = ListAdjacentStations.FindAll(adjS => adjS.Station1Code == stationDO.Code || adjS.Station2Code == stationDO.Code);
            ListAdjacentStations.RemoveAll(adjS => adjS.Station1Code == stationDO.Code || adjS.Station2Code == stationDO.Code);
            for (int i = 0; i < myAdjacentStations.Count(); i++)
            {
                AddAdjacentStations(myAdjacentStations[i].Station1Code, myAdjacentStations[i].Station2Code);
            }
            XMLTools.SaveListToXMLSerializer(ListAdjacentStations, AdjacentStationsPath);
        }
        #endregion
    }
}
