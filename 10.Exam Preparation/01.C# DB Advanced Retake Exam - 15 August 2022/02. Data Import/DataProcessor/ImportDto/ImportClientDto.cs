using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trucks.Common;

namespace Trucks.DataProcessor.ImportDto
{
    public class ImportClientDto
    {
        [Required]
        [MinLength(ValidationConstants.ClientNameMinLength)]
        [MaxLength(ValidationConstants.ClientNameMaxLength)]
        [JsonProperty("Name")]
        public string Name { get; set; } = null!;

        [Required]
        [MinLength(ValidationConstants.ClientNationalityMinLength)]
        [MaxLength(ValidationConstants.ClientNationalityMaxLength)]
        [JsonProperty("Nationality")]
        public string Nationality { get; set; } = null!;

        [JsonProperty("Type")]
        public string Type { get; set; } = null!;

        [JsonProperty("Trucks")] //-> в Jason може 
        public int[] TruckIds { get; set; }  
    }
}
