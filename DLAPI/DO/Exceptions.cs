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

    public class BadLineIdStationCodeException : Exception
    {
        public int LineId;
        public int StationCode;
        public BadLineIdStationCodeException(int lineId, int stationCode) : base() {LineId = lineId; StationCode = stationCode; }
        public BadLineIdStationCodeException(int lineId, int stationCode, string message) :
            base(message)
        { LineId = lineId; StationCode = stationCode; }
        public BadLineIdStationCodeException(int lineId, int stationCode, string message, Exception innerException) :
            base(message, innerException)
        { LineId = lineId; StationCode = stationCode; }

        public override string ToString() => base.ToString() + $", bad line id: {LineId} and station code: {StationCode}";
    }
}
