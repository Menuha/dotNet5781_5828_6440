using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using BL;

namespace BLAPI
{
    public static class BLFactory
    {
        public static IBL GetBL(string type)
        {
            switch (type)
            {
                case "1":
                    return BLImp.Instance;
                //case "2":
                ////return new BLImp2();
                default:
                    throw new ArgumentException("Wrong number");
            }
        }
    }
}
