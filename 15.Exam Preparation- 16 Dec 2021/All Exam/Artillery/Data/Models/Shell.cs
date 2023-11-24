using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artillery.Data.Models
{
    public class Shell
    {
        public Shell()
        {
            Guns = new HashSet<Gun>();
        }
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(1_680)]
        public string ShellWeight { get; set; }

        [Required]
        [MaxLength(40)]
        public string Caliber { get; set; } = null!;

        public virtual ICollection<Gun> Guns { get; set; }
    }

}
