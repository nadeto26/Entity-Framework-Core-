using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace VaporStore.DataProcessor.ExportDto
{
    [XmlType("User")]
    public class ExportUserDto
    {
        [XmlAttribute]
        public string UserName { get; set; }

        [XmlArray("Purchase")]
        public ExportUserPurscheDto[] Purchase { get; set; }

        [XmlElement("TotalSpent")]
        public int TotalSpent { get; set; }


    }
}
