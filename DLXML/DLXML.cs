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
        public DO.StationOfLine GetStationOfLine(int lineID, int stationCode)
        {
            List<StationOfLine> ListStationsOfLines = XMLTools.LoadListFromXMLSerializer<StationOfLine>(StationOfLinesPath);
            DO.StationOfLine sol = ListStationsOfLines.Find(sl => sl.LineID == lineID && sl.StationCode == stationCode);
            if (sol != null)
                return sol;
            else
                throw new DO.BadLineIDStationCodeException(lineID, stationCode, "bad station code or line id");
        }
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
