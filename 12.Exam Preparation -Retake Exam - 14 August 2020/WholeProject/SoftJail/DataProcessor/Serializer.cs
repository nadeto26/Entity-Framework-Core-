namespace SoftJail.DataProcessor
{
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Data;
    using Newtonsoft.Json;
    using SoftJail.Data.Models;
    using SoftJail.DataProcessor.ExportDto;
    using System;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;

    public class Serializer
    {
        public static string ExportPrisonersByCells(SoftJailDbContext context, int[] ids)
        {
             var prisoners = 
                context.Prisoners
                .Where(p=>ids.Contains(p.Id))
                .Select(p=> new
                {
                    Id = p.Id,
                    Name = p.Nickname,
                    CellNumber = p.Cell.CellNumber,
                    Officers = p.PrisonerOfficers.Select(pf => new
                    {
                        Name = pf.Officer.FullName,
                        DepartmentName = pf.Officer.Department.Name
                    })
                    .OrderBy(o=>o.Name)
                    .ToArray(),
                    TotalSalary = p.PrisonerOfficers.Sum(p=>p.Officer.Salary).ToString(":F2") //sumata ot zaplatite na vsichki officers
                })
                .OrderBy (o=>o.Name)
                .ThenBy(of=>of.Id)
                .ToArray();

            string json =JsonConvert.SerializeObject(prisoners,Formatting.Indented);
            return json;
        }

        public static string ExportPrisonersInbox(SoftJailDbContext context, string prisonersNames)
        {

            string[] prisonerArray = prisonersNames.Split(',');
            ExportPrisonerDto[] prisonerDtos = context.Prisoners
                .Where(p=>prisonerArray.Contains(p.FullName))
                .ProjectTo<ExportPrisonerDto>(Mapper.Configuration)
                .OrderBy(p =>p.Name)
                .ThenBy (of => of.Id)
                .ToArray();

            StringBuilder stringBuilder = new StringBuilder();
            using StringWriter sw = new StringWriter(stringBuilder); //ще пише вместо нас в sb

            XmlRootAttribute rootAttribute = new XmlRootAttribute("Prisoners");
            XmlSerializer xmlSerializer =
                new XmlSerializer(typeof(ExportPrisonerDto[]), rootAttribute);

            //за премахването на текста под мета данните 
            XmlSerializerNamespaces nameSpaces = new XmlSerializerNamespaces();
            nameSpaces.Add(string.Empty, string.Empty);

            //serialize

            xmlSerializer.Serialize(sw,prisonerDtos, nameSpaces);
            return stringBuilder.ToString();
 

        }
    }
}