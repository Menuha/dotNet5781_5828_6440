using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DLAPI;
using BLAPI;

namespace BL
{
    internal class BLImp : IBL
    {
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

        public void AddLine(BO.Line line, int firstStationCode, int lastStationCode)
        {
            //Add new DO.Line with no stations          
            DO.Line lineDO = new DO.Line();
            line.CopyPropertiesTo(lineDO);
            try
            {
                lineDO.FirstStationCode = dl.GetStation(firstStationCode).Code;
                lineDO.LastStationCode = dl.GetStation(lastStationCode).Code;
                dl.AddLine(lineDO);

                AddAdjacentStations(firstStationCode, lastStationCode);

                AddStationOfLine(line.ID, firstStationCode);
                AddStationOfLine(line.ID, lastStationCode);
            }
            catch (DO.BadStationCodeException ex)
            {
                throw new BO.BadStationCodeException("Station code is illegal", ex);
            }
            catch (DO.BadLineIDException ex)
            {
                throw new BO.BadLineIDException("Line ID is illegal", ex);
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
                throw new BO.BadStationCodeException("Station Code is illegal", ex);
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
                throw new BO.BadStationCodeException("Station code does Not exist", ex);
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
                solBO.DistanceFromPre = dl.GetAdjacentStations(dl.GetPrevSol(solDO.LineID, solDO.StationCode).StationCode, solDO.StationCode).Distance;
                solBO.TimeFromPre = dl.GetAdjacentStations(dl.GetPrevSol(solDO.LineID, solDO.StationCode).StationCode, solDO.StationCode).AvgTime;
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
                if (dl.GetLine(lineID).FirstStationCode != stationCode)
                    AddAdjacentStations(dl.GetLine(lineID).LastStationCode, stationCode);
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
                //Add adjacent stations - previous & next stations
                if (dl.GetLine(lineID).FirstStationCode != stationCode && dl.GetLine(lineID).LastStationCode != stationCode)
                    AddAdjacentStations(dl.GetPrevSol(lineID, stationCode).StationCode, dl.GetNextSol(lineID, stationCode).StationCode);

                dl.DeleteStationOfLine(lineID, stationCode);
            }
            catch (DO.BadLineIDStationCodeException ex)
            {
                throw new BO.BadLineIDStationCodeException("Line ID and Station code is Not exist", ex);
            }
        }

        public void UpdateStationIndexInLine(int lineID, int stationCode, int newIndex)
        {
            try
            {
                //Add adjacent stations - part 1
                if (dl.GetLine(lineID).FirstStationCode != stationCode && dl.GetLine(lineID).LastStationCode != stationCode)
                    AddAdjacentStations(dl.GetPrevSol(lineID, stationCode).StationCode, dl.GetNextSol(lineID, stationCode).StationCode);

                dl.UpdateStationIndexInLine(lineID, stationCode, newIndex);

                //Add adjacent stations - part 2
                if (dl.GetLine(lineID).FirstStationCode != stationCode)
                    AddAdjacentStations(dl.GetPrevSol(lineID, stationCode).StationCode, stationCode);
                if (dl.GetLine(lineID).LastStationCode != stationCode)
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
        #endregion
    }
}
