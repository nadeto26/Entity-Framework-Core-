using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeisterMask.Data.Models
{
    public class Project
    {
        public Project()
        {
            Tasks = new HashSet<Task>();
        }
        [Key]
        public int Id { get; set; }

        [MinLength(2)]
        [MaxLength(40)]
        [Required]
        public string Name { get; set; } = null!;

        [Required]
        public DateTime OpenDate { get; set; }

        public DateTime? DueDate { get; set; } 

        public virtual ICollection<Task> Tasks { get; set; }

    }
}
