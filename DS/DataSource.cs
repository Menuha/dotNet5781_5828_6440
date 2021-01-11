using DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DS
{

    public static class DataSource
    {
        public static List<Bus> ListBuses;
        public static List<Station> ListStations;
        public static List<Line> ListLines;
        public static List<StationOfLine> ListStationsOfLine;
        public static List<User> ListUsers;
        public static List<Trip> ListTrips;

        static DataSource()
        {
            InitAllLists();
        }
        static void InitAllLists()
        {
            ListBuses = new List<Bus>
            {
                new Bus
                {
                    LicenseNum= 1111,
                    FromDate=new DateTime(2019,2,1),
                    TotalTrip=12000,
                    FuelRemain=450,
                    Status=BusStatus.ReadyToDrive
                },

                new Bus
                {
                    LicenseNum= 1112,
                    FromDate=new DateTime(2018,3,1),
                    TotalTrip=18000,
                    FuelRemain=500,
                    Status=BusStatus.ReadyToDrive
                },

                 new Bus
                {
                    LicenseNum= 1113,
                    FromDate=new DateTime(2018,1,1),
                    TotalTrip=5000,
                    FuelRemain=1000,
                    Status=BusStatus.ReadyToDrive
                },

                new Bus
                {
                    LicenseNum= 1114,
                    FromDate=new DateTime(2020,2,1),
                    TotalTrip=12000,
                    FuelRemain=400,
                    Status=BusStatus.Driving
                },

                 new Bus
                {
                    LicenseNum= 1115,
                    FromDate=new DateTime(2019,8,1),
                    TotalTrip=19000,
                    FuelRemain=450,
                    Status=BusStatus.ReadyToDrive
                },
            };

            ListStations = new List<Station>
            {
                new Station
                {
                  Code=38831,
                  Name="בי''ס בר לב/בן יהודה",
                  Latitude=32,
                  Longitude=34,
                  Adress="רחוב:בן יהודה 76 עיר: כפר סבא"
                },
                new Station
                {
                  Code=38832,
                  Name="הרצל/צומת בילו",
                  Latitude=31,
                  Longitude=34,
                  Adress=" רחוב:הרצל  עיר: קרית עקרון"
                },
                new Station
                {
                  Code=38833,
                  Name="הנחשול/הדייגים",
                  Latitude=31,
                  Longitude=34,
                  Adress="רחוב:הנחשול 30 עיר: ראשון לציון"
                },
                new Station
                {
                  Code=38834,
                  Name="פריד/ששת הימים",
                  Latitude=31,
                  Longitude=34,
                  Adress=" רחוב:משה פריד 9 עיר: רחובות"
                },
            };

            ListLines = new List<Line>
            {
                new Line
                {
                    Id = 1,
                    Code = 2,
                    Area = Areas.Center,
                    FirstStationCode = 38831,
                    LastStationCode = 38833
                },
            };

            ListStationsOfLine = new List<StationOfLine>
            {
                new StationOfLine
                {
                    LineId=123,
                    StationCode=38832,
                    StationIndexInLine=12,
                    PrevStationCode=38831,
                    NextStationCode=38833
                },
            };

            ListUsers = new List<User>
            {
                new User
                {
                    UserName="Moshe",
                    Password="1234AA",
                    Admin=true
                },
            };

            ListTrips = new List<Trip>
            {
                new Trip
                {
                    Id=12345,
                    UserName="David Cohen",
                    LineId=11111,
                    InStationCode=38832,
                    InAt=new TimeSpan(07,00,00),
                    OutStationCode=38834,
                    OutAt =new TimeSpan(04,00,00)
                },
            };
        }

    }
}
