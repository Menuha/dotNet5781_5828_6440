using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO
{

    [Serializable]

    public class BadStationCodeException : Exception
    {
        public int Code;
        public BadStationCodeException(int code) : base() => Code = code;
        public BadStationCodeException(int code, string message) :
            base(message) => Code = code;
        public BadStationCodeException(int code, string message, Exception innerException) :
            base(message, innerException) => Code = code;

        public override string ToString() => base.ToString() + $", bad station code: {Code}";
    }

    public class BadLineIdException : Exception
    {
        public int Id;
        public BadLineIdException(int id) : base() => Id = id;
        public BadLineIdException(int id, string message) :
            base(message) => Id = id;
        public BadLineIdException(int id, string message, Exception innerException) :
            base(message, innerException) => Id = id;

        public override string ToString() => base.ToString() + $", bad Line code: {Id}";
    }

    //public class BadPersonIdCourseIDException : Exception
    //{
    //    public int personID;
    //    public int courseID;
    //    public BadPersonIdCourseIDException(int perID, int crsID) : base() { personID = perID; courseID = crsID; }
    //    public BadPersonIdCourseIDException(int perID, int crsID, string message) :
    //        base(message)
    //    { personID = perID; courseID = crsID; }
    //    public BadPersonIdCourseIDException(int perID, int crsID, string message, Exception innerException) :
    //        base(message, innerException)
    //    { personID = perID; courseID = crsID; }

    //    public override string ToString() => base.ToString() + $", bad person id: {personID} and course id: {courseID}";
    //}
}
