using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace VaporStore.DataProcessor.ImportDto
{
    [XmlType("Purchase")]
    public class ImportPurchaseDto
    {
        [XmlElement("Type")]
        [Required]
        public string PurchaseType { get; set; }

        [Required]
        [RegularExpression(@"^[A-Z0-9]{4}-[A-Z0-9]{4}-[A-Z0-9]{4}$")]
        [XmlElement("Key")]
        public string ProductKey { get; set; } = null!;

        [Required]
        [XmlElement("Date")]
        public string Date { get; set; }

        [XmlAttribute("title")]
        public string Title { get; set; }

        [Required]
        [XmlAttribute("Card")]
        public string CardNumber { get; set; }


    }

}
