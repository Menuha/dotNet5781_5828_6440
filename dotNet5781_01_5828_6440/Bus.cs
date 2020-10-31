using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace dotNet5781_01_5828_6440
{
    class Bus
    {
        private string licenseNum;
        private DateTime day;
        private static int km;
        /// <summary>
        /// gas= num of km till the last refueling
        /// </summary>
        private int gas;

        public string LicenseNum
        { 
            get
            {
                return licenseNum;
            }   
            set
            {
                if (digitsNum(value) == true)
                    licenseNum = value;
            }
        }

        public static int Km
        { 
            get
            {
                return km;
            }
            set
            {
                if (value > km)
                    km = value;
            }
        }
        public int Gas
        {
            get
            {
                return gas;
            }
            set
            { }
        }
        public bool Treat()
        {
            if (Km > 20000) 
            {
                Console.WriteLine("DANGEROUS");
                return true;
            }
            return false;
        }
        public bool Refueling()
        {
            if (km >= gas + 1200) 
            {
                return false;
            }
            return true;  
        }
        public void GasNow()
        {
            gas = km;
        }
        public bool digitsNum(string num)
        {
            if((day.Year<2018) && (num.Length!=7))
            {
                Console.WriteLine("Worng License Number");
                return false;
            }
            else if ((day.Year >= 2018) && (num.Length != 8))
            {
                Console.WriteLine("Worng License Number");
                return false;
            }
            return true;
        }
    }
}
