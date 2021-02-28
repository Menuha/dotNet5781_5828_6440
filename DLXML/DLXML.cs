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

        string AdjacentStationsPath = @"AdjacentStationsXml.xml"; //XElement

        string LinesPath = @"LinesXml.xml"; //XMLSerializer
        string StationsPath = @"StationsXml.xml"; //XMLSerializer
        string StationOfLinesPath = @"StationOfLinesXml.xml"; //XMLSerializer

        #endregion

        #region AdjacentStations
        //public void AddAdjacentStations(int station1Code, int station2Code)
        //{

        //    //if (DataSource.ListStationsOfLines.FirstOrDefault(sols => (sols.LineID == station1Code && sols.StationCode == stationCode)) != null)
        //    //    throw new DO.BadLineIDStationCodeException(lineId, stationCode, "line ID is already registered to station code");

        //    if (DataSource.ListAdjacentStations.FirstOrDefault(adjSt => (adjSt.Station1Code == station1Code && adjSt.Station2Code == station2Code)) == null)
        //    {
        //        DO.Station station1 = GetStation(station1Code);
        //        DO.Station station2 = GetStation(station2Code);
        //        var sCoord = new GeoCoordinate(station1.Longitude, station1.Latitude);
        //        var eCoord = new GeoCoordinate(station2.Longitude, station2.Latitude);

        //        var distance = sCoord.GetDistanceTo(eCoord);
        //        DO.AdjacentStations adjSt = new DO.AdjacentStations()
        //        {
        //            Station1Code = station1Code,
        //            Station2Code = station2Code,
        //            Distance = distance
        //            //timeSpan
        //        };

        //        DataSource.ListAdjacentStations.Add(adjSt);
        //    }
        //    //else
        //    //throw 

        //}
        //public void AddPerson(DO.Person person)
        //{
        //    XElement personsRootElem = XMLTools.LoadListFromXMLElement(personsPath);

        //    XElement per1 = (from p in personsRootElem.Elements()
        //                     where int.Parse(p.Element("ID").Value) == person.ID
        //                     select p).FirstOrDefault();

        //    if (per1 != null)
        //        throw new DO.BadPersonIdException(person.ID, "Duplicate person ID");

        //    XElement personElem = new XElement("Person", new XElement("ID", person.ID),
        //                          new XElement("Name", person.Name),
        //                          new XElement("Street", person.Street),
        //                          new XElement("HouseNumber", person.HouseNumber.ToString()),
        //                          new XElement("City", person.City),
        //                          new XElement("BirthDate", person.BirthDate),
        //                          new XElement("PersonalStatus", person.PersonalStatus.ToString()),
        //                          new XElement("Duration", person.Duration.ToString()));

        //    personsRootElem.Add(personElem);

        //    XMLTools.SaveListToXMLElement(personsRootElem, personsPath);
        //}
        public IEnumerable<DO.AdjacentStations> GetAllAdjacentStations()
        {
            XElement AdjacentStationsRootElem = XMLTools.LoadListFromXMLElement(AdjacentStationsPath);

            return (from a in AdjacentStationsRootElem.Elements()
                    select new AdjacentStations()
                    {
                        Station1Code = Int32.Parse(a.Element("Station 1 Code").Value),
                        Station2Code = Int32.Parse(a.Element("Station 2 Code").Value),
                        Distance = double.Parse(a.Element("Distance").Value),
                        AvgTime = TimeSpan.ParseExact(a.Element("Avarage Time").Value, "hh\\:mm\\:ss", CultureInfo.InvariantCulture)
                    }
                   );
        }
        #endregion


        #region Line
        public DO.Line GetLine(int id)
        {
            List<Line> ListLines = XMLTools.LoadListFromXMLSerializer<Line>(LinesPath);

            DO.Line line = ListLines.Find(l => l.ID == id);
            if (line != null)
                return line; //no need to Clone()
            else
                throw new DO.BadLineIDException(id, $"bad line id: {id}");
        }
        public void AddLine(DO.Line line)
        {
            List<Line> ListLines = XMLTools.LoadListFromXMLSerializer<Line>(LinesPath);

            if (ListLines.FirstOrDefault(s => s.ID == line.ID) != null)
                throw new DO.BadLineIDException(line.ID, "Duplicate line id");

            //if (GetPerson(student.ID) == null)
            //    throw new DO.BadPersonIdException(student.ID, "Missing person ID");

            ListLines.Add(line); //no need to Clone()

            XMLTools.SaveListToXMLSerializer(ListLines, LinesPath);

        }
        public IEnumerable<DO.Line> GetAllLines()
        {
            List<Line> ListLines = XMLTools.LoadListFromXMLSerializer<Line>(LinesPath);

            return from line in ListLines
                   select line; //no need to Clone()
        }
        //public IEnumerable<object> GetStudentFields(Func<int, string, object> generate)
        //{
        //    List<Student> ListStudents = XMLTools.LoadListFromXMLSerializer<Student>(studentsPath);

        //    return from student in ListStudents
        //           select generate(student.ID, GetPerson(student.ID).Name);
        //}

        //public IEnumerable<object> GetStudentListWithSelectedFields(Func<DO.Student, object> generate)
        //{
        //    List<Student> ListStudents = XMLTools.LoadListFromXMLSerializer<Student>(studentsPath);

        //    return from student in ListStudents
        //           select generate(student);
        //}
        public void UpdateLine(DO.Line line)
        {
            List<Line> ListLines = XMLTools.LoadListFromXMLSerializer<Line>(LinesPath);

            DO.Line li = ListLines.Find(p => p.ID == line.ID);
            if (li != null)
            {
                ListLines.Remove(li);
                ListLines.Add(line); //no nee to Clone()
            }
            else
                throw new DO.BadLineIDException(line.ID, $"bad line id: {line.ID}");

            XMLTools.SaveListToXMLSerializer(ListLines, LinesPath);
        }
        public void DeleteLine(int id)
        {
            List<Line> ListLines = XMLTools.LoadListFromXMLSerializer<Line>(LinesPath);

            DO.Line line = ListLines.Find(p => p.ID == id);

            if (line != null)
            {
                ListLines.Remove(line);
            }
            else
                throw new DO.BadLineIDException(id, $"bad line id: {id}");

            XMLTools.SaveListToXMLSerializer(ListLines, LinesPath);
        }
        #endregion Student

        #region Stations

        //public IEnumerable<DO.Station> GetSortStations()
        //{
        //    List<Station> ListStations = XMLTools.LoadListFromXMLSerializer<Station>(stationsPath);
        //    return from Station in ListStations
        //           orderby Station.Code
        //           select Station;

        //}

        public IEnumerable<DO.Station> GetAllStations()
        {
            List<Station> ListStations = XMLTools.LoadListFromXMLSerializer<Station>(StationsPath);

            return from station in ListStations
                   select station; //no need to Clone()
        }
        public DO.Station GetStation(int code)
        {
            List<Station> ListStations = XMLTools.LoadListFromXMLSerializer<Station>(StationsPath);

            DO.Station sta = ListStations.Find(l => l.Code == code);
            if (sta != null)
                return sta; //no need to Clone()
            else
                throw new DO.BadLineIDException(code, $"bad station code: {code}");


            //return ListStations.Find(s => s.Code == Code); //no need to Clone()
            //if not exist throw exception etc.
        }
        public void AddStation(DO.Station station)
        {
            List<Station> ListStations = XMLTools.LoadListFromXMLSerializer<Station>(StationsPath);
            if (ListStations.FirstOrDefault(s => s.Code == station.Code) != null)
                throw new DO.BadStationCodeException(station.Code, "Duplicate station code");

            ListStations.Add(station); //no need to Clone()

            XMLTools.SaveListToXMLSerializer(ListStations, StationsPath);
        }
        public void DeleteStation(int code)
        {
            List<Station> ListStations = XMLTools.LoadListFromXMLSerializer<Station>(StationsPath);
            DO.Station sta = ListStations.Find(l => l.Code == code);

            if (sta != null)
            {
                ListStations.Remove(sta);
            }
            else
                throw new DO.BadStationCodeException(code, $"bad station code: {code}");

            XMLTools.SaveListToXMLSerializer(ListStations, StationsPath);
        }
        public void UpdateStation(DO.Station station)
        {
            List<Station> ListStations = XMLTools.LoadListFromXMLSerializer<Station>(StationsPath);
            DO.Station sta = ListStations.Find(s => s.Code == station.Code);
            if (sta != null)
            {
                ListStations.Remove(sta);
                ListStations.Add(station); //no nee to Clone()
            }
            else
                throw new DO.BadStationCodeException(station.Code, $"bad station code: {station.Code}");

            XMLTools.SaveListToXMLSerializer(ListStations, StationsPath);
        }
        #endregion

        #region StationOfLine
        public IEnumerable<DO.StationOfLine> GetAllStationsOfLinesBy (Predicate<DO.StationOfLine> predicate)
        {
            List<StationOfLine> ListStationOfLines = XMLTools.LoadListFromXMLSerializer<StationOfLine>(StationOfLinesPath);
            return from stationOfLine in ListStationOfLines
                   where predicate(stationOfLine)
                   select stationOfLine;
        }
        public IEnumerable<DO.StationOfLine> GetAllStationsOfLine(int lineID)
        {
            List<StationOfLine> ListStationOfLines = XMLTools.LoadListFromXMLSerializer<StationOfLine>(StationOfLinesPath);

            return from linestation in ListStationOfLines
                   where linestation.LineID == lineID
                   select linestation; //no need to Clone()
        }
        public void UpdateStationOfLine(DO.StationOfLine stationOfLine)
        {
            List<StationOfLine> ListStationOfLines = XMLTools.LoadListFromXMLSerializer<StationOfLine>(StationOfLinesPath);
            DO.StationOfLine sol = ListStationOfLines.Find(l => l.LineID == stationOfLine.LineID);
            if (sol != null)
            {
                ListStationOfLines.Remove(sol);
                ListStationOfLines.Add(stationOfLine); //no nee to Clone()
            }
            else
                throw new DO.BadLineIDStationCodeException(stationOfLine.LineID, stationOfLine.StationCode, "Worng station of line");
            XMLTools.SaveListToXMLSerializer(ListStationOfLines, StationOfLinesPath);
        }
        public void AddStationOfLine(int lineId, int stationCode)
        {
            List<StationOfLine> ListStationOfLines = XMLTools.LoadListFromXMLSerializer<StationOfLine>(StationOfLinesPath);
                if (ListStationOfLines.FirstOrDefault(sols => (sols.LineID == lineId && sols.StationCode == stationCode)) != null)
                    throw new DO.BadLineIDStationCodeException(lineId, stationCode, "line ID is already registered to station code");

            int index = ListStationOfLines.FindAll(sols => sols.LineID == lineId).Count() + 1;
                DO.StationOfLine sol = new DO.StationOfLine() { LineID = lineId, StationCode = stationCode, StationIndexInLine = index };
                ListStationOfLines.Add(sol); //no need to Clone()

            XMLTools.SaveListToXMLSerializer(ListStationOfLines, StationOfLinesPath);
        }
        public void DeleteStationOfLine(int lineID, int stationCode)
        {
            List<StationOfLine> ListStationOfLines = XMLTools.LoadListFromXMLSerializer<StationOfLine>(StationOfLinesPath);

            DO.StationOfLine sic = ListStationOfLines.Find(sol => (sol.LineID == lineID && sol.StationCode == stationCode));

            if (sic != null)
            {
                ListStationOfLines.Remove(sic);
            }
            else
                throw new DO.BadLineIDStationCodeException(lineID, stationCode, "Worng line id or station code");

            XMLTools.SaveListToXMLSerializer(ListStationOfLines, StationOfLinesPath);
        }
        public void DeleteSolByLine(int lineID)
        {
            List<StationOfLine> ListStationOfLines = XMLTools.LoadListFromXMLSerializer<StationOfLine>(StationOfLinesPath);

            ListStationOfLines.RemoveAll(p => p.LineID == lineID);

            XMLTools.SaveListToXMLSerializer(ListStationOfLines, StationOfLinesPath);

        }
        public void UpdateStationIndexInLine(int lineID, int stationCode, int newIndex)
        { 
            List<StationOfLine> ListStationOfLines = XMLTools.LoadListFromXMLSerializer<StationOfLine>(StationOfLinesPath);

            DO.StationOfLine sol = ListStationOfLines.Find(cis => (cis.LineID == lineID && cis.StationCode == stationCode));

            if (sol != null)
            {
                sol.StationIndexInLine = newIndex;
            }
            else
                throw new DO.BadLineIDStationCodeException(lineID, stationCode, "Worng station of line");

            XMLTools.SaveListToXMLSerializer(ListStationOfLines, StationOfLinesPath);
        }

        #endregion

    }
}
