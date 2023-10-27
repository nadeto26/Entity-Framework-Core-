using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P02_FootballBetting.Data.Models
{
    public class PlayerStatistic
    {
        //композитен ключ -> следователно ще използваме FLUENTIP
        //for config
        public int GameId { get; set; }

        public int PlayerId { get; set; }

        //Judge can not be happy with that

        public byte ScoredGoals { get; set; }

        public byte Assists { get; set; }

        public byte MinutesPlayed { get; set; }
    }
}
