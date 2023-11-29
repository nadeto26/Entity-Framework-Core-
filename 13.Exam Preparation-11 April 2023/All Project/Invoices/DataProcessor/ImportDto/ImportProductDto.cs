using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
         
            public decimal Price { get; set; }

            [Required]
        
            public int CategoryType { get; set; }

           
            public int[] ClientsIds { get; set; }
        }
    

}
