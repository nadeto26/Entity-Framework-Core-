using SoftJail.Data.Models;
using SoftJail.Data.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Xml.Serialization;
using System.Xml.Linq;

namespace SoftJail.DataProcessor.ImportDto
{
    [XmlType(nameof(Officer))]
    public class ImportOfficersPrisonersDto
    {
        [Required]
        [MaxLength(30)]
        [MinLength(3)]
        [XmlAnyElement("Name")]
        public string FullName { get; set; }

        [Required]
        [XmlAnyElement("Money")]
        [MinLength(0)]
        public decimal Salary { get; set; }

        [Required]
        [XmlAnyElement("Position")]
        public Position Position { get; set; }

        [Required]
        [XmlAnyElement("Weapon")]
        public Weapon Weapon { get; set; }

        [Required]
        [XmlAnyElement("DepartmentId")]
        [ForeignKey(nameof(Department))]
        public int DepartmentId { get; set; }

        [XmlArray("Prisoners")]
        public ImportPrisonersDto[] Prisoners { get; set; }
    }
}
