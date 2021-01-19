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

    public class BadLineIDException : Exception
    {
        public int ID;
        public BadLineIDException(int id) : base() => ID = id;
        public BadLineIDException(int id, string message) :
            base(message) => ID = id;
        public BadLineIDException(int id, string message, Exception innerException) :
            base(message, innerException) => ID = id;

        public override string ToString() => base.ToString() + $", bad Line id: {ID}";
    }

    public class BadLineIDStationCodeException : Exception
    {
        public int LineID;
        public int StationCode;
        public BadLineIDStationCodeException(int lineID, int stationCode) : base() {LineID = lineID; StationCode = stationCode; }
        public BadLineIDStationCodeException(int linID, int stationCode, string message) :
            base(message)
        { LineID = linID; StationCode = stationCode; }
        public BadLineIDStationCodeException(int lineID, int stationCode, string message, Exception innerException) :
            base(message, innerException)
        { LineID = lineID; StationCode = stationCode; }

        public override string ToString() => base.ToString() + $", bad line id: {LineID} and station code: {StationCode}";
    }
}
