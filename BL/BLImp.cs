using DLAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    internal class BLImp:IBL
    {
        IDL dl = DLFactory.GetDL();
        BO.Line lineDoBoAdapter(DO.Line lineDO)
        {
            BO.Line lineBO = new BO.Line();
            int id = lineDO.Id;
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
            catch (DO.BadLineIdException ex)
            {
                throw new BO.BadLineIdException("Line id does not exist", ex);
            }
            return lineDoBoAdapter(lineDO);
        }


    }
}
