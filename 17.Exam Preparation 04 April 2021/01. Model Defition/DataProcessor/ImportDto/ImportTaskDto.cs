using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using TeisterMask.Data.Models.Enums;

namespace TeisterMask.DataProcessor.ImportDto
{
    [XmlType("Tasks")]
    public class ImportTaskDto
    {

        [MinLength(2)]
        [MaxLength(40)]
        [Required]
        public string Name { get; set; } = null!;

        [Required]
        public string OpenDate { get; set; }

        [Required]
        public string DueDate { get; set; }

        [Required]
        public string ExecutionType { get; set; }

        [Required]
        public string LabelType { get; set; }

    }
}
