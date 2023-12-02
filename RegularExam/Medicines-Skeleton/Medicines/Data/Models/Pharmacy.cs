using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Medicines.Data.Models
{
    public class Pharmacy
    {
        public Pharmacy()
        {
            Medicines = new HashSet<Medicine>();
        }
        [Key]
        public int Id  { get; set; }

        [MinLength(2)]
        [MaxLength(50)]
        [Required]
        public string Name { get; set; } = null!;

        [Required]
        [MaxLength(14)]
        [RegularExpression("^\\(\\d{3}\\) \\d{3}-\\d{4}$\r\n")]
        public string PhoneNumber { get; set; } = null!;

        [Required]
        public bool IsNonStop  { get; set; }

        public virtual ICollection<Medicine> Medicines { get; set; } = null!;
    }
}
