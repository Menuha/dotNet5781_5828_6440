﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DLAPI;
using BLAPI;

namespace BL
{
    internal sealed class BLImp : IBL
    {
        #region singelton
        static readonly BLImp instance = new BLImp();
        static BLImp() { }// static ctor to ensure instance init is done just before first usage
        BLImp() { } // default => private
        public static BLImp Instance { get => instance; }// The public Instance property to use
        #endregion

        IDL dl = DLFactory.GetDL();

        #region Line
        BO.Line lineDoBoAdapter(DO.Line lineDO)
        {
            BO.Line lineBO = new BO.Line();
            lineDO.CopyPropertiesTo(lineBO);

            lineBO.StationsInLine = from sol in dl.GetAllStationsOfLine(lineDO.ID)
                                    select stationOfLineDoBoAdapter(sol);

            return lineBO;
        }

        public BO.Line GetLine(int id)
        {
            DO.Line lineDO;
            try
            {
                lineDO = dl.GetLine(id);
            }
            catch (DO.BadLineIDException ex)
            {
                throw new BO.BadLineIDException("Line id does not exist", ex);
            }
            return lineDoBoAdapter(lineDO);
        }

        public IEnumerable<BO.Line> GetAllLines()
        {
            return from lineDO in dl.GetAllLines()
                   orderby lineDO.ID
                   select lineDoBoAdapter(lineDO);
        }

        public int AddLine(int number, BO.Areas newArea, int firstStationCode, int lastStationCode)
        {         
            try
            {
                int lineID = dl.AddLine(number, (DO.Areas)newArea, firstStationCode, lastStationCode);

                AddAdjacentStations(firstStationCode, lastStationCode);

                AddStationOfLine(lineID, firstStationCode);
                AddStationOfLine(lineID, lastStationCode);

                return lineID;
            }
            catch (DO.BadStationCodeException ex)
            {
                throw new BO.BadStationCodeException("Station code is illegal", ex);
            }
            catch (DO.BadLineIDException ex)
            {
                throw new BO.BadLineIDException("Duplicate Line ID", ex);
            }
        }
        
        public void UpdateLine(BO.Line line)
        {
            DO.Line lineDO = new DO.Line();
            line.CopyPropertiesTo(lineDO);
            lineDO.FirstStationCode = line.StationsInLine.First().StationCode;
            lineDO.LastStationCode = line.StationsInLine.Last().StationCode;

            try
            {
                dl.UpdateLine(lineDO);
            }
            catch (DO.BadLineIDException ex)
            {
                throw new BO.BadLineIDException("Line ID is illegal", ex);
            }
        }

        public void DeleteLine(int id)
        {
            try
            {
                dl.DeleteLine(id);
                dl.DeleteSolByLine(id);
            }
            catch (DO.BadLineIDException ex)
            {
                throw new BO.BadLineIDException("Line ID does Not exist", ex);
            }
        }

        //public IEnumerable<BO.Line> GetAllLinesBy(Predicate<BO.Line> predicate)
        //{
        //    //return from lineDO in dl.GetAllLinesBy(predicate)
        //    //       orderby lineDO.Id
        //    //       select lineDoBoAdapter(lineDO);
        //    throw new NotImplementedException();
        //}
        //public void UpdateLine(int id, Action<BO.Line> update)
        //{
        //    throw new NotImplementedException();
        //}
        #endregion

        #region Station
        BO.Station stationDoBoAdapter(DO.Station stationDO)
        {
            BO.Station stationBO = new BO.Station();
            stationDO.CopyPropertiesTo(stationBO);

            stationBO.LinesInStation = from sol in dl.GetAllStationsOfLinesBy(sol => sol.StationCode == stationDO.Code)
                                       let line = dl.GetLine(sol.LineID)
                                       select line.CopyToLineOfStation(sol);

            return stationBO;
        }
        
