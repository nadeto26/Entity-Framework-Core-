using P02_FootballBetting.Data.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P02_FootballBetting.Data.Models
{
    public class Player
    {
        [Key]
        public int PlayerId { get; set; }

        [Required]
        [MaxLength(ValidationConstants.PlayerNameMaxLength)]
        public string Name { get; set; }

        public int SquadNumber { get; set; }

       

        // SQL Type -> BIT
        // By default bool is NOT NULL, by default is required
        public bool IsInjured { get; set; }

        // This FK should not be NOT NULL
        // Warning: This may cause a problem in Judge!!!
        
        public int TeamId { get; set; }

    }
}
