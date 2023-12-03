using Medicines.Data.Models;
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
    public class ImportPharmacyDTO
    {
        [XmlElement("Name")]
        [Required]
        [MaxLength(50)]
        [MinLength(2)]
        public string Name { get; set; } = null!;

        [XmlElement("PhoneNumber")]
        [Required]
        [MaxLength(14)]
        [RegularExpression(@"^\(\d{3}\) \d{3}-\d{4}$")]
        public string PhoneNumber { get; set; } = null!;

        [XmlAttribute("non-stop")]
        [Required]
        public string IsNonStop { get; set; } = null!;

        [XmlArray("Medicines")]
        public virtual ImportMedicineDTO[] Medicines { get; set; }
    }
}

//· Id – integer, Primary Key

//· Name – text with length [2, 50] (required)

//· PhoneNumber – text with length 14. (required)

//o All phone numbers must have the following structure: three digits enclosed in parentheses, followed by a space, three more digits, a hyphen, and four final digits:

//§ Example-> (123) 456 - 7890

//· IsNonStop – bool (required)

//· Medicines - collection of type Medicine