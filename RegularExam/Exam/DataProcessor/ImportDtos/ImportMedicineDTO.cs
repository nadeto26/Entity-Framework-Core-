using Medicines.Data.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Medicines.DataProcessor.ImportDtos
{
    [XmlType("Medicine")]
    public class ImportMedicineDTO
    {
        [XmlElement("Name")]
        [Required]
        [StringLength(150)]
        [MinLength(3)]
        public string Name { get; set; } = null!;

        [XmlElement("Price")]
        [Required]
        [Range((double)0.01m, (double)1000.00m)]
        public decimal Price { get; set; }

        [XmlAttribute("category")]
        [Required]
        [Range(0,4)]
        public int Category { get; set; }

        [XmlElement("ProductionDate")]
        [Required]
        public string? ProductionDate { get; set; }

        [XmlElement("ExpiryDate")]
        [Required]
        public string? ExpiryDate { get; set; }

        [XmlElement("Producer")]
        [Required]
        [StringLength(100)]
        [MinLength(3)]
        public string Producer { get; set; } = null!;
    }
}
//· Name – text with length [3, 150] (required)

//· Price – decimal in range [0.01…1000.00] (required)

//· Category – Category enum (Analgesic = 0, Antibiotic, Antiseptic, Sedative, Vaccine)(required)

//· ProductionDate – DateTime (required)

//· ExpiryDate – DateTime (required)

//· Producer – text with length [3, 100] (required)

//· PharmacyId – integer, foreign key (required)

//· Pharmacy – Pharmacy

//· PatientsMedicines - collection of type PatientMedicine