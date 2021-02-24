using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DO;

namespace DS
{

    public static class DataSource
    {
        public static List<Line> ListLines;
        public static List<LineTrip> ListLinesTrips;
        public static List<Station> ListStations;
        public static List<StationOfLine> ListStationsOfLines;
        public static List<AdjacentStations> ListAdjacentStations;
       
        //Bonus:
        public static List<Bus> ListBuses;
        public static List<BusOnTrip> ListBusesOnTrips;
        public static List<User> ListUsers;
        public static List<Trip> ListTrips;

        static DataSource()
        {
            InitAllLists();
        }
        static void InitAllLists()
        {
            ListLines = new List<Line>
            {
                new Line
                {
                    ID = 1,
                    Number = 2,
                    Area = Areas.Center,
                    FirstStationCode = 38831,
                    LastStationCode = 38833
                },

                new Line
                {
                    ID = 2,
                    Number = 3,
                    Area = Areas.Center,
                    FirstStationCode = 38831,
                    LastStationCode = 38833
                },

                 new Line
                {
                    ID = 3,
                    Number = 2,
                    Area = Areas.Jerusalem,
                    FirstStationCode = 38834,
                    LastStationCode = 38833
                },
            };

            ListLinesTrips = new List<LineTrip>();

            ListStations = new List<Station>
            {
                new Station
                {
                  Code=38831,
                  Name="בי''ס בר לב/בן יהודה",
                  Latitude=32,
                  Longitude=34,             
                },
                new Station
                {
                  Code=38832,
                  Name="הרצל/צומת בילו",
                  Latitude=31,
                  Longitude=34,
                },
                new Station
                {
                  Code=38833,
                  Name="הנחשול/הדייגים",
                  Latitude=31,
                  Longitude=34,
                },
                new Station
                {
                  Code=41133,
                  Name="פריד/ששת הימים",
                  Latitude=31,
                  Longitude=34,
                },
                new Station
                {
                  Code=38834,
                  Name="מסדה/בלפור",
                  Latitude=31,
                  Longitude=34,
                },
            };

            ListStationsOfLines = new List<StationOfLine>
            {
                new StationOfLine
                {
                    LineID=1,
                    StationCode=38831,
                    StationIndexInLine=1,
                },
                
                new StationOfLine
                {
                    LineID=1,
                    StationCode=38833,
                    StationIndexInLine=2,
                },

            };

            ListAdjacentStations = new List<AdjacentStations>
            {
                new AdjacentStations
                {
                    Station1Code=38833,
                    Station2Code=38831,
                    Distance=50,
                    AvgTime=new TimeSpan(0,5,0),
                },
            };

            //Bonus:
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

            ListBusesOnTrips = new List<BusOnTrip>();

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
                    ID=12345,
                    UserName="David Cohen",
                    LineID=11111,
                    InStationCode=38832,
                    InAt=new TimeSpan(07,00,00),
                    OutStationCode=38834,
                    OutAt =new TimeSpan(04,00,00)
                },
            };
        }

    }
}
