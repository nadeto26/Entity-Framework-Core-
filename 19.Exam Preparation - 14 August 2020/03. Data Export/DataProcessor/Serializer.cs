namespace SoftJail.DataProcessor
{
    using Data;
    using Newtonsoft.Json;
    using SoftJail.Data.Models;
    using SoftJail.Utilities;

    public class Serializer
    {
        public static string ExportPrisonersByCells(SoftJailDbContext context, int[] ids)
        {
            var prisoners = 
                context.Prisoners
                .ToArray()
                .Where(p=>ids.Contains(p.Id))
                .ToArray()
                .Select(p=> new
                {
                    Id = p.Id,
                    Name = p.FullName,
                    CellNumber = p.Cell.CellNumber,
                    Officer = p.PrisonerOfficers
                    .Select(o=> new
                    {
                        Name = o.Officer.FullName,
                        DepartmentName = o.Officer.Department.Name
                    })
                     .OrderBy(o => o.Name)
                     .ToArray(),
                     TotalSalary = Math.Round(p.PrisonerOfficers.Select(of=>of.Officer.Salary).Sum(),2)
                    })
                .ToArray()
                 .OrderBy(p => p.Name)
                .ThenBy(p => p.Id)
                .ToArray();

            string json = JsonConvert.SerializeObject(prisoners, Formatting.Indented);

            return json;


        }

        private static XmlHelper xmlHelper;
        public static string ExportPrisonersInbox(SoftJailDbContext context, string prisonersNames)
        {
            xmlHelper = new XmlHelper();

            string[] prisonerNamesArr = prisonersNames
               .Split(",")
               .ToArray();
            var prisoner = context.Prisoners
                 .Where(p=>prisonerNamesArr.Contains(p.Nickname)) 
                 .ToArray()
                 .Select(p=> new
                 {
                     Id = p.Id,
                     Name = p.FullName,
                     IncarcerationDate = p.IncarcerationDate.ToString("yyyy-MM-dd"),
                     Mails = p.Mails
                     .ToArray()
                     .Select(m=>m.Description.Reverse())
                     .ToArray(),
                    })
                 .OrderBy (p => p.Name) 
                 .ThenBy (p => p.Id)
                 .ToArray();
            return xmlHelper.Serialize(prisoner, "Prisoners");
        }
    }
}