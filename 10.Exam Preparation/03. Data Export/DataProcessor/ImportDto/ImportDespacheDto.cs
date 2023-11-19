using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Trucks.Common;

namespace Trucks.DataProcessor.ImportDto
{
    [XmlType("Despatcher")]
    public class ImportDespacheDto
    {
        [XmlElement("Name")]
        [Required]
        [MaxLength(ValidationConstants.DespatcherNameMaxLength)]
        [MinLength(ValidationConstants.DespatcherNameMinLength)]
        public string Name { get; set; } = null!;

        [XmlElement("Position")]
        [Required]
        public string Position { get; set; } = null!;

        [XmlArray("Trucks")] //--> масив от вложените dto 
        public ImportTruckDto[] Trucks { get;set; } //-> колекция от вложените dto
    }
}
