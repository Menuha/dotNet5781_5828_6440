using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotNet5781_00_5828_6440
{
    partial class Program
    {
        static void Main(string[] args)
        {
            Welcome5828();
            Welcome6440();
            Console.ReadKey();
        }

        static partial void Welcome6440();

        private static void Welcome5828()
        {
            Console.Write("Enter your name: ");
            string userName = Console.ReadLine();
            Console.WriteLine("{0}, welcome to my first console application", userName);
        }
    }
}
