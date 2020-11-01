using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotNet5781_01_5828_6440
{
    enum Option
    { Add, Drive, Refueling, Show, Exit };
    class Program
    {
        static void Main(string[] args)
        {
            List<Bus> buses = new List<Bus>();
            Console.WriteLine(@"Press 1 to add a new bus
Press 2 to choose a bus for drive
Press 3 to treat or refuel of a bus
Press 4 to show the drive since the last treat
Press 5 to exit");
            Option choice = (Option)Console.Read();


            switch (choice)
            {
                case Option.Add:
                    Console.Write("Enter license number:");
                    int num = Console.Read();
                    Console.Write("Enter the year of operation of the bus:");
                    int year = Console.Read();
                    Console.Write("Enter the month of operation of the bus:");
                    int month = Console.Read();
                    Console.Write("Enter the day of operation of the bus:");
                    int day = Console.Read();
                    DateTime date = new DateTime(year, month, day);
                    Console.WriteLine(date);
                    break;
                case Option.Drive:
                    break;
                case Option.Refueling:
                    break;
                case Option.Show:
                    break;
                case Option.Exit:
                    break;
            };
            Console.ReadKey();
        }
    }
}
