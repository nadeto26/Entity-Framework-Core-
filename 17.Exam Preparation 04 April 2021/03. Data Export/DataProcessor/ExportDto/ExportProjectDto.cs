using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TeisterMask.DataProcessor.ExportDto
{
    [XmlType("Project")]
    public class ExportProjectDto
    {
        [XmlAttribute("TasksCount")]
        public int TasksCount { get; set; }

        [XmlElement("ProjectName")]
        public string Name { get; set; } = null!;

        [XmlElement("HasEndDate")]
        public string DueDate { get; set; }

        [XmlArray("Task")]
        public ExportTaskDto[] Task { get; set; }
    }
}
