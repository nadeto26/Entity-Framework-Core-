using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Medicines.DataProcessor.ImportDtos
{
    [XmlType("Pharmacy")]
    public class ImportPharmacyDto
    {
        [MinLength(2)]
        [MaxLength(50)]
        [Required]
        [XmlElement("Name")]
        public string Name { get; set; } = null!;

        [Required]
        [MaxLength(14)]
        [RegularExpression("(\\(\\d{3}\\) )\\d{3}-\\d{4}")]
        [XmlElement("PhoneNumber")]
        public string PhoneNumber { get; set; } = null!;

        [Required]
        [XmlAttribute("non-stop")]
        public string IsNonStop { get; set; }

        [XmlArray("Medicines")]
        public ImportMedicineDto[] Medicines { get; set; }
    }
}
