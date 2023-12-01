using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artillery.Data.Models
{
    public class CountryGun
    {
        public int CountryId { get; set; }
        public virtual Country Country { get; set; }

        public int GunId { get; set; }
        public virtual Gun Gun { get; set; }
    }
}
