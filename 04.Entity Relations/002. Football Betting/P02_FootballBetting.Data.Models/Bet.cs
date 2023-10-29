using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using P02_FootballBetting.Data.Models.Enums;

namespace P02_FootballBetting.Data.Models
{
    public class Bet
    {
        [Key]
        public int BetId { get; set; }

        public decimal Amount { get; set; }

        // Enumerations are stored as integer (non-nullable)
        public Prediction Prediction { get; set; }

        public DateTime DateTime { get; set; }

        [ForeignKey(nameof(User))]
        public int UserId { get; set; }

        public virtual User User { get; set; } = null!;

        [ForeignKey(nameof(Game))]
        public int GameId { get; set; }

        public virtual Game Game { get; set; } = null!;
    }
}