using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artillery.DataProcessor.ImportDto
{
    public class ImportGunDto
    {

        //проверка + JsonProperty да си напиша 
        public int Id { get; set; }

        [Required]

        public int ManufacturerId { get; set; }



        [Required]
        [MaxLength(1_350_000)]
        [MinLength(100)]
        public int GunWeight { get; set; }

        [MaxLength(35_00)]
        [MinLength(2_00)]
        [Required]
        public double BarrelLength { get; set; }

        public int NumberBuild { get; set; }

        [Required]
        [MaxLength(100_000)]
        [MinLength(1)]
        public int Range { get; set; }

        [Required]
        public string GunType { get; set; }

        [Required]

        public int ShellId { get; set; }

        [JsonProperty("Countries")]
        public int[] Countries { get; set; }
    }

}
