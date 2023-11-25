using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Theatre.Data.Models
{
    public class Ticket
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MinLength(1_00)]
        [MaxLength(100_00)]
        public decimal Price { get; set; }

        [Required]
        [MinLength(1)]
        [MaxLength(10)]
        public sbyte RowNumber  { get; set; }

        [Required]
        [ForeignKey(nameof(Play))]
        public int PlayId { get; set; }

        public virtual Play Play { get; set; } = null!;

        [Required]
        [ForeignKey(nameof(Theatre))]
        public int TheatreId { get; set; }

        public virtual Theatre Theatre { get; set; } = null!;
    }
}
