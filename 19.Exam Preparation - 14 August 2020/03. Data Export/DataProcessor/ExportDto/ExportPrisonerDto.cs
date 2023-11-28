using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SoftJail.DataProcessor.ExportDto
{
    [XmlType("Prisoner")]
    public class ExportPrisonerDto
    {
        [XmlElement("Id")]
        public int Id { get; set; }

        [XmlElement("Name")]
        public string FullName { get; set; }


        [XmlElement("IncarcerationDate")]
        public string IncarcerationDate { get; set; }

        [XmlElement("EncryptedMessages")]
        public ExportMailDto[] Mails { get; set; }
    }
}
