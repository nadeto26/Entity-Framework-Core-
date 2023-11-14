using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CarDealer.DTOs.Import
{
    [XmlType("PartId")]
    public class ImportCarPartDto
    {
        //вече е атрибут , а не елемент 
        [XmlAttribute("id")]
        public int PartId { get; set; }
    }
}
