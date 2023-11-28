using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftJail.DataProcessor.ImportDto
{
    public class ImportPrisonersDto
    {
        [Required]
        [MaxLength(GlobalConstants.DepartmentNameMaxLength)]
        public string Name { get; set; }

        public ImportCell[] Cells { get; set; }
    }
}
