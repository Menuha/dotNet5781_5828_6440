//Menuha Peleg 208095828
//
//A program to represent a list of buses
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace dotNet5781_01_5828_6440
{
    enum Option{ Add = 1, Drive, RefuelOrTreat, ShowKilometrage, Exit };
    class Program
    {
        public const int maxKm = 500;
        static void Main(string[] args)
        {
            List<Bus> busesList = new List<Bus>();
            Option choice;
            do
            {
                choice = ActionsMenu();
                switch (choice)
                {
                    case Option.Add:
                        NewBus(busesList);
                        break;

                    case Option.Drive:
                        NewDrive(busesList); 
                        break;

                    case Option.RefuelOrTreat:
                        NewRefuelOrTreat(busesList);
                        break;

                    case Option.ShowKilometrage:
                        NewShowKilomtrage(busesList);
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
            while (true)
            {
                Console.WriteLine("Press 1 to add a new bus");
                Console.WriteLine("Press 2 to add a new drive");
                Console.WriteLine("Press 3 to treat or refuel of a bus");
                Console.WriteLine("Press 4 to show all the buses's kilometrage since the last treat");
                Console.WriteLine("Press 5 to exit");
                m = Console.ReadLine();
                if (m.Length == 1 && m[0] >= '1' && m[0] <= '5') 
                    break;
                Console.WriteLine("WRONG CHOISE");
            }
            return (Option)Enum.Parse(typeof(Option), m);
        }

        /// <summary>
        /// Method for adding a bus to the list of buses in the company
        /// </summary>
        /// <param name="busesList">The list to which you would like to add the bus</param>
        public static void NewBus(List<Bus> busesList)
        {
            string licenseNum;
            while (true)
            {
                Console.WriteLine("Enter license number (nnn-nn-nnn or nn-nnn-nn): ");
                licenseNum = Console.ReadLine();
                if (validateLicenseNum(licenseNum) == true)
                {
                    if(SearchBus(licenseNum, busesList) == -1)
                        break;
                    Console.WriteLine("This bus already exist");
                }
                else
                    Console.WriteLine("WRONG LICENSE STRUCTURE");
            }
            string fd;
            bool flag = false;
            DateTime firstDate;
            while (true)
            {
                Console.WriteLine("Enter the first date this bus started working (dd/mm/yyyy):");
                fd = Console.ReadLine();
                flag = DateTime.TryParse(fd, out firstDate);
                if (flag == true)
                    break;
                Console.WriteLine("WRONG DATE");
            }
            Bus newBus = new Bus(licenseNum, firstDate);
            busesList.Add(newBus);
        }

        /// <summary>
        /// Method for choosing a bus for travel
        /// </summary>
        /// <param name="busesList">The list you search and update your bus details in</param>
        public static void NewDrive(List<Bus> busesList)
        {
            if(busesList.Count == 0)
            {
                Console.WriteLine("There are no Buses yet");
                return;
            }
            string licenseNum;
            int index = -1;
            while (true)
            {
                Console.WriteLine("Enter license number (nnn-nn-nnn or nn-nnn-nn): ");
                licenseNum = Console.ReadLine();
                if (validateLicenseNum(licenseNum) == true)
                {
                    index = SearchBus(licenseNum, busesList);
                    if (index != -1)
                        break;
                }
                Console.WriteLine("WRONG LICENSE NUMBER");
            }
            Random r = new Random();
            int reqKm = r.Next(maxKm);
            Console.WriteLine(reqKm);
            if ((busesList[index].IfTreat(reqKm) == false) && (busesList[index].IfRefueling(reqKm) == false))            
                busesList[index].Kilometrage = reqKm;
        }

        /// <summary>
        /// Method for refueling or treating a bus
        /// </summary>
        /// <param name="busesList">The list you search and update your bus details in</param>
        public static void NewRefuelOrTreat(List<Bus> busesList)
        {
            if (busesList.Count == 0)
            {
                Console.WriteLine("There are no Buses yet");
                return;
            }
            string licenseNum;
            int index = -1;
            while (true)
            {
                Console.WriteLine("Enter license number (nnn-nn-nnn or nn-nnn-nn): ");
                licenseNum = Console.ReadLine();
                if (validateLicenseNum(licenseNum) == true)
                {
                    index = SearchBus(licenseNum, busesList);
                    if (index != -1)
                        break;
                }
                Console.WriteLine("WRONG LICENSE NUMBER");
            }
            int ans;
            while (true)
            {
                Console.WriteLine("Press 1 for refueling");
                Console.WriteLine("Press 2 for treatment");
                ans = int.Parse(Console.ReadLine());
                if (ans == 1 || ans == 2)
                    break;
                Console.WriteLine("WRONG CHOICE");
            }
            if (ans == 1)
            {
                busesList[index].GasNow();
            }
            else if (ans == 2)
            {
                busesList[index].TreatNow();
            }
        }

        /// <summary>
        /// Method for presenting the kilometrage since the last treatment for all vehicles in the company
        /// </summary>
        /// <param name="busesList">The list from which you would like to print your buses details</param>
        public static void NewShowKilomtrage(List<Bus> busesList)
        {
            if (busesList.Count == 0)
            {
                Console.WriteLine("There are no Buses yet");
                return;
            }
            for (int i = 0; i < busesList.Count; i++)
            {
                Console.WriteLine($"The bus with the license number {busesList[i].LicenseNum} traveled {busesList[i].LastTreatKm} kilometers since the last treat.");
            }
        }

        /// <summary>
        /// A method that searches for whether a bus is in the list by its license number
        /// </summary>
        /// <param name="licenseNum">The license number you want to find</param>
        /// <param name="busesList">The list you want to search the bus in</param>
        /// <returns>"-1" if the license number not fond, else the index of the bus in the list</returns>
        public static int SearchBus(string licenseNum, List<Bus> busesList)
        {
            int index = -1;
            if (validateLicenseNum(licenseNum) == true)
            {
                for (int i = 0; i < busesList.Count; i++)
                {
                    if (busesList[i].LicenseNum == licenseNum)
                        index = i;
                }
            }
            return index;
        }

        /// <summary>
        /// This method checks the validity of a bus license number
        /// </summary>
        /// <param name="licenceNum">The bus license number you want to check</param>
        /// <returns>"true" if the license number is correct</returns>
        public static bool validateLicenseNum(string licenceNum)
        {
            string pattern7 = @"\d\d-\d\d\d-\d\d$";
            string pattern8 = @"\d\d\d-\d\d-\d\d\d$";
            Regex rgx7 = new Regex(pattern7);
            Regex rgx8 = new Regex(pattern8);

            return (rgx7.IsMatch(licenceNum) || rgx8.IsMatch(licenceNum));
        }
    }
}
//Example of output:
//Press 1 to add a new bus
//Press 2 to add a new drive
//Press 3 to treat or refuel of a bus
//Press 4 to show all the buses's kilometrage since the last treat
//Press 5 to exit
//1
//Enter license number (nnn-nn-nnn or nn-nnn-nn):
//111 - 22 - 333
//Enter the first date this bus started working (dd/mm/yyyy):
//1 / 1 / 2020
//Press 1 to add a new bus
//Press 2 to add a new drive
//Press 3 to treat or refuel of a bus
//Press 4 to show all the buses's kilometrage since the last treat
//Press 5 to exit
//2
//Enter license number (nnn-nn-nnn or nn-nnn-nn):
//111 - 22 - 333
//158
//Press 1 to add a new bus
//Press 2 to add a new drive
//Press 3 to treat or refuel of a bus
//Press 4 to show all the buses's kilometrage since the last treat
//Press 5 to exit
//4
//The bus with the license number 111 - 22 - 333 traveled 158 kilometers since the last treat.
//Press 1 to add a new bus
//Press 2 to add a new drive
//Press 3 to treat or refuel of a bus
//Press 4 to show all the buses's kilometrage since the last treat
//Press 5 to exit
//5
