using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using DLAPI;
using DO;

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

        string stationsPath = @"StationsXml.xml"; //XElement
        string linesPath = @"LinesXml.xml"; //XMLSerializer
        string stationsOfLinesPath = @"StationsOfLinesXml.xml"; //XMLSerializer
        string adjacentStationsPath = @"AdjacentStationsXml.xml"; //XElement
        string linesTripsPath = @"LinesTripsXml.xml"; //XElement

        #endregion

        #region Station
        public IEnumerable<DO.Station> GetAllStations()
        {
            XElement stationsRootElem = XMLTools.LoadListFromXMLElement(stationsPath);

            return (from sta in stationsRootElem.Elements()
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
            XElement stationsRootElem = XMLTools.LoadListFromXMLElement(stationsPath);

            Station s = (from sta in stationsRootElem.Elements()
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
            XElement stationsRootElem = XMLTools.LoadListFromXMLElement(stationsPath);

            XElement sta = (from s in stationsRootElem.Elements()
                             where int.Parse(s.Element("ID").Value) == station.Code
                             select s).FirstOrDefault();

            if (sta != null)
                throw new DO.BadStationCodeException(station.Code, "Duplicate station code");

            XElement stationElem = new XElement("Station", new XElement("Code", station.Code),
                                  new XElement("Name", station.Name),
                                  new XElement("Latitude", station.Latitude),
                                  new XElement("Longitude", station.Longitude));

            stationsRootElem.Add(stationElem);

            XMLTools.SaveListToXMLElement(stationsRootElem, stationsPath);
        }
       
        public void DeleteStation(int code)
        {
            XElement stationsRootElem = XMLTools.LoadListFromXMLElement(stationsPath);
            XElement stationsOfLinesRootElem = XMLTools.LoadListFromXMLElement(stationsOfLinesPath);


            XElement sta = (from s in stationsRootElem.Elements()
                            where int.Parse(s.Element("Code").Value) == code
                            select s).FirstOrDefault();
            XElement sol = (from so in stationsOfLinesRootElem.Elements()
                            where int.Parse(so.Element("StationCode").Value) == code
                            select so).FirstOrDefault();

            if (sta != null && sol == null)
            {
                sta.Remove(); //<==>   Remove sta from stationsRootElem

                XMLTools.SaveListToXMLElement(stationsRootElem, stationsPath);
            }
            else if (sol != null)
                throw new DO.BadStationCodeException(code, $"There is a line passing through the station with code: {code}");
            else
                throw new DO.BadStationCodeException(code, $"bad station code: {code}");
        }

        public void UpdateStation(DO.Station station)
        {

            XElement stationsRootElem = XMLTools.LoadListFromXMLElement(stationsPath);

            XElement sta = (from s in stationsRootElem.Elements()
                            where int.Parse(s.Element("Code").Value) == station.Code
                            select s).FirstOrDefault();

            if (sta != null)
            {
                sta.Element("Code").Value = station.Code.ToString();
                sta.Element("Name").Value = station.Name;
                sta.Element("Latitude").Value = station.Latitude.ToString();
                sta.Element("Longitude").Value = station.Longitude.ToString();


                XMLTools.SaveListToXMLElement(stationsRootElem, stationsPath);
            }
            else
                throw new DO.BadStationCodeException(station.Code, $"bad station code: {station.Code}");
        }
        #endregion

        #region Line
        public IEnumerable<DO.Line> GetAllLines()
        {
            List<Line> ListLines = XMLTools.LoadListFromXMLSerializer<Line>(linesPath);
            return from line in ListLines
                   select line;
        }

        public IEnumerable<DO.Line> GetAllLinesBy(Predicate<DO.Line> predicate)
        {
            List<Line> ListLines = XMLTools.LoadListFromXMLSerializer<Line>(linesPath);
            return from li in ListLines
                   where predicate(li)
                   select li;
        }

        public DO.Line GetLine(int id)
        {
            List<Line> ListLines = XMLTools.LoadListFromXMLSerializer<Line>(linesPath);
            DO.Line li = ListLines.Find(l => l.ID == id);

            if (li != null)
                return li;
            else
                throw new DO.BadLineIDException(id, $"bad line id: {id}");
        }

        public int AddLine(int number, DO.Areas area, int firstStationCode, int lastStationCode)
        {
            DO.Station station = GetStation(firstStationCode);
            if (station == null)
                throw new DO.BadStationCodeException(firstStationCode, $"Station code {firstStationCode} does not exist");
            station = GetStation(lastStationCode);
            if (station == null)
                throw new DO.BadStationCodeException(lastStationCode, $"Station code {lastStationCode} does not exist");

            List<Line> ListLines = XMLTools.LoadListFromXMLSerializer<Line>(linesPath);
            DO.Line line = ListLines.FirstOrDefault(l => l.Number == number && l.Area == area);
            if (line != null)
                throw new DO.BadLineIDException(line.ID, "Duplicate line id");
            line = new DO.Line()
            {
                ID = ++DO.Config.LineID,
                Number = number,
                Area = area,
                FirstStationCode = firstStationCode,
                LastStationCode = lastStationCode
            };
            ListLines.Add(line);

            XMLTools.SaveListToXMLSerializer(ListLines, linesPath);

            return line.ID;
        }

        public void DeleteLine(int id)
        {
            List<Line> ListLines = XMLTools.LoadListFromXMLSerializer<Line>(linesPath);
            DO.Line li = ListLines.Find(l => l.ID == id);

            if (li != null)
            {
                ListLines.Remove(li);
            }
            else
                throw new DO.BadLineIDException(id, $"bad line id: {id}");

            XMLTools.SaveListToXMLSerializer(ListLines, linesPath);
        }

        public void UpdateLine(DO.Line line)
        {
            List<Line> ListLines = XMLTools.LoadListFromXMLSerializer<Line>(linesPath);
            DO.Line li = ListLines.Find(l => l.ID == line.ID);
            if (li != null)
            {
                ListLines.Remove(li);
                ListLines.Add(line);
            }
            else
                throw new DO.BadLineIDException(line.ID, $"bad line id: {line.ID}");

            XMLTools.SaveListToXMLSerializer(ListLines, linesPath);
        }
        #endregion

        #region StationOfLine
        public IEnumerable<DO.StationOfLine> GetAllStationsOfLinesBy(Predicate<DO.StationOfLine> predicate)
        {
            List<StationOfLine> ListStationsOfLines = XMLTools.LoadListFromXMLSerializer<StationOfLine>(stationsOfLinesPath);
            return from sol in ListStationsOfLines
                   where predicate(sol)
                   select sol;
        }

        public IEnumerable<DO.StationOfLine> GetAllStationsOfLine(int lineID)
        {
            List<StationOfLine> ListStationsOfLines = XMLTools.LoadListFromXMLSerializer<StationOfLine>(stationsOfLinesPath);
            return from sol in ListStationsOfLines
                   where sol.LineID == lineID
                   select sol;
        }

        public DO.StationOfLine GetStationOfLine(int lineID, int stationCode)
        {
            List<StationOfLine> ListStationsOfLines = XMLTools.LoadListFromXMLSerializer<StationOfLine>(stationsOfLinesPath);
            DO.StationOfLine sol = ListStationsOfLines.Find(sl => sl.LineID == lineID && sl.StationCode == stationCode);
            if (sol != null)
                return sol;
            else
                throw new DO.BadLineIDStationCodeException(lineID, stationCode, "bad station code or line id");
        }

        public DO.StationOfLine GetPrevSol(int lineID, int stationCode)
        {
            int myIndex = GetStationOfLine(lineID, stationCode).StationIndexInLine;
            DO.StationOfLine prevSol = GetAllStationsOfLine(lineID).FirstOrDefault(sol => sol.StationIndexInLine == myIndex - 1);
            if (prevSol != null)
                return prevSol;
            else
                throw new DO.BadLineIDStationCodeException(lineID, stationCode, "this is the first station of this line");
        }

        public DO.StationOfLine GetNextSol(int lineID, int stationCode)
        {
            int myIndex = GetStationOfLine(lineID, stationCode).StationIndexInLine;
            DO.StationOfLine prevSol = GetAllStationsOfLine(lineID).FirstOrDefault(sol => sol.StationIndexInLine == myIndex + 1);
            if (prevSol != null)
                return prevSol;
            else
                throw new DO.BadLineIDStationCodeException(lineID, stationCode, "this is the last station of this line");
        }

        public void AddStationOfLine(int lineID, int stationCode)
        {
            List<StationOfLine> ListStationsOfLines = XMLTools.LoadListFromXMLSerializer<StationOfLine>(stationsOfLinesPath);
            List<Line> ListLines = XMLTools.LoadListFromXMLSerializer<Line>(linesPath);
            if (ListStationsOfLines.FirstOrDefault(sols => (sols.LineID == lineID && sols.StationCode == stationCode)) != null)
                throw new DO.BadLineIDStationCodeException(lineID, stationCode, $"line ID {lineID} is already registered to station code {stationCode}");
            int index = ListStationsOfLines.FindAll(sols => sols.LineID == lineID).Count() + 1;
            DO.StationOfLine sol = new DO.StationOfLine() { LineID = lineID, StationCode = stationCode, StationIndexInLine = index };
            ListStationsOfLines.Add(sol);
            ListLines.Find(li => li.ID == lineID).LastStationCode = stationCode;
            XMLTools.SaveListToXMLSerializer(ListLines, linesPath);
            XMLTools.SaveListToXMLSerializer(ListStationsOfLines, stationsOfLinesPath);
        }

        public void DeleteStationOfLine(int lineID, int stationCode)
        {
            List<StationOfLine> ListStationsOfLines = XMLTools.LoadListFromXMLSerializer<StationOfLine>(stationsOfLinesPath);
            List<Line> ListLines = XMLTools.LoadListFromXMLSerializer<Line>(linesPath);
            DO.StationOfLine sol = ListStationsOfLines.Find(sl => sl.LineID == lineID && sl.StationCode == stationCode);
            int routeLength = GetAllStationsOfLine(lineID).Count();
            if (routeLength == 2)
                throw new DO.BadLineIDStationCodeException(lineID, stationCode, "Line route should have at least 2 stations");
            if (sol != null)
            {
                UpdateStationIndexInLine(lineID, stationCode, routeLength);
                ListStationsOfLines.Remove(sol);
                //Update last station of this line
                ListLines.Find(li => li.ID == lineID).LastStationCode = ListStationsOfLines.Find(s => s.StationIndexInLine == GetAllStationsOfLine(lineID).Count() - 1).StationCode;

                XMLTools.SaveListToXMLSerializer(ListStationsOfLines, stationsOfLinesPath);
                XMLTools.SaveListToXMLSerializer(ListLines, linesPath);
            }
            else
                throw new DO.BadLineIDStationCodeException(lineID, stationCode, "Worng line id or station code");
        }

        public void DeleteSolByLine(int lineID)
        {
            List<StationOfLine> ListStationsOfLines = XMLTools.LoadListFromXMLSerializer<StationOfLine>(stationsOfLinesPath);
            ListStationsOfLines.RemoveAll(sol => sol.LineID == lineID);
            XMLTools.SaveListToXMLSerializer(ListStationsOfLines, stationsOfLinesPath);
        }

        public void UpdateStationIndexInLine(int lineID, int stationCode, int newIndex)
        {
            List<StationOfLine> ListStationsOfLines = XMLTools.LoadListFromXMLSerializer<StationOfLine>(stationsOfLinesPath);
            List<Line> ListLines = XMLTools.LoadListFromXMLSerializer<Line>(linesPath);
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

            XMLTools.SaveListToXMLSerializer(ListStationsOfLines, stationsOfLinesPath);
            XMLTools.SaveListToXMLSerializer(ListLines, linesPath);
        }
        #endregion

        #region AdjacentStations
        public IEnumerable<DO.AdjacentStations> GetAllAdjacentStations()
        {
            //List<AdjacentStations> ListAdjacentStations = XMLTools.LoadListFromXMLSerializer<AdjacentStations>(AdjacentStationsPath);

            //return from adjacentStations in ListAdjacentStations
            //       select adjacentStations; //no need to Clone()

            XElement adjacentStationsRootElem = XMLTools.LoadListFromXMLElement(adjacentStationsPath);

            return (from adjS in adjacentStationsRootElem.Elements()
                    select new AdjacentStations()
                    {
                        Station1Code = Int32.Parse(adjS.Element("Station1Code").Value),
                        Station2Code = Int32.Parse(adjS.Element("Station2Code").Value),
                        Distance = Double.Parse(adjS.Element("Distance").Value),
                        AvgTime = TimeSpan.ParseExact(adjS.Element("AvgTime").Value, "hh\\:mm\\:ss", CultureInfo.InvariantCulture)
                    }
                   );
        }
      
        public DO.AdjacentStations GetAdjacentStations(int station1Code, int station2Code)
        {
            //List<AdjacentStations> ListAdjacentStations = XMLTools.LoadListFromXMLSerializer<AdjacentStations>(AdjacentStationsPath);
            //AdjacentStations adjacentStations = ListAdjacentStations.Find(adjS => (adjS.Station1Code == station1Code && adjS.Station2Code == station2Code) || (adjS.Station2Code == station1Code && adjS.Station1Code == station2Code));
            //if (adjacentStations != null)
            //    return adjacentStations;
            //else
            //    throw new DO.BadStationCodeException(station1Code, "Adjacent stations is not found");

            XElement adjacentStationsRootElem = XMLTools.LoadListFromXMLElement(adjacentStationsPath);

            AdjacentStations adjacentStations = (from adjS in adjacentStationsRootElem.Elements()
                                                 where (int.Parse(adjS.Element("Station1Code").Value) == station1Code && 
                                                        int.Parse(adjS.Element("Station2Code").Value) == station2Code) || 
                                                       (int.Parse(adjS.Element("Station2Code").Value) == station1Code && 
                                                        int.Parse(adjS.Element("Station1Code").Value) == station2Code)
                                                 select new AdjacentStations()
                                                 {
                                                     Station1Code = Int32.Parse(adjS.Element("Station1Code").Value),
                                                     Station2Code = Int32.Parse(adjS.Element("Station2Code").Value),
                                                     Distance = Double.Parse(adjS.Element("Distance").Value),
                                                     AvgTime = TimeSpan.ParseExact(adjS.Element("AvgTime").Value, "hh\\:mm\\:ss", CultureInfo.InvariantCulture)
                                                 }
                                                ).FirstOrDefault();

            if (adjacentStations != null)
                return adjacentStations;
            else
                throw new DO.BadStationCodeException(station1Code, "Adjacent stations is not found");
        }

        public IEnumerable<DO.AdjacentStations> GetMyAdjacentStations(int stationCode)
        {
            //List<AdjacentStations> ListAdjacentStations = XMLTools.LoadListFromXMLSerializer<AdjacentStations>(AdjacentStationsPath);
            //return from adjSt in ListAdjacentStations
            //       where adjSt.Station1Code == stationCode || adjSt.Station2Code == stationCode
            //       select adjSt;

            XElement adjacentStationsRootElem = XMLTools.LoadListFromXMLElement(adjacentStationsPath);

            return (from adjS in adjacentStationsRootElem.Elements()
                    where int.Parse(adjS.Element("Station1Code").Value) == stationCode || 
                          int.Parse(adjS.Element("Station2Code").Value) == stationCode
                    select new AdjacentStations()
                    {
                        Station1Code = Int32.Parse(adjS.Element("Station1Code").Value),
                        Station2Code = Int32.Parse(adjS.Element("Station2Code").Value),
                        Distance = Double.Parse(adjS.Element("Distance").Value),
                        AvgTime = TimeSpan.ParseExact(adjS.Element("AvgTime").Value, "hh\\:mm\\:ss", CultureInfo.InvariantCulture)
                    }
                   );
        }
       
        public void AddAdjacentStations(int station1Code, int station2Code)
        {
            //List<AdjacentStations> ListAdjacentStations = XMLTools.LoadListFromXMLSerializer<AdjacentStations>(AdjacentStationsPath);
            //List<Station> ListStations = XMLTools.LoadListFromXMLSerializer<Station>(StationsPath);
            //if (ListStations.FirstOrDefault(sta => (sta.Code == station1Code)) == null)
            //    throw new DO.BadStationCodeException(station1Code, $"station code {station1Code} is not found");
            //if (ListStations.FirstOrDefault(sta => (sta.Code == station2Code)) == null)
            //    throw new DO.BadStationCodeException(station2Code, $"station code {station2Code} is not found");
            //if (ListAdjacentStations.FirstOrDefault(adjSt => (adjSt.Station1Code == station1Code && adjSt.Station2Code == station2Code) || (adjSt.Station1Code == station2Code && adjSt.Station2Code == station1Code)) == null)
            //{
            //    DO.Station station1 = GetStation(station1Code);
            //    DO.Station station2 = GetStation(station2Code);
            //    var sCoord = new GeoCoordinate(station1.Longitude, station1.Latitude);
            //    var eCoord = new GeoCoordinate(station2.Longitude, station2.Latitude);
            //    var distance = sCoord.GetDistanceTo(eCoord);

            //    DO.AdjacentStations adjSt = new DO.AdjacentStations()
            //    {
            //        Station1Code = station1Code,
            //        Station2Code = station2Code,
            //        Distance = 1.5 * distance,
            //        //speed = 10 m/s
            //        AvgTime = new TimeSpan(0, 0, Convert.ToInt32(1.5 * distance / 10))
            //    };
            //    ListAdjacentStations.Add(adjSt);
            //    XMLTools.SaveListToXMLSerializer(ListAdjacentStations, AdjacentStationsPath);

            XElement stationsRootElem = XMLTools.LoadListFromXMLElement(stationsPath);
         
            XElement station1 = (from s1 in stationsRootElem.Elements()
                                 where int.Parse(s1.Element("Code").Value) == station1Code
                                 select s1).FirstOrDefault();
            if (station1 == null)
                throw new DO.BadStationCodeException(station1Code, $"station code {station1Code} is not found");

            XElement station2 = (from s2 in stationsRootElem.Elements()
                                 where int.Parse(s2.Element("Code").Value) == station2Code
                                 select s2).FirstOrDefault();
            if (station2 == null)
                throw new DO.BadStationCodeException(station1Code, $"station code {station2Code} is not found");

            XElement adjacentStationsRootElem = XMLTools.LoadListFromXMLElement(adjacentStationsPath);

            XElement adjacentStations = (from adjS in adjacentStationsRootElem.Elements()
                                         where (int.Parse(adjS.Element("Station1Code").Value) == station1Code && 
                                                int.Parse(adjS.Element("Station2Code").Value) == station2Code) || 
                                               (int.Parse(adjS.Element("Station2Code").Value) == station1Code && 
                                                int.Parse(adjS.Element("Station1Code").Value) == station2Code)
                                         select adjS).FirstOrDefault();
            if (adjacentStations == null)
            {
                var sCoord = new GeoCoordinate(float.Parse(station1.Element("Longitude").Value), float.Parse(station1.Element("Latitude").Value));
                var eCoord = new GeoCoordinate(float.Parse(station2.Element("Longitude").Value), float.Parse(station2.Element("Latitude").Value));
                var distance = sCoord.GetDistanceTo(eCoord) * 1.5;

                XElement adjElem = new XElement("AdjacentStations", new XElement("Station1Code", station1Code),
                                      new XElement("Station2Code", station2Code),
                                      new XElement("Distance", distance),
                                      //speed = 10 m/s => time = distance/speed
                                      new XElement("AvgTime", (new TimeSpan(0, 0, Convert.ToInt32(distance / 10))).ToString()));

                adjacentStationsRootElem.Add(adjElem);

                XMLTools.SaveListToXMLElement(adjacentStationsRootElem, adjacentStationsPath);
            }
        }
        
        public void DeleteAdjacentStationsByStation(int stationCode)
        {
            //List<AdjacentStations> ListAdjacentStations = XMLTools.LoadListFromXMLSerializer<AdjacentStations>(adjacentStationsPath);
            //ListAdjacentStations.RemoveAll(adjS => adjS.Station1Code == stationCode || adjS.Station2Code == stationCode);
            //XMLTools.SaveListToXMLSerializer(ListAdjacentStations, adjacentStationsPath);

            XElement adjacentStationsRootElem = XMLTools.LoadListFromXMLElement(adjacentStationsPath);
            XElement adjS = null;
            do
            {
                if (adjS != null)
                {
                    adjS.Remove(); //<==>   Remove adj from adjacentStationsRootElem
                }

                adjS = (from adj in adjacentStationsRootElem.Elements()
                                 where int.Parse(adj.Element("Station1Code").Value) == stationCode ||
                                         int.Parse(adj.Element("Station2Code").Value) == stationCode
                                 select adj).FirstOrDefault();
            } while (adjS != null);
            
            XMLTools.SaveListToXMLElement(adjacentStationsRootElem, adjacentStationsPath);
        }

        public void UpdateAdjacentStations(DO.Station stationDO)
        {
            //List<AdjacentStations> ListAdjacentStations = XMLTools.LoadListFromXMLSerializer<AdjacentStations>(adjacentStationsPath);
            //List<AdjacentStations> myAdjacentStations = GetMyAdjacentStations(stationDO.Code).ToList();
            //ListAdjacentStations.RemoveAll(adjS => adjS.Station1Code == stationDO.Code || adjS.Station2Code == stationDO.Code);
            //for (int i = 0; i < myAdjacentStations.Count(); i++)
            //{
            //    AddAdjacentStations(myAdjacentStations[i].Station1Code, myAdjacentStations[i].Station2Code);
            //}
            //XMLTools.SaveListToXMLSerializer(ListAdjacentStations, adjacentStationsPath);

            XElement adjacentStationsRootElem = XMLTools.LoadListFromXMLElement(adjacentStationsPath);
            List<AdjacentStations> myAdjacentStations = GetMyAdjacentStations(stationDO.Code).ToList();
            XElement adjS = null;
            do
            {
                if (adjS != null)
                {
                    adjS.Remove(); //<==>   Remove adj from adjacentStationsRootElem
                }

                adjS = (from adj in adjacentStationsRootElem.Elements()
                        where int.Parse(adj.Element("Station1Code").Value) == stationDO.Code ||
                                int.Parse(adj.Element("Station2Code").Value) == stationDO.Code
                        select adj).FirstOrDefault();
            } while (adjS != null);
           
            XMLTools.SaveListToXMLElement(adjacentStationsRootElem, adjacentStationsPath);

            for (int i = 0; i < myAdjacentStations.Count(); i++)
            {
                AddAdjacentStations(myAdjacentStations[i].Station1Code, myAdjacentStations[i].Station2Code);
            }
        }
        #endregion

        #region LineTrip
        public IEnumerable<DO.LineTrip> GetAllLineTrips(int lineID)
        {
            XElement linesTripsRootElem = XMLTools.LoadListFromXMLElement(linesTripsPath);

            return (from lt in linesTripsRootElem.Elements()
                    where int.Parse(lt.Element("LineID").Value) == lineID
                    select new LineTrip()
                    {
                        LineTripID = Int32.Parse(lt.Element("LineTripID").Value),
                        LineID = Int32.Parse(lt.Element("LineID").Value),
                        StartAt = TimeSpan.ParseExact(lt.Element("StartAt").Value, "hh\\:mm\\:ss", CultureInfo.InvariantCulture)
                    }
                   );
        }

        public int AddLineTrip(int lineID, TimeSpan startAt)
        {
            XElement linesRootElem = XMLTools.LoadListFromXMLElement(linesPath);
            XElement line = (from li in linesRootElem.Elements()
                             where int.Parse(li.Element("ID").Value) == lineID
                             select li).FirstOrDefault();
            if(line == null)
                throw new DO.BadLineIDException(lineID, $"Line ID {lineID} does not exist");

            XElement linesTripsRootElem = XMLTools.LoadListFromXMLElement(linesTripsPath);

            XElement lineTrip = (from lt in linesTripsRootElem.Elements()
                                 where int.Parse(lt.Element("LineID").Value) == lineID &&
                                       TimeSpan.Parse(lt.Element("StartAt").Value) == startAt
                                 select lt).FirstOrDefault();
            if (lineTrip != null)
                throw new DO.BadLineIDException(lineID, "Duplicate line trip");

            lineTrip = new XElement("LineTrip", new XElement("LineTripID", ++DO.Config.LineTripID),
                                  new XElement("LineID", lineID),
                                  new XElement("StartAt", startAt));

            linesTripsRootElem.Add(lineTrip);

            XMLTools.SaveListToXMLElement(linesTripsRootElem, linesTripsPath);

            return DO.Config.LineTripID;
        }

        public void DeleteLineTrip(int lineTripID)
        {
            XElement linesTripsRootElem = XMLTools.LoadListFromXMLElement(linesTripsPath);

            XElement lineTrip = (from lt in linesTripsRootElem.Elements()
                                 where int.Parse(lt.Element("LineTripID").Value) == lineTripID
                                 select lt).FirstOrDefault();
            if (lineTrip != null)
            {
                lineTrip.Remove(); //<==>   Remove lineTrip from linesTripsRootElem
                XMLTools.SaveListToXMLElement(linesTripsRootElem, linesTripsPath);
            }
            else
                throw new DO.BadLineIDException(lineTripID, "Line trip does not exist");
        }

        //public IEnumerable<DO.LineTrip> GetAllLinesTrips()
        //{
        //    throw new NotImplementedException();
        //}

        //public IEnumerable<DO.LineTrip> GetAllLinesTripsBy(Predicate<DO.LineTrip> predicate)
        //{
        //    List<LineTrip> ListLinesTrips = XMLTools.LoadListFromXMLSerializer<LineTrip>(linesTripsPath);
        //    return from lt in ListLinesTrips
        //           where predicate(lt)
        //           select lt;
        //}

        //public void UpdateLineTrip(DO.LineTrip lineTrip)
        //{
        //    throw new NotImplementedException();
        //}
        //public void DeleteLineTrip(int id)
        //{
        //    throw new NotImplementedException();
        //}
        #endregion

    }
}
