namespace TeisterMask.DataProcessor
{
    using Data;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.VisualBasic;
    using Newtonsoft.Json;
    using System.Globalization;
    using System.Net.NetworkInformation;
    using System.Text;
    using System.Xml.Linq;
    using System.Xml.Serialization;
    using TeisterMask.DataProcessor.ExportDto;
    using TeisterMask.Utilities;

    public class Serializer
    {
        private static XmlHelper xmlHelper;
        public static string ExportProjectWithTheirTasks(TeisterMaskContext context)
        {
            StringBuilder sb = new StringBuilder();

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ExportProjectDto[]), new XmlRootAttribute("Projects"));

            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);

            using StringWriter sw = new StringWriter(sb);

            ExportProjectDto[] projects = context
                .Projects
                .Where(p => p.Tasks.Any())
                .ToArray()
            .Select(p => new ExportProjectDto()
            {
                Name = p.Name,
                DueDate = p.DueDate.HasValue ? "Yes" : "No",
                TasksCount = p.Tasks.Count,
                Task = p.Tasks
                    .ToArray()
                    .Select(t => new ExportTaskDto()
                    {
                        Name = t.Name,
                        LabelType = t.LabelType.ToString()
                    })
                    .OrderBy(t => t.Name)
                    .ToArray()
            })
            .OrderByDescending(p => p.TasksCount)
            .ThenBy(p => p.Name)
            .ToArray();

             

            xmlSerializer.Serialize(sw, projects, namespaces);

            return sb.ToString().TrimEnd();


        }



        public static string ExportMostBusiestEmployees(TeisterMaskContext context, DateTime date)
        {
            var employees = context.Employees


              .Where(e => e.EmployeesTasks.Any(t => t.Task.OpenDate >= date))
               .ToArray()
              .Select(e => new
              {
                  Name = e.Username,
                  Task = e.EmployeesTasks
                  .Where(t => t.Task.OpenDate >= date)
                  .Select(t => new
                  {
                      Name = t.Task.Name,
                      OpenDate = t.Task.OpenDate.ToString("d", CultureInfo.InvariantCulture),
                      DueDate = t.Task.DueDate.ToString("d", CultureInfo.InvariantCulture),
                      Label = t.Task.LabelType.ToString(),
                      ExecutionType = t.Task.ExecutionType.ToString(),

                  })
                  .OrderByDescending(t => t.DueDate)
                  .ThenBy(t => t.Name)
                  .ToArray()
              })
              .OrderBy(e => e.Task.Count())
              .ThenBy(e => e.Name)
              .Take(10)
              .ToArray();

            return JsonConvert.SerializeObject(employees, Formatting.Indented);
        }
    }
}