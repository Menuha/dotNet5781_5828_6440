using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    class Exceptions
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
        public class BadLineIdException : Exception
        {
            public int Id;
            public BadLineIdException(string message, Exception innerException) :
                base(message, innerException) => Id = ((DO.BadLineIdException)innerException).Id;
            public override string ToString() => base.ToString() + $", bad Line code: {Id}";
        }

        [Serializable]
        public class BadLineIdStationCodeException : Exception
        {
            public int LineId;
            public int StationCode;
            public BadLineIdStationCodeException(string message, Exception innerException) :
                base(message, innerException)
            { LineId = ((DO.BadLineIdException)innerException).Id; StationCode = ((DO.BadStationCodeException)innerException).Code; }
            public override string ToString() => base.ToString() + $", bad line id: {LineId} and station code: {StationCode}";
        }
    }
}

