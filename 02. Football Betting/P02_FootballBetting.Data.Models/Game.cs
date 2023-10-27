﻿using P02_FootballBetting.Data.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P02_FootballBetting.Data.Models
{
    public class Game 
    {
        public int GameId { get; set; }

        public int HomeTeamId { get; set; }

        public int AwayTeamId { get; set; }

        public int HomeTeamGoals { get; set; }

        public int AwayTeamGoals { get; set; }


        //DateTime is required by default
        //DateTime? is nullable
        public DateTime DateTime { get; set; }


        //double is required by default 
        public double HomeTeamBetRate { get; set; }

        public double  AwayTeamBetRate { get; set; }

        public double DrawBetRate { get; set; }

        [MaxLength(ValidationConstants.GameResultMaxLength)]
        public string? Result { get; set; }
    }
}
