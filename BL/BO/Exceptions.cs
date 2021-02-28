using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    [Serializable]
    public class BadStationCodeException : Exception
    {
        public int Code;
        public BadStationCodeException(string message, Exception innerException) :
           base(message, innerException) => Code = ((DO.BadStationCodeException)innerException).Code;
        public override string ToString() => base.ToString() + $", bad station code: {Code}";
    }

    [Serializable]
    public class BadLineIDException : Exception
    {
        public int ID;
        public BadLineIDException(string message, Exception innerException) :
            base(message, innerException) => ID = ((DO.BadLineIDException)innerException).ID;
        public override string ToString() => base.ToString() + $", bad Line id: {ID}";
    }

    [Serializable]
    public class BadLineIDStationCodeException : Exception
    {
        public int LineID;
        public int StationCode;
        public BadLineIDStationCodeException(string message, Exception innerException) :
            base(message, innerException)
        { 
            LineID = ((DO.BadLineIDStationCodeException)innerException).LineID; 
            StationCode = ((DO.BadLineIDStationCodeException)innerException).StationCode; 
        }
        public override string ToString() => base.ToString() + $", bad line id: {LineID} and station code: {StationCode}";
    }
}


