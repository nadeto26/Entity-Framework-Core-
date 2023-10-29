using P02_FootballBetting.Data.Models.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P02_FootballBetting.Data.Models
{
    public class User
    {
        public User()
        {
            this.Bets = new HashSet<Bet>();
        }

        [Key]
        public int UserId { get; set; }

        [MaxLength(ValidationConstants.UserUsernameMaxLength)]
        public string Username { get; set; } = null!;

        // Password are saved hashed in the DB
        [MaxLength(ValidationConstants.UserPasswordMaxLength)]
        public string Password { get; set; } = null!;

        [MaxLength(ValidationConstants.UserEmailMaxLength)]
        public string Email { get; set; } = null!;

        [MaxLength(ValidationConstants.UserNameMaxLength)]
        public string Name { get; set; } = null!;

        public decimal Balance { get; set; }

        public virtual ICollection<Bet> Bets { get; set; }
    }
}
