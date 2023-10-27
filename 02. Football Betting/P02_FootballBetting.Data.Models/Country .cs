﻿using P02_FootballBetting.Data.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P02_FootballBetting.Data.Models
{
    public class Country
    {
        [Key]
        public int CountryId { get; set; }


        [Required] //NotNull
        [MaxLength(ValidationConstants.ColorNameMaxLength)]
        public string Name { get; set; }
    }
}