        public BO.Station GetStation(int code)
        {
            DO.Station stationDO;
            try
            {
                stationDO = dl.GetStation(code);
            }
            catch (DO.BadStationCodeException ex)
            {
                throw new BO.BadStationCodeException("Station code does not exist", ex);
            }
            return stationDoBoAdapter(stationDO);
        }
        
        public IEnumerable<BO.Station> GetAllStations()
        {
            return from stationDO in dl.GetAllStations()
                   orderby stationDO.Code
                   select stationDoBoAdapter(stationDO);
        }
        
        public void AddStation(BO.Station stationBO)
        {
            //Add new DO.Station with no lines       
            DO.Station stationDO = new DO.Station();
            stationBO.CopyPropertiesTo(stationDO);
            try
            {
                dl.AddStation(stationDO);
            }
            catch (DO.BadStationCodeException ex)
            {
                throw new BO.BadStationCodeException("Duplicate Station Code", ex);
            }
        }
        
        public void UpdateStation(BO.Station station)
        {           
            DO.Station stationDO = new DO.Station();
            station.CopyPropertiesTo(stationDO);
            try
            {
                dl.UpdateStation(stationDO);
                //if location is changed:
                dl.UpdateAdjacentStations(stationDO);
            }
            catch (DO.BadStationCodeException ex)
            {
                throw new BO.BadStationCodeException("Station code is illegal", ex);
            }
        }

        public void DeleteStation(int code)
        {
            try
            {
                dl.DeleteStation(code);
                dl.DeleteAdjacentStationsByStation(code);
            }
            catch (DO.BadStationCodeException ex)
            {
                throw new BO.BadStationCodeException(ex.Message, ex);
            }
        }

        //public IEnumerable<BO.Station> GetAllStationsBy(Predicate<BO.Station> predicate)
        //{
        //    //return from stationDO in dl.GetAllStationsBy(predicate)
        //    //       orderby stationDO.Code
        //    //       select stationDoBoAdapter(stationDO);
        //    throw new NotImplementedException();
        //}
        //public void UpdateStation(int code, Action<BO.Station> update)
        //{
        //    throw new NotImplementedException();
        //}
        #endregion

        #region StationOfLine
        BO.StationOfLine stationOfLineDoBoAdapter(DO.StationOfLine solDO)
        {
            BO.StationOfLine solBO = new BO.StationOfLine();
            solDO.CopyPropertiesTo(solBO);
            solBO.StationName = dl.GetStation(solDO.StationCode).Name;
            if (solDO.StationCode != dl.GetLine(solDO.LineID).FirstStationCode)
            {
                DO.AdjacentStations adj = dl.GetAdjacentStations(dl.GetPrevSol(solDO.LineID, solDO.StationCode).StationCode, solDO.StationCode);
                solBO.DistanceFromPre = adj.Distance;
                solBO.TimeFromPre = adj.AvgTime;
            }
            return solBO;
        }

        public IEnumerable<BO.StationOfLine> GetAllStationsOfLine(int id)
        {
            return from sol in dl.GetAllStationsOfLine(id)
                   orderby sol.StationIndexInLine
                   select stationOfLineDoBoAdapter(sol);
        }
        
        public void AddStationOfLine(int lineID, int stationCode)
        {
            try
            {
                DO.Line line = dl.GetLine(lineID);
                if (line.FirstStationCode != stationCode)
                    AddAdjacentStations(line.LastStationCode, stationCode);
                dl.AddStationOfLine(lineID, stationCode);
            }
            catch (DO.BadLineIDStationCodeException ex)
            {
                throw new BO.BadLineIDStationCodeException("Line ID and Station code is Not exist", ex);
            }
            catch (DO.BadStationCodeException ex)
            {
                throw new BO.BadStationCodeException("Bad station code", ex);
            }
        }
        
        public void DeleteStationOfLine(int lineID, int stationCode)
        {
            try
            {
                DO.Line line = dl.GetLine(lineID);
                //Add adjacent stations - previous & next stations
                if (line.FirstStationCode != stationCode && line.LastStationCode != stationCode)
                    AddAdjacentStations(dl.GetPrevSol(lineID, stationCode).StationCode, dl.GetNextSol(lineID, stationCode).StationCode);

                dl.DeleteStationOfLine(lineID, stationCode);
            }
            catch (DO.BadLineIDStationCodeException ex)
            {
                throw new BO.BadLineIDStationCodeException("Line ID and Station code does Not exist", ex);
            }
        }

