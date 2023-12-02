using Medicines.Data.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medicines.DataProcessor.ImportDtos
{
    public class ImportPationDto
    {

        [MinLength(5)]
        [MaxLength(100)]
        [Required]
        public string FullName { get; set; } = null!;

        [Required]
        public int AgeGroup { get; set; }

        [Required]
        public int Gender { get; set; }

        public int[] Medicines { get; set; }
    }
}
