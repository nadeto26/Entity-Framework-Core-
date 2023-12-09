using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System;

namespace Trucks.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Trucks.Common;
    using Trucks.Data.Models.Enums;

    public class Truck
    {
        public Truck()
        {
            ClientsTrucks = new HashSet<ClientTruck>();
        }

        [Key]
        public int Id { get; set; }

        [MaxLength(ValidationConstants.RegistrationNumberLength)]
        public string? RegistrationNumber { get; set; }

        [Required]
        [MaxLength(ValidationConstants.VinNumberMaxLength)]
        public string VinNumber { get; set; } = null!;

        public int TankCapacity { get; set; }

        public int CargoCapacity { get; set; }

        public CategoryType CategoryType { get; set; }

        public MakeType MakeType { get; set; }

        [ForeignKey(nameof(Despatcher))] //-> принадлежи на навигационното пропърти и така ги навързваме 
        public int DespatcherId { get; set; }

        //navigation property

        public virtual Despatcher Despatcher { get; set; } = null!;

        public virtual ICollection<ClientTruck> ClientsTrucks { get; set; } = null!;


    }
}