
 
using SoftUni.Data;
using SoftUni.Models;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace SoftUni;

public class StartUp
{
    static void Main(string[] args)
    {
        SoftUniContext context = new SoftUniContext();
        string result = GetEmployeesByFirstNameStartingWithSa(context);
        Console.WriteLine(result);
    }

    //problem3

    public static string GetEmployeesFullInformation(SoftUniContext context)
    {

        StringBuilder sb = new StringBuilder();
        var employees = context.Employees
              .OrderBy(e => e.EmployeeId).ToArray();

        foreach (var e in employees)
        {
            sb.AppendLine($"{e.FirstName} {e.LastName} {e.MiddleName} {e.JobTitle} {e.Salary:F2}");
        }

        return sb.ToString();
    }

    //problem5

    public static string GetEmployeesFromResearchAndDevelopment(SoftUniContext context)
    {
        StringBuilder sb = new StringBuilder();
        var employyeRnD = context.Employees
            .Where(e => e.Department.Name == "Research and Development")
            .Select(e => new
            {
                e.FirstName,
                e.LastName,
                DepartmentName = e.Department.Name,
                e.Salary
            })
               .OrderBy(e => e.Salary)
               .ThenByDescending(e => e.FirstName)
               .ToArray();

        foreach (var e in employyeRnD)
        {
            sb.AppendLine($"{e.FirstName} {e.LastName} from {e.DepartmentName} - ${e.Salary:F2}");
        }

        return sb.ToString();
    }

    //problem 06

    public static string AddNewAddressToEmployee(SoftUniContext context)
    {
        Address newAddress = new Address()
        {
            AddressText = "Vitoshka 15",
            TownId = 4
        };



        Employee? employee = context.Employees
          .FirstOrDefault(e => e.LastName == "Nakov");
        employee.Address = newAddress;

        context.Addresses.Add(newAddress); // This is the way for adding into the db



        string[] employeeAddresses = context.Employees
         .OrderByDescending(e => e.AddressId)
         .Take(10)
         .Select(e => e.Address!.AddressText)
         .ToArray();
        return String.Join(Environment.NewLine, employeeAddresses);
    }

    //problem4

    public static string GetEmployeesWithSalaryOver50000(SoftUniContext context)
    {
        StringBuilder sb = new StringBuilder();

        var employeers = context.Employees
            .Where(e => e.Salary > 50000)
            .Select(e => new
            {
                e.FirstName,
                e.Salary
            })
            .OrderBy(e => e.FirstName).ToArray();

        foreach (var e in employeers)
        {
            sb.AppendLine($"{e.FirstName} - {e.Salary:F2}");
        }

        return sb.ToString();

    }

    //problem7

