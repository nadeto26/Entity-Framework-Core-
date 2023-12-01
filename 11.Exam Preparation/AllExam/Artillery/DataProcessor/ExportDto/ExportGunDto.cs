using Artillery.Data.Models.Enums;
using Artillery.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Artillery.DataProcessor.ExportDto
{
    [XmlType("Gun")]
    public class ExportGunDto
    {
        [XmlElement("Manufacturer")]
        public string Manufacturer { get; set; }

        [XmlElement("GunWeight")]
        public int GunWeight { get; set; }

        [XmlElement("BarrelLength")]
        public double BarrelLength { get; set; }

        [XmlElement("Range")]
        public int Range { get; set; }

        [XmlElement("GunType")]
        public string GunType { get; set; }

        [XmlArray("Countries")]
        public ExportCountryDto[] Countries { get; set; }
    }
}
