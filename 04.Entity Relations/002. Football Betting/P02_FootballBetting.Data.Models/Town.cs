﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using P02_FootballBetting.Data.Models.Common;

namespace P02_FootballBetting.Data.Models
{
    public class Town
    {
        public Town()
        {
            this.Teams = new HashSet<Team>();
        }

        [Key]
        public int TownId { get; set; }

        [MaxLength(ValidationConstants.TownNameMaxLength)]
        public string Name { get; set; } = null!;

        [ForeignKey(nameof(Country))]
        public int CountryId { get; set; }

        public virtual Country Country { get; set; } = null!;

        public virtual ICollection<Team> Teams { get; set; }
    }
}
