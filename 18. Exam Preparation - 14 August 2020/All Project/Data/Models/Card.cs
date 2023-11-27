using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VaporStore.Data.Models.Enums;

namespace VaporStore.Data.Models
{
    public class Card
    {
        public Card()
        {
            Purchases = new HashSet<Purchase>();
        }
        [Key]
        public int Id { get; set; }

        [RegularExpression(@"^\d{4} \d{4} \d{4} \d{4}$")]
        [Required]
        public string Number { get; set; }

        [RegularExpression(@"^\d{3}$")]
        [Required]
        public string Cvc { get; set; }


        [Required]
        public CardType Type { get; set; }

        [Required]
        [ForeignKey(nameof(User))]
        public int UserId { get; set; }

        public virtual User User { get; set; } = null!;

        public virtual ICollection<Purchase> Purchases { get; set; } = null!;
    }

}
