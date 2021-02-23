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

            //lineBO.StationsInLine= from sil in dl.GetAllStationsOfLineBy(sil=> sil.LineId==lineDO.Id)


            //lineBO.ListOfCourses = from sic in dl.GetStudentsInCourseList(sic => sic.PersonId == id)
            //                          let course = dl.GetCourse(sic.CourseId)
            //                          select course.CopyToStudentCourse(sic);
            //new BO.StudentCourse()
            //{
            //    ID = course.ID,
            //    Number = course.Number,
            //    Name = course.Name,
            //    Year = course.Year,
            //    Semester = (BO.Semester)(int)course.Semester,
            //    Grade = sic.Grade
            //};

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
            //Add DO.Line           
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
                //dl.DeleteStudentFromAllCourses(id);
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

            //stationBO.LinesInStation= from sil in dl.GetAllStationsOfLineBy(sil => sil.LineId == lineDO.Id)


            //lineBO.ListOfCourses = from sic in dl.GetStudentsInCourseList(sic => sic.PersonId == id)
            //                          let course = dl.GetCourse(sic.CourseId)
            //                          select course.CopyToStudentCourse(sic);
            //new BO.StudentCourse()
            //{
            //    ID = course.ID,
            //    Number = course.Number,
            //    Name = course.Name,
            //    Year = course.Year,
            //    Semester = (BO.Semester)(int)course.Semester,
            //    Grade = sic.Grade
            //};
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
            //Add DO.Station            
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
                //dl.DeleteStudentFromAllCourses(id);
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

            //stationBO.LinesInStation= from sil in dl.GetAllStationsOfLineBy(sil => sil.LineId == lineDO.Id)


            //lineBO.ListOfCourses = from sic in dl.GetStudentsInCourseList(sic => sic.PersonId == id)
            //                          let course = dl.GetCourse(sic.CourseId)
            //                          select course.CopyToStudentCourse(sic);
            //new BO.StudentCourse()
            //{
            //    ID = course.ID,
            //    Number = course.Number,
            //    Name = course.Name,
            //    Year = course.Year,
            //    Semester = (BO.Semester)(int)course.Semester,
            //    Grade = sic.Grade
            //};

            return solBO;
        }

        public IEnumerable<BO.StationOfLine> GetAllStationsOfLine(int id)
        {
            return from sol in dl.GetAllStationsOfLine(id)
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
