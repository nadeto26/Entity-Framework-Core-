using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VaporStore.Data.Models
{

    public class Game
    {
        public Game()
        {
            Purchases = new HashSet<Purchase>();
            GameTags = new HashSet<GameTag>();
        }
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        [MinLength(0)]
        [Required]
        public decimal Price { get; set; }

        [Required]
        public DateTime ReleaseDate { get; set; }

        [Required]
        [ForeignKey(nameof(Developer))]
        public int DeveloperId { get; set; }

        public virtual Developer Developer { get; set; } = null!;

        [ForeignKey(nameof(Genre))]
        [Required]
        public int GenreId { get; set; }

        [Required]
        public virtual Genre Genre { get; set; } = null!;

        public virtual ICollection<Purchase> Purchases { get; set; }

        public virtual ICollection<GameTag> GameTags { get; set; }
    }


}
