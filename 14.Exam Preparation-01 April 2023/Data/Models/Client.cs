using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trucks.Common;

namespace Trucks.Data.Models
{
    public class Client
    {
        public Client()
        {
            ClientsTrucks = new HashSet<ClientTruck>();
        }
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(ValidationConstants.ClientNameMaxLength)]
        public string Name { get; set; } = null!; // няма да има null стойности 

        [Required]
        [MaxLength(ValidationConstants.ClientNationnalittityMaxLength)]
        public string Nationality { get; set; } = null!;

        [Required]
        public string Type { get; set; } = null!;

       public virtual ICollection<ClientTruck> ClientsTrucks { get; set; } = null!;
    }
}
