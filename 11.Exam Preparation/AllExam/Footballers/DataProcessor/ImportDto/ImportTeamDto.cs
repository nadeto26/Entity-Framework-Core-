using Footballers.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Footballers.DataProcessor.ImportDto
{
    public class ImportTeamDto
    {
        [Required]
        [MaxLength(GlobalConstants.TeamNameMaxLength)]
        [MinLength(GlobalConstants.TeamNameMinLength)]
        [RegularExpression(GlobalConstants.TeamNameRegex)]
        
        public string Name { get; set; }

        [Required]
        [MaxLength(GlobalConstants.TeamNationalityMaxLength)]
        [MinLength(GlobalConstants.TeamNationalityMinLength)]
        
        public string Nationality { get; set; }

        [Required]
       
        public int Trophies { get; set; }

        
        public int[] Footballers { get; set; }
    }
}
