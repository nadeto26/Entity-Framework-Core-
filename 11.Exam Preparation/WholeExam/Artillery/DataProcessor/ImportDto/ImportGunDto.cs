using Artillery.Data.Models.Enums;
using Artillery.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Artillery.DataProcessor.ImportDto
{
     
    public class ImportGunDto
    {
        public int ManufacturerId { get; set; }

        [Range(100, 1_350_000)]
        public int GunWeight { get; set; }

        [Range(2_00, 35_00)]
        public double BarrelLength { get; set; }

        public int? NumberBuild { get; set; }

        [Range(1,100_000)]
        public int Range { get; set; }

        [Required]
        public string GunType { get; set; }

        [ForeignKey(nameof(ShellId))]
        public int ShellId { get; set; }

        public int[] Countries { get; set; }
    }
}