        public void UpdateStationIndexInLine(int lineID, int stationCode, int newIndex)
        {
            try
            {
                DO.Line line = dl.GetLine(lineID);
                //Add adjacent stations - part 1
                if (line.FirstStationCode != stationCode && line.LastStationCode != stationCode)
                    AddAdjacentStations(dl.GetPrevSol(lineID, stationCode).StationCode, dl.GetNextSol(lineID, stationCode).StationCode);

                dl.UpdateStationIndexInLine(lineID, stationCode, newIndex);
                
                //The update changed the line
                line = dl.GetLine(lineID);
                //Add adjacent stations - part 2
                if (line.FirstStationCode != stationCode)
                    AddAdjacentStations(dl.GetPrevSol(lineID, stationCode).StationCode, stationCode);
                if (line.LastStationCode != stationCode)
                    AddAdjacentStations(stationCode, dl.GetNextSol(lineID, stationCode).StationCode);

            }
            catch (DO.BadLineIDStationCodeException ex)
            {
                throw new BO.BadLineIDStationCodeException("Line ID and Station code is Not exist", ex);
            }
            catch (DO.BadStationCodeException ex)
            {
                throw new BO.BadStationCodeException("Bad station code", ex);
            }
        }
        #endregion

        #region LineOfStation
        public IEnumerable<BO.LineOfStation> GetAllLinesOfStation(int code)
        {
            return from sol in dl.GetAllStationsOfLinesBy(sol => sol.StationCode == code)
                   let line = dl.GetLine(sol.LineID)
                   select line.CopyToLineOfStation(sol);
        }

        public IEnumerable<BO.LineOfStation> GetAllLinesOfStationSorted(int stationCode)
        {
            return from li in GetAllLinesOfStation(stationCode)
                   orderby li.Area
                   select li; //GetAllLinesOfStation did the cloning for this object
        }
        #endregion

        #region AdjacentStations
        BO.AdjacentStations adjacentStationsDoBoAdapter(DO.AdjacentStations adjDO)
        {
            BO.AdjacentStations adjBO = new BO.AdjacentStations();
            adjDO.CopyPropertiesTo(adjBO);

            return adjBO;
        }
        
        public IEnumerable<BO.AdjacentStations> GetMyAdjacentStations(int stationCode)
        {
            return from adjSt in dl.GetMyAdjacentStations(stationCode)
                   select adjacentStationsDoBoAdapter(adjSt);
        }

        public void AddAdjacentStations(int station1Code, int station2Code)
        {
            try
            {
                dl.AddAdjacentStations(station1Code, station2Code);
            }
            catch (DO.BadStationCodeException ex)
            {
                throw new BO.BadStationCodeException("Station code is Not exist", ex);
            }
        }
        public double Distance2S (int lineID, int station1Code, int station2Code)
        {
            int index1 = (from sol in GetAllStationsOfLine(lineID)
                         where sol.StationCode == station1Code
                         select sol.StationIndexInLine).FirstOrDefault();
            int index2 = (from sol in GetAllStationsOfLine(lineID)
                          where sol.StationCode == station2Code
                          select sol.StationIndexInLine).FirstOrDefault();

            if (index1 > index2)
            {
                int tmp = index1;
                index1 = index2;
                index2 = tmp;
            }
            return (from sol in GetAllStationsOfLine(lineID)
                   where sol.StationIndexInLine > index1 && sol.StationIndexInLine <= index2
                   let prevSol = dl.GetPrevSol(lineID, sol.StationCode)
                   select dl.GetAdjacentStations(prevSol.StationCode, sol.StationCode).Distance).Sum();

        }
        public TimeSpan TimeSpan2S(int lineID, int station1Code, int station2Code)
        {
            int index1 = (from sol in GetAllStationsOfLine(lineID)
                          where sol.StationCode == station1Code
                          select sol.StationIndexInLine).FirstOrDefault();
            int index2 = (from sol in GetAllStationsOfLine(lineID)
                          where sol.StationCode == station2Code
                          select sol.StationIndexInLine).FirstOrDefault();

            if (index1 > index2)
            {
                int tmp = index1;
                index1 = index2;
                index2 = tmp;
            }
            List<TimeSpan> times = (from sol in GetAllStationsOfLine(lineID)
                    where sol.StationIndexInLine > index1 && sol.StationIndexInLine <= index2
                    let prevSol = dl.GetPrevSol(lineID, sol.StationCode)
                    select dl.GetAdjacentStations(prevSol.StationCode, sol.StationCode).AvgTime).ToList();
            TimeSpan totalTime = new TimeSpan(0, 0, 0);
            for (int i = 0; i < times.Count; i++)
            {
                totalTime += times[i];
            }
            return totalTime;

        }
        #endregion

