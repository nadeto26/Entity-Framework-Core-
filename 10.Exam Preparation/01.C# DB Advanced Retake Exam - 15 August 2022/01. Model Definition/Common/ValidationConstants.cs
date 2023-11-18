using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trucks.Common
{
    public static  class ValidationConstants
    {
        //Truck

        public const int RegistrationNumberLength = 8;

        public const int VinNumberMaxLength = 17;

        //Client

        public const int ClientNameMaxLength = 40; 
        public const int ClientNationnalittityMaxLength = 40;

        //Despatcher

        public const int DespatcherNameMaxLength = 40;
    }
}
