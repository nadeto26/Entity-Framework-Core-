using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CarDealer.DTOs.Import
{

    [XmlType("Customer")]
    public class ImportCustomersDto
    {
        [XmlElement("name")]
        public string Name { get; set; } = null!;

        // Always read datetime, enums and other hard to parse data types as string
        // Parse it yourself in your business logic
        // JsonConvert and XmlSerializer are not capable of parsing!!!
        [XmlElement("birthDate")]
        public string BirthDate { get; set; } = null!;

        [XmlElement("isYoungDriver")]
        public bool IsYoungDriver { get; set; }
    }
}
