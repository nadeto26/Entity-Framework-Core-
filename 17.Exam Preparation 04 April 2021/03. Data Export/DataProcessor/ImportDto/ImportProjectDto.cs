using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TeisterMask.DataProcessor.ImportDto
{
    [XmlType("Project")]
    public class ImportProjectDto
    {
        [MinLength(2)]
        [MaxLength(40)]
        [Required]
        public string Name { get; set; } = null!;

        [Required]
        public string OpenDate { get; set; }

        public string DueDate { get; set; }

        public ImportTaskDto[] Tasks { get; set; }
    }
}
