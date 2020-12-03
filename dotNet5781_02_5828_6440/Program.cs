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
            List<BusLine> busLinesList = new List<BusLine>();
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
                        NewAdd(busLinesList, busStationsList);
                        break;

                    case Option.Delete:
                        NewDelete();
                        break;

                    case Option.Search:
                        NewSearch();
                        break;

                    case Option.Print:
                        NewPrint();
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

        public static void NewAdd(List<BusLine> busLinesList, List<BusStation> busStationsList )
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
                AddBusLine(busLinesList, /*busStationsList*/);
            else
               AddBusStation(busStationsList);

        }
        public static void AddBusLine(List<Buses> buses, List<BusLineStation> stations)
        {
            
        }
        public static void NewDelete()
        {

        }
    }
}


