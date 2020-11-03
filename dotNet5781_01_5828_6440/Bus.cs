using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace dotNet5781_01_5828_6440
{
    /// <summary>
    /// This is a class to represent a bus by
    /// </summary>
    class Bus
    {
        public const int maxKmTreat = 20000;
        public const int maxKmRefueling = 1200;

        private string licenseNum;
        private DateTime firstDate;
        /// <summary>
        /// kilometrage = kilometrage since the first day
        /// </summary>
        private int kilometrage;
        /// <summary>
        /// gas = kilometrage since the last refueling
        /// </summary>
        private int gas;
        /// <summary>
        /// lastTreatKm = kilometrage since the last treatment
        /// </summary>
        private int lastTreatKm;
        private DateTime lastTreatDate;

        /// <summary>
        /// Constractor of the Bus class
        /// </summary>
        /// <param name="licenseNum">The bus licunse number</param>
        /// <param name="firstDate">The first date this bus started working</param>
        public Bus(string licenseNum, DateTime firstDate)
        {
            this.licenseNum = licenseNum;
            this.firstDate = firstDate;
            kilometrage = 0;
            gas = 0;
            lastTreatKm = 0;
            LastTreatDate = DateTime.Now;
        }

        public string LicenseNum
        {
            get => licenseNum;
            private set => licenseNum = value;
        }
        public DateTime FirstDate
        {
            get => firstDate;
            private set => firstDate = value;
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
                    LastTreatKm += value;
                    gas += value;
                    kilometrage += value;
                }
            }
        }
        public int Gas
        {
            get => gas;
            private set => gas = value;
        }
        public int LastTreatKm
        {
            get => lastTreatKm;
            private set => lastTreatKm = value;
        }
        public DateTime LastTreatDate 
        { 
            get => lastTreatDate; 
            private set => lastTreatDate = value; 
        }

        /// <summary>
        /// This method checks if refueling is needed to get going
        /// </summary>
        /// <param name="distance">This is the distance you want to travel on this bus.</param>
        /// <returns>"true" if refueling is required</returns>
        public bool IfRefueling(int distance)
        {
            if ((gas + distance) > maxKmRefueling)
            {
                Console.WriteLine("Gas needed");
                return true;
            }
            return false;
        }

        /// <summary>
        /// This method checks if general bus care is needed to get going.
        /// </summary>
        /// <param name="distance">This is the distance you want to travel on this bus.</param>
        /// <returns>"true" if treatment is required</returns>
        public bool IfTreat(int distance)
        {
            if ((LastTreatKm + distance) > maxKmTreat)
            {
                Console.WriteLine("Please make a general treatment, this bus is DANGEROUS!");
                return true;
            }
            DateTime currentDay = DateTime.Now;
            DateTime checkDate = new DateTime(LastTreatDate.Year + 1, LastTreatDate.Month, LastTreatDate.Day);
            if (checkDate <= currentDay)
            {
                Console.WriteLine("Please make a general treatment");
                return true;
            }
            return false;
        }

        /// <summary>
        /// This method updates that refueling has been performed
        /// </summary>
        public void GasNow()
        {
            gas = 0;
        }

        /// <summary>
        /// This method updates that a general bus treatment has been performed
        /// </summary>
        public void TreatNow()
        {
            LastTreatKm = 0;
            LastTreatDate = DateTime.Now;
        }

    }
}

