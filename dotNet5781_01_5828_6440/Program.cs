using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotNet5781_01_5828_6440
{
    enum Option{ AddBus = '1', Drive, Refueling, Show, Exit };
    class Program
    {
        static void Main(string[] args)
        {
            List<Bus> buses = new List<Bus>();

            Option choice = Menu();
            while (choice != Option.Exit)
            {
                switch (choice)
                {
                    case Option.AddBus:
                        Console.WriteLine("Enter license number: ");
                        string num1 = Console.ReadLine();
                        Console.Write("Enter the year of operation of the bus: ");
                        string year = Console.ReadLine();
                        Console.Write("Enter the month of operation of the bus: ");
                        string month = Console.ReadLine();
                        Console.Write("Enter the day of operation of the bus: ");
                        string day = Console.ReadLine();
                        //DateTime date = new DateTime(year, month, day);
                        //Console.WriteLine(date);
                        break;

                    case Option.Drive:
                        Console.WriteLine("Enter license number: ");
                        string num2 = Console.ReadLine();
                        //בדיקה אם המספר נכון:

                        Random r = new Random();
                        int reqKm = r.Next(500);
                        bool flag = false;
                        for (int i = 0; i < buses.Count; i++)
                        {
                            if (buses[i].LicenseNum == num2)
                            {
                                flag = true;
                                if ((buses[i].IfTreat(reqKm) == false) && (buses[i].IfRefueling(reqKm) == false))
                                    buses[i].Kilometrage = reqKm;
                            }
                        }
                        if (flag == false)
                            Console.WriteLine("Wrong license numbuer.");
                        break;

                    case Option.Refueling:

                        break;
                    case Option.Show:
                        break;
                    case Option.Exit:
                        break;
                }
                choice = Menu();
            }
            Console.ReadKey();
        }
        public static Option Menu()
        {
            Console.WriteLine(@"Press 1 to add a new bus
Press 2 to choose a bus for drive
Press 3 to treat or refuel of a bus
Press 4 to show the drive since the last treat
Press 5 to exit");
            //:בדיקה אם הוקש מספר נכון

            return (Option)Console.Read();
        }
        public static Bus NewBus()
        {
            return null;
        }
    }
}
