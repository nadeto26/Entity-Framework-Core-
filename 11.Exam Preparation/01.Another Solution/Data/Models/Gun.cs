﻿using Artillery.Data.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artillery.Data.Models
{
    public class Gun
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey(nameof(Manufacturer))]
        public int ManufacturerId { get; set; }
        public virtual Manufacturer Manufacturer { get; set; }

        public int GunWeight { get; set; }

        public double BarrelLength { get; set; }
        public int? NumberBuild { get; set; }

        public int Range { get; set; }
        [Required]
        public GunType GunType { get; set; }

        [ForeignKey(nameof(ShellId))]
        public int ShellId { get; set; }
        public virtual Shell Shell { get; set; }

        public virtual ICollection<CountryGun> CountriesGuns { get; set; } = new HashSet<CountryGun>();
    }
}
