using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
                        Console.WriteLine("Enter license number: ");
                        string num3 = Console.ReadLine();
                        //בדיקה אם המספר נכון:

                        Console.WriteLine("Press 1 for refueling, Press 2 for treat:");
                        int ans = int.Parse(Console.ReadLine());
                        if (ans==1)
                        {
                            Bus bus1= SearchBus(num3, buses);
                            bus1.GasNow();
                        }
                       else if (ans==2)
                       {
                            Bus bus2 = SearchBus(num3, buses);
                            bus2.TreatNow();

                       }
                        else
                        {
                            Console.WriteLine("WRONG CHOICE");
                        }
                        break;
                    case Option.Show:
                        for (int i = 0; i < buses.Count; i++)
                        {
                            Console.WriteLine(buses[i].LicenseNum, buses[i].Kilometrage);
                        }
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
        public static Bus SearchBus(string num, ref Bus buses)
        {
            if (validate_busId(num)==false)
            {
                Console.WriteLine("WRONG LICENSE NUMBER");
            }
            else
            {
                for (int i = 0; i < buses.Count; i++)
                {
                    if (buses[i].LicenseNum == num2)
                    {
                        flag = true;
                        if ((buses[i].IfTreat(reqKm) == false) && (buses[i].IfRefueling(reqKm) == false))
                            buses[i].Kilometrage = reqKm;
                    }
                }
            }
            return null;
        }
        public static bool validate_busId(string busId)
        {
            string pattern7 = @"\d\d-\d\d\d-\d\d$";
            string pattern8 = @"\d\d\d-\d\d-\d\d\d$";
            Regex rgx7 = new Regex(pattern7);
            Regex rgx8 = new Regex(pattern8);

            return (rgx7.IsMatch(busId) || rgx8.IsMatch(busId));
        }
    }
}