        #region LineTrip
        BO.LineTrip lineTripDoBoAdapter(DO.LineTrip lineTripDO)
        {
            BO.LineTrip lineTripBO = new BO.LineTrip();
            lineTripDO.CopyPropertiesTo(lineTripBO);
            return lineTripBO;
        }
       
        public IEnumerable<BO.LineTrip> GetAllLineTrips(int lineID)
        {
            return from lt in dl.GetAllLineTrips(lineID)
                   select lineTripDoBoAdapter(lt);
        }
        
        public int AddLineTrip(int lineID, TimeSpan startAt)
        {
            try 
            {
                int lineTripID = dl.AddLineTrip(lineID, startAt);
                return lineTripID;
            }
            catch(DO.BadLineIDException ex)
            {
                throw new BO.BadLineIDException("Duplicate line trip", ex);
            }

        }

        public void DeleteLineTrip(int lineTripID)
        {
            try
            {
                dl.DeleteLineTrip(lineTripID);
            }
            catch (DO.BadLineIDException ex)
            {
                throw new BO.BadLineIDException("Line ID does Not exist", ex);
            }
        }

        //public IEnumerable<BO.LineTrip> GetAllLinesTrips()
        //{
        //    throw new NotImplementedException();
        //}
        //public IEnumerable<BO.LineTrip> GetAllLinesTripsBy(Predicate<BO.Line> predicate)
        //{
        //    throw new NotImplementedException();
        //}
        //public void UpdateLineTrip(BO.LineTrip lineTrip)
        //{
        //    throw new NotImplementedException();
        //}
        #endregion

        #region LineTiming
        public IEnumerable<BO.LineTiming> GetAllLinesTimingOfStation(int stationCode, TimeSpan time)
        {
            //For each line that passes through this station, select it and the time it takes to reach me
            var lines = (from los in GetAllLinesOfStation(stationCode)
                         let lineDO = dl.GetLine(los.ID)
                         let timeS = TimeSpan2S(lineDO.ID, lineDO.FirstStationCode, stationCode)
                         select new {LineBO = lineDoBoAdapter(lineDO), TimeS = timeS }).ToList();

            //For each line passing through this station, 
            //find its departure time closest to the requested time and calculate the time of arrival at the requested station
            return (from line in lines
                    let nearestStart = (from lt in GetAllLineTrips(line.LineBO.ID)
                                        //Check that this line with this departure time has not yet passed the requested station
                                        where lt.StartAt + line.TimeS >= time
                                        select lt.StartAt).Min()
                    let lineTiming = new BO.LineTiming() { 
                        StartedAt = nearestStart,
                        LineId = line.LineBO.ID, 
                        LineNumber = line.LineBO.Number,
                        LastStationName = line.LineBO.StationsInLine.Last().StationName,
                        ExpectedTimeTillArrive = nearestStart + line.TimeS - time
                    }
                    select lineTiming).ToList();
        }

        #endregion
    }
}
