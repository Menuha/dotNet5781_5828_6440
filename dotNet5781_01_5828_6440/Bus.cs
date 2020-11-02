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
        public const int maxKmTreat = 20000;
        public const int maxKmRefueling = 1200;
        private string licenseNum;
        private DateTime firstDay;
        /// <summary>
        /// kilometrage = kilometrage since the first day
        /// </summary>
        private int kilometrage;
        /// <summary>
        /// gas = kilometrage since the last refueling
        /// </summary>
        private int gas;
        private DateTime lastTreatDate;
        /// <summary>
        /// lastTreatKm = kilometrage since the last treat
        /// </summary>
        private int lastTreatKm;

        public string LicenseNum
        {
            get
            {
                return licenseNum;
            }
            set
            {
                if (DigitsNum(value) == true)
                    licenseNum = value;
            }
        }

        public int Kilometrage
        {
            get
            {
                return kilometrage;
            }
            set
            {
                if (value > 0)
                {
                    lastTreatKm += value;
                    gas += value;
                    kilometrage += value;
                }
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
        public bool IfTreat(int distance)
        {
            if ((lastTreatKm + distance) > maxKmTreat)
            {
                Console.WriteLine("DANGEROUS");
                return true;
            }

            //DateTime currentDay = DateTime.Now;
            //TimeSpan timeSpan = currentDay - lastTreatDate;
            //if (timeSpan)
            //    return false;

            //checkDate.Year = lastTreatDate.Year + 1;
            //if (checkDate.Year + 1 > currentDay.Year)

            DateTime currentDay = DateTime.Now;
            DateTime checkDate = new DateTime(lastTreatDate.Year + 1, lastTreatDate.Month, lastTreatDate.Day);
            if (checkDate >= currentDay)
                return true;
            return false;
        }
        public bool IfRefueling(int distance)
        {
            if ((gas + distance) > maxKmRefueling)
            {
                Console.WriteLine("Gas needed");
                return true;
            }
            return false;
        }
        public void GasNow()
        {
            gas = 0;
        }
        public void TreatNow()
        {
            lastTreatKm = 0;
            DateTime currentDay = DateTime.Now;
            lastTreatDate = currentDay;
        }
        public bool DigitsNum(string num)
        {
            if ((firstDay.Year >= 2018) && (num.Length == 10))
            {
                for (int i = 0; i < 10; i++)
                {
                    if ((i == 3 || i == 6) && (num[i] != '-'))
                        return false;
                    else if (num[i] < '0' || num[i] > '9')
                        return false;
                }
                return true;
            }
            if ((firstDay.Year < 2018) && (num.Length == 9))
            {
                for (int i = 0; i < 9; i++)
                {
                    if ((i == 2 || i == 6) && (num[i] != '-'))
                        return false;
                    else if (num[i] < '0' || num[i] > '9')
                        return false;
                }
                return true;
            }
            return false;
        }
    }
}

