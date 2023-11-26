// ReSharper disable InconsistentNaming

namespace TeisterMask.DataProcessor
{
    using System.ComponentModel.DataAnnotations;
    using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;

    using Data;
    using TeisterMask.Utilities;
    using System.Text;
    using System.Reflection.Metadata;
    using TeisterMask.DataProcessor.ImportDto;
    using TeisterMask.Data.Models;
    using System.Collections.Generic;
    using TeisterMask.Data.Models.Enums;
    using System.Globalization;
    using System.Xml.Linq;
    using Microsoft.VisualBasic;
    using Newtonsoft.Json;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedProject
            = "Successfully imported project - {0} with {1} tasks.";

        private const string SuccessfullyImportedEmployee
            = "Successfully imported employee - {0} with {1} tasks.";

        XmlHelper xmlHelper;
        public static string ImportProjects(TeisterMaskContext context, string xmlString)
        {
             XmlHelper xmlHelper = new XmlHelper();
             StringBuilder sb = new StringBuilder();

            ImportProjectDto[] prDtos = xmlHelper.Deserialize<ImportProjectDto[]>(xmlString, "Projects");
            ICollection<Project> validProjects = new HashSet<Project>();    

            foreach(ImportProjectDto pdDto in prDtos)
            {
                if(!IsValid(pdDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                
                var taskOpenDate = DateTime.ParseExact(pdDto.OpenDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                var taskDueDate = DateTime.ParseExact(pdDto.DueDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                ICollection<Task> validTasks = new HashSet<Task>();
                foreach(ImportTaskDto tDto in pdDto.Tasks)
                {
                    if(!IsValid(tDto)  || taskDueDate < taskOpenDate || taskDueDate> taskOpenDate )
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    Task task = new Task()
                    {
                        Name = tDto.Name,
                        OpenDate = DateTime.ParseExact(tDto.OpenDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                        DueDate = DateTime.ParseExact(tDto.DueDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                        ExecutionType = (ExecutionType)Enum.Parse(typeof(ExecutionType), tDto.ExecutionType),
                        LabelType = (LabelType)Enum.Parse(typeof(LabelType), tDto.LabelType)
                    };
                    validTasks.Add(task);
                 
                }

                Project project = new Project()
                {
                    Name = pdDto.Name,
                    OpenDate = DateTime.ParseExact(pdDto.OpenDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                    DueDate = DateTime.ParseExact(pdDto.DueDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                    Tasks = validTasks
                };
                sb.AppendLine(string.Format(SuccessfullyImportedProject, project.Name, validTasks.Count()));



            }

            context.Projects.AddRange(validProjects);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportEmployees(TeisterMaskContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();

            ImportEmployeeDto[] eDtos = JsonConvert.DeserializeObject<ImportEmployeeDto[]>(jsonString);

            ICollection<Employee> validEmployyess = new HashSet<Employee>();
            ICollection<int> existingTaksIds = context.EmployeesTasks
                .Select(t => t.TaskId)
                .ToArray();

            foreach(ImportEmployeeDto eDto in eDtos)
            {
                if(!IsValid(eDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;

                }

                Employee employee = new Employee()
                {
                    Username = eDto.Username,
                    Email = eDto.Email,
                    Phone = eDto.Phone
                };

                foreach(int TasksId in eDto.Tasks.Distinct())
                {
                    if(!existingTaksIds.Contains(TasksId))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    EmployeeTask task = new EmployeeTask()
                    {
                        Employee = employee,
                        TaskId = TasksId
                    };

                    employee.EmployeesTasks.Add(task);
                }

                validEmployyess.Add(employee);
                sb.AppendLine(string.Format(SuccessfullyImportedEmployee, employee.Username, employee.EmployeesTasks.Count()));
            }

            context.Employees.AddRange(validEmployyess);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}