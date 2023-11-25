using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Theatre.Data.Models.Enums;

namespace Theatre.DataProcessor.ImportDto
{
    [XmlType("Play")]
    public class ImportPlayDto
    {
        [MinLength(4)]
        [MaxLength(50)]
        [Required]
        [XmlElement("Title")]
        public string Title { get; set; } = null!;

        [Required]
        [MinLength(1)]
        [XmlElement("Duration")]
        public TimeSpan Duration { get; set; }

        [Required]
        [MinLength(0_00)]
        [MaxLength(10_00)]
        [XmlElement("Rating")]
        public float Rating { get; set; }

        [Required]
        [XmlElement("Genre")]
        public Genre Genre { get; set; }

        [Required]
        [MaxLength(700)]
        [XmlElement("Description")]
        public string Description { get; set; } = null!;

        [Required]
        [MinLength(4)]
        [MaxLength(30)]
        [XmlElement("Screenwriter")]
        public string Screenwriter { get; set; } = null!;
    }
}
