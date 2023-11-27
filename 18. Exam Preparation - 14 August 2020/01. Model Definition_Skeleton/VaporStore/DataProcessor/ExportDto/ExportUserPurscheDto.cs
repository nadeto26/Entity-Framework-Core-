using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace VaporStore.DataProcessor.ExportDto
{
    [XmlType("Purchase")]
    public class ExportUserPurscheDto
    {
        [Required]
        [XmlElement("Card")]
        public string CardNumber { get; set; }

        [XmlElement("Cvc")]
        public int Cvc { get; set; }

        [Required]
        [XmlElement("Date")]
        public string Date { get; set; }

        [XmlArray("Game")]
        public ExportUserGame[] game { get; set; }





    }
}
