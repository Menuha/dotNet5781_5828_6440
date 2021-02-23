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

        //public IEnumerable<BO.Line> GetAllLinesBy(Predicate<BO.Line> predicate)
        //{
        //    //return from lineDO in dl.GetAllLinesBy(predicate)
        //    //       orderby lineDO.Id
        //    //       select lineDoBoAdapter(lineDO);
        //    throw new NotImplementedException();
        //}

        public void AddLine(BO.Line line)
        {
            //Add new DO.Line with no stations          
            DO.Line lineDO = new DO.Line();
            line.CopyPropertiesTo(lineDO);
            try
            {
                dl.AddLine(lineDO);
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

        //public void UpdateLine(int id, Action<BO.Line> update)
        //{
        //    throw new NotImplementedException();
        //}

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
        
        //public IEnumerable<BO.Station> GetAllStationsBy(Predicate<BO.Station> predicate)
        //{
        //    //return from stationDO in dl.GetAllStationsBy(predicate)
        //    //       orderby stationDO.Code
        //    //       select stationDoBoAdapter(stationDO);
        //    throw new NotImplementedException();
        //}
        
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
            }
            catch (DO.BadStationCodeException ex)
            {
                throw new BO.BadStationCodeException("Station code is illegal", ex);
            }
        }

        //public void UpdateStation(int code, Action<BO.Station> update)
        //{
        //    throw new NotImplementedException();
        //}

        public void DeleteStation(int code)
        {
            try
            {
                dl.DeleteStation(code);
                dl.DeleteSolByStation(code);
            }
            catch (DO.BadStationCodeException ex)
            {
                throw new BO.BadStationCodeException("Station code does Not exist", ex);
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

        #region StationOfLine
        BO.StationOfLine stationOfLineDoBoAdapter(DO.StationOfLine solDO)
        {
            BO.StationOfLine solBO = new BO.StationOfLine();
            solDO.CopyPropertiesTo(solBO);
            solBO.StationName = dl.GetStation(solDO.StationCode).Name;

            //solBO.DistanceFromPre = 
            //solBO.TimeFromPre = 

            return solBO;
        }

        public IEnumerable<BO.StationOfLine> GetAllStationsOfLine(int id)
        {
            return from sol in dl.GetAllStationsOfLine(id)
                   orderby sol.StationIndexInLine
                   select stationOfLineDoBoAdapter(sol);
        }
        
        public void AddStationOfLine(int lineId, int stationCode)
        {
            try
            {
                dl.AddStationOfLine(lineId, stationCode);
            }
            catch (DO.BadLineIDStationCodeException ex)
            {
                throw new BO.BadLineIDStationCodeException("Line ID and Station code is Not exist", ex);
            }
        }
        
        public void DeleteStationOfLine(int lineId, int stationCode)
        {
            try
            {
                dl.DeleteStationOfLine(lineId, stationCode);
            }
            catch (DO.BadLineIDStationCodeException ex)
            {
                throw new BO.BadLineIDStationCodeException("Line ID and Station code is Not exist", ex);
            }
        }
        #endregion
    }
}
