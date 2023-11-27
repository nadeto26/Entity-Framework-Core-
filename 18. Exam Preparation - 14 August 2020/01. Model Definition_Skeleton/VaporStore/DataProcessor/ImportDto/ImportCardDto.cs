using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VaporStore.DataProcessor.ImportDto
{
    public class ImportCardDto
    {
        public string Number { get; set; }

        [RegularExpression(@"^\d{3}$")]
        [Required]
        public string Cvc { get; set; }


        [Required]
        public string Type { get; set; }
    }

}
