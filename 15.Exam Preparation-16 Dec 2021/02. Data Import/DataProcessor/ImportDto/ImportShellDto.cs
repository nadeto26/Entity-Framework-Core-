using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Artillery.DataProcessor.ImportDto
{
    [XmlType("Shell")]
    public class ImportShellDto
    {
        [Required]
        [MaxLength(1_680)]
        [MinLength(2)]
        [XmlElement("ShellWeight")]
        public string ShellWeight { get; set; }

        [Required]
        [MaxLength(40)]
        [MinLength(4)]
        [XmlElement("Caliber")]
        public string Caliber { get; set; }
    }

}
