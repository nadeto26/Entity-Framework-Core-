using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace VaporStore.DataProcessor.ExportDto
{
    [XmlType("Game")]
    public class ExportUserGame
    {
        [XmlAttribute("title")]
        public string Title { get; set; }

        [XmlElement("Price")]
        public decimal Price { get; set; }

        [XmlElement("Genre")]
        public string Genre { get; set; }


    }
}
