using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Artillery.DataProcessor.ExportDto
{
    [XmlType("Country")]
    public class ExportCountryDto
    {
        [XmlElement("Country")]
        public string Country { get; set; }

        [XmlElement("ArmySize")]
        public int ArmySize { get; set; }
    }
}
