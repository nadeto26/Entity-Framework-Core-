using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Invoices.DataProcessor.ImportDto
{
    public class ImportProductDto
    {
        [Required]
        [MaxLength(30)]
        [MinLength(9)]
        [JsonProperty("Name")]
        public string Name { get; set; } = null!;

        [Required]
        [Range(5_00, 1000_00)]
        [JsonProperty("Price")]
        public decimal Price { get; set; }

        [Required]
        [JsonProperty("CategoryType")]
        public int CategoryType { get; set; }

        [JsonProperty("Clients")]
        public int[] ClientsIds { get; set; }
    }


}
