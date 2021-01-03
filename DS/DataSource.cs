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
        public static List<Line> ListLines;
        public static List<LineStation> ListLineStations;
        public static List<Station> ListStations;
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
                    LicenseNum= 1234,
                    FromDate=new DateTime(2019,2,1),
                    TotalTrip=12000,
                    FuelRemain=450,
                    Status=BusStatus.ReadyToDrive
                },

                new Person
                {
                    Name = "Yossi",
                    ID = 23,
                    Street = "Moshe Dayan",
                    HouseNumber = 145,
                    City = "Jerusalem",
                    PersonalStatus = PersonalStatus.SINGLE,
                    BirthDate = DateTime.Parse("13.10.95")
                },

                new Person
                {
                    Name = "Roni",
                    ID = 15,
                    Street = "Dayan",
                    HouseNumber = 33,
                    City = "Petach Tikva",
                    PersonalStatus = PersonalStatus.MARRIED,
                    BirthDate = DateTime.Parse("14.02.98")
                },

                new Person
                {
                    Name = "Shira",
                    ID = 3,
                    Street = "Moshe",
                    HouseNumber = 33,
                    City = "Eilat",
                    PersonalStatus = PersonalStatus.SINGLE,
                    BirthDate = DateTime.Parse("13.10.95")
                },

                new Person
                {
                    Name = "Gila",
                    ID = 67,
                    Street = "Marom",
                    HouseNumber = 56,
                    City = "Givataiim",
                    PersonalStatus = PersonalStatus.MARRIED,
                    BirthDate = DateTime.Parse("14.11.90")
                }


            };

        }
    }
