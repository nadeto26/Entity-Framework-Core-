using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeisterMask.DataProcessor.ImportDto
{
    public class ImportEmployeeDto
    {
        [MinLength(3)]
        [MaxLength(40)]
        [RegularExpression(@"^[a-zA-Z0-9]*$")]
        [Required]
        public string Username { get; set; } = null!;

        [Required]
        [RegularExpression(@"[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$")]
        public string Email { get; set; } = null!;

        [Required]
        [RegularExpression(@"^\d{3}-\d{3}-\d{4}$")]
        public string Phone { get; set; } = null!;

        public int[] Tasks { get; set; }
    }
}