    public static string GetEmployeesInPeriod(SoftUniContext context)
    {
        StringBuilder sb = new StringBuilder();
        var employeesWithProjects = context.Employees
            //.Where(e => e.EmployeesProjects
            //    .Any(ep => ep.Project.StartDate.Year >= 2001 &&
            //               ep.Project.StartDate.Year <= 2003))
            .Take(10)
            .Select(e => new
            {
                e.FirstName,
                e.LastName,
                ManagerFirstName = e.Manager!.FirstName,
                ManagerLastName = e.Manager!.LastName,
                Projects = e.EmployeesProjects
                    .Where(ep => ep.Project.StartDate.Year >= 2001 &&
                                 ep.Project.StartDate.Year <= 2003)
                    .Select(ep => new
                    {
                        ProjectName = ep.Project.Name,
                        StartDate = ep.Project.StartDate.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture),
                        EndDate = ep.Project.EndDate.HasValue
                            ? ep.Project.EndDate.Value.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture)
                            : "not finished"
                    })
                    .ToArray()
            })
            .ToArray();

        foreach (var e in employeesWithProjects)
        {
            sb
                .AppendLine($"{e.FirstName} {e.LastName} - Manager: {e.ManagerFirstName} {e.ManagerLastName}");

            foreach (var p in e.Projects)
            {
                sb
                    .AppendLine($"--{p.ProjectName} - {p.StartDate} - {p.EndDate}");
            }
        }

        return sb.ToString().TrimEnd();
    }



    //Problem08

    public static string GetAddressesByTown(SoftUniContext context)
    {
        StringBuilder sb = new StringBuilder();
        var result = context.Addresses
          .OrderByDescending(a => a.Employees.Count)
          .ThenBy(a => a.Town.Name)
          .Take(10)
          .Select(a => new
          {
              Text = a.AddressText,
              Name = a.Town.Name,
              EmployeeCount = a.Employees.Count()

          }
         ).ToArray();

        foreach (var e in result)
        {
            sb.AppendLine($"{e.Text}, {e.Name} - {e.EmployeeCount} employees");
        }

        return sb.ToString().TrimEnd();
    }

    //problem09
    public static string GetEmployee147(SoftUniContext context)
    {
        StringBuilder sb = new StringBuilder();
        var employee = context.Employees
            .Where(e => e.EmployeeId == 147)
            .Select(a => new
            {
                FirstName = a.FirstName,
                LastName = a.LastName,
                JobTitle = a.JobTitle,
                
            }



            ) .ToArray();

        foreach (var e in employee)
        {
            sb.AppendLine($"{e.FirstName} {e.LastName} - {e.JobTitle}");
        }
        return sb.ToString().TrimEnd();
    }

    public static string GetDepartmentsWithMoreThan5Employees(SoftUniContext context)
    {
        StringBuilder sb = new StringBuilder();



        var department = context.Departments
          .Where(e => e.Employees.Count() > 5)
          .OrderBy(e => e.Employees.Count())
          .ThenBy(e => e.Name)
             .Select(e => new
             {
                 e.Name,
                 ManagerFirstName = e.Manager!.FirstName,
                 ManagerLastName = e.Manager!.LastName,
                    employees = e.Employees
              .Select(e => new
              {
                  e.FirstName,
                  e.LastName,
                  e.JobTitle
              })
              .OrderBy(e=>e.FirstName)
              .ThenBy(e=>e.LastName)
                  .ToArray()
                  
             })
          .ToArray();

        foreach (var depart in department)
        {
            sb.AppendLine($"{depart.Name} – {depart.ManagerFirstName} {depart.ManagerLastName}");
            foreach (var emp in depart.employees)
            {
                sb.AppendLine($"{emp.FirstName} {emp.LastName} - {emp.JobTitle}");
            }
        }

        return sb.ToString().TrimEnd();

    }


    //Problem11

  
 
        public static string GetLatestProjects(SoftUniContext context)
        {
            var projects = context.Projects.OrderByDescending(p => p.StartDate)
                .Take(10)
                .OrderBy(e => e.Name);

            var sb = new StringBuilder();
            foreach (var project in projects)
            {
                sb.AppendLine(project.Name);
                sb.AppendLine(project.Description);
                sb.AppendLine(project.StartDate.ToString("M/d/yyyy h:mm:ss tt"));
            }
            return sb.ToString().TrimEnd();

        }


    //problem13

    public static string GetEmployeesByFirstNameStartingWithSa(SoftUniContext context)
    {
        StringBuilder sb = new StringBuilder();
        var employee = context.Employees.Where(e => e.FirstName.StartsWith("Sa"))
            .OrderBy(e =>e.FirstName)
            .ThenBy(e =>e.LastName)
                .ToArray();


        foreach(var e in employee)
        {
            sb.AppendLine($"{e.FirstName} {e.LastName} - {e.JobTitle} - (${e.Salary:F2})");
        }

        return sb.ToString();

    }

    //Problem14
    public static string DeleteProjectById(SoftUniContext context)
    {
        // Delete all rows from EmployeeProject that refer to Project with Id = 2
        IQueryable<EmployeeProject> epToDelete = context.EmployeesProjects
            .Where(ep => ep.ProjectId == 2);
        context.EmployeesProjects.RemoveRange(epToDelete);

        Project projectToDelete = context.Projects.Find(2)!;
        context.Projects.Remove(projectToDelete);
        context.SaveChanges();

        string[] projectNames = context.Projects
            .Take(10)
            .Select(p => p.Name)
            .ToArray();
        return String.Join(Environment.NewLine, projectNames);
    }

}

















