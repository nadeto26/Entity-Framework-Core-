using P02_FootballBetting.Data.Common;
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
        [Key]
        public int UserId { get; set; }

        [Required]
        [MaxLength(ValidationConstants.UserNameMaxLength)]
        public  string Username { get; set; }

        [Required]
        [MaxLength(ValidationConstants.PasswordMaxLength)]

        public int Password { get; set; }

        [MaxLength(ValidationConstants.UserEmailMaxLength)]

        public int Email { get; set; }

        [Required]
        [MaxLength(ValidationConstants.NameMaxLength)]
        public int Name { get; set; }

        //required by default
        public decimal Balance  { get; set; }
    }
}
