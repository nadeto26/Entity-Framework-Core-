using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VaporStore.DataProcessor.ImportDto
{
    public class ImportUserDto
    {
        [Required]
        [MinLength(3)]
        [MaxLength(20)]
        public string Username { get; set; } = null!;

        [RegularExpression(@"^[A-Z][a-z]+ [A-Z][a-z]+$")]
        [Required]
        public string FullName { get; set; }

        [Required]
        public string Email { get; set; } = null!;

        [Required]
        [MinLength(3)]
        [MaxLength(103)]
        public int Age { get; set; }

        public ImportCardDto[] Cards { get; set; }
    }

}
