//Menuha Peleg 208095828
//Shira Cohen 207486440
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace dotNet5781_02_5828_6440
{
    enum Option { Add = 1, Delete, Search, Print, Exit };
    class Program
    {
        static void Main(string[] args)
        {
            List<BusStation> busStationsList = new List<BusStation>();
            Buses busLinesList = new Buses();
            Buses busLines = new Buses();
            //List<BusLine> busLinesList = new List<BusLine>();
            while (busStationsList.Count < 40)
            {
                AddBusStation(busStationsList);
            }
            while (busLinesList.Count < 10)
            {
                AddBusLine(busLinesList);
            }



            Option choice;
            do
            {
                choice = ActionsMenu();
                switch (choice)
                {
                    case Option.Add:
                        NewAdd(busLinesList);
                        break;

                    case Option.Delete:
                        NewDelete(busLines);
                        break;

                    case Option.Search:
                        NewSearch(busLines);
                        break;

                    case Option.Print:
                        NewPrint(busLinesList);
                        break;

                    case Option.Exit:
                        break;
                }

            } while (choice != Option.Exit);
            Console.ReadKey();

        }
        /// <summary>
        /// Show menu of actions and select action
        /// </summary>
        /// <returns>The user selection</returns>
        public static Option ActionsMenu()
        {
            string m;
            try
            {
                Console.WriteLine("Press 1 to add");
                Console.WriteLine("Press 2 to delete");
                Console.WriteLine("Press 3 to search");
                Console.WriteLine("Press 4 to print");
                Console.WriteLine("Press 5 to exit");
                m = Console.ReadLine();
                if (m.Length != 1 || m[0] < '1' || m[0] > '5')
                {
                    throw new System.ArgumentException("WRONG CHOISE");
                }

                //    if (m.Length == 1 && m[0] >= '1' && m[0] <= '5')
                //        break;
                //    Console.WriteLine("WRONG CHOISE");
                //}
                //return (Option)Enum.Parse(typeof(Option), m);
            }
            catch
            {

            }
        }
        //add-if noy number
        //  catch (FormatException e)
        //{
        //    Console.WriteLine("The value must be numeric"); 
        // }

        public static void NewAdd(Buses busLinesList /*List<BusLine> busLinesList*//*, List<BusStation> busStationsList*/ )
        {
            string m;
            while (true)
            {
                Console.WriteLine("Press 1 to add a new bus line");
                Console.WriteLine("Press 2 to add a new bus station to bus line");
                m = Console.ReadLine();
                if (m == "1" || m == "2")
                    break;
                Console.WriteLine("WRONG CHOISE");
            }
            if (m == "1")
                AddBusLine(busLinesList);
            else
               AddBusStation(busLinesList);

        }
        public static void AddBusLine(Buses busLinesList)
        {
            Console.WriteLine("Enter bus code:");
            int busCode = int.Parse(Console.ReadLine());
            Console.WriteLine("Enter first station code:");
            int fStationCode = int.Parse(Console.ReadLine());
            BusStation firstStation = new BusStation(fStationCode);
            Console.WriteLine("Enter last station code:");
            int lStationCode = int.Parse(Console.ReadLine());
            BusStation lastStation = new BusStation(lStationCode);
            Console.WriteLine("Enter bus area(1-7):");
            string busarea = Console.ReadLine();
            Area myArea = (Area)Enum.Parse(typeof(Area), busarea);
            busLinesList.Add(new BusLine(busCode, firstStation, lastStation, myArea));
        }
        public static void AddBusStation(Buses busLinesList)
        {
            Console.WriteLine("Enter the bus code:");
            int busCode = int.Parse(Console.ReadLine());
            BusLine b = busLinesList.Search(busCode);
            Console.WriteLine("Enter station code:");
            int stationCode = int.Parse(Console.ReadLine());
            BusStation s = new BusStation(stationCode);
            Console.WriteLine("Enter the index of station in the route:");
            int i = int.Parse(Console.ReadLine());
            b.Insert(i, s);
        }
        public static void NewDelete(Buses busLines)
        {
            string m;
            while (true)
            {
                Console.WriteLine("Press 1 to remove a bus line");
                Console.WriteLine("Press 2 to remove a bus station");
                m = Console.ReadLine();
                if (m == "1" || m == "2")
                    break;
                Console.WriteLine("WRONG CHOISE");
            }
            if (m == "1")
                DeleteBusLine(busLines);
            else
                DeleteBusStation(busLines);
        }
        public static void DeleteBusLine(Buses busLines)
        {
            Console.WriteLine("Enter the bus code:");
            busLines.Remove(int.Parse(Console.ReadLine()));
        }
        public static void DeleteBusStation(Buses busLines)
        {
            Console.WriteLine("Enter the bus code of the station you want to remove:");
            int busCode = int.Parse(Console.ReadLine());
            Console.WriteLine("Enter the station code:");
            busLines.Find(busCode).Remove(int.Parse(Console.ReadLine()));
        }
        public static void NewSearch(Buses busLines)
        {
            string m;
            while (true)
            {
                Console.WriteLine("Press 1 to search buses in a station");
                Console.WriteLine("Press 2 to search buses for your route");
                m = Console.ReadLine();
                if (m == "1" || m == "2")
                    break;
                Console.WriteLine("WRONG CHOISE");
            }
            if (m == "1")
                SearchByStation(busLines);
            else
                SearchByRoute(busLines);
        }
        public static void SearchByStation(Buses busLines)
        {
            Console.WriteLine("Enter the station code:");
            int stationCode = int.Parse(Console.ReadLine());
            Console.WriteLine(busLines.LinesForStation(stationCode));
        }
        public static void SearchByRoute(Buses busLines)
        {
            Console.WriteLine("Enter first station code:");

        }
        public static void NewPrint(Buses busLinesList)
        {
            string m;
            while (true)
            {
                Console.WriteLine("Press 1 to print all the bus lines");
                Console.WriteLine("Press 2 to print list of all stations and the buses code passing through them");
                m = Console.ReadLine();
                if (m == "1" || m == "2")
                    break;
                Console.WriteLine("WRONG CHOISE");
            }
            if (m == "1")
                PrintAll(busLinesList);
            else
                PrintStations(busLinesList);

        }
        public static void PrintAll(Buses busLinesList)
        {
            foreach (var item in busLinesList)
                Console.WriteLine(item);
        }
        public static void PrintStations(Buses busLinesList)
        {
            for (int i = 0; i < ; i++)
            {

            }
        }

    }
}


