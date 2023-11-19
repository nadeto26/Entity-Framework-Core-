namespace Footballers.DataProcessor
{
    using Data;
    using Footballers.Data.Models;
    using Footballers.Data.Models.Enums;
    using Footballers.DataProcessor.ExportDto;
    using Footballers.Utilities;
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json;

    public class Serializer
    {
        private static XmlHelper xmlHelper;
        public static string ExportCoachesWithTheirFootballers(FootballersContext context)
        {
            xmlHelper = new XmlHelper();

            ExportCoachDto[] exportCoachDtos =  context.Coaches
                .Where(c=>c.Footballers.Any())
                .Select(c=> new ExportCoachDto()
                {
                    Name = c.Name,
                    FootballersCount = c.Footballers.Count(),
                    Footballer = c.Footballers
                    .Select(ft=> new ExportFootballerDto()
                    {
                        Name = ft.Name,
                        Position = ft.PositionType.ToString()
                    })
                    .OrderBy(ft=>ft.Name)
                    .ToArray()
                })
                .OrderByDescending(c=>c.FootballersCount)
                .ThenBy(c=>c.Name)
                .ToArray();

            return xmlHelper.Serialize(exportCoachDtos, "Coaches");
        }

        public static string ExportTeamsWithMostFootballers(FootballersContext context, DateTime date)
        {
            var teams = context.Teams
              .AsNoTracking()
              .Take(5)
              .Where(t => t.TeamsFootballers.Any(tf => tf.Footballer.ContractStartDate >= date))
              .Select(t => new
              {
                  Name = t.Name,
                  Footballers = t.TeamsFootballers
                 .Where(tf => tf.Footballer.ContractStartDate >= date)
                 .Select(tf => new
                 {
                     Name = tf.Footballer.Name,
                     ContractStartDate = tf.Footballer.ContractStartDate,
                     ContractEndDate = tf.Footballer.ContractEndDate,
                     BestSkillType = tf.Footballer.BestSkillType.ToString(),
                     Position = tf.Footballer.PositionType.ToString(),
                     

                 })
                 .OrderByDescending(tf => tf.ContractEndDate)
                 .ThenBy(tf => tf.Name)
                 .ToArray()

                })
              .OrderByDescending(f => f.Footballers.Count())
              .ThenBy(f => f.Name)
               .ToArray();

            return JsonConvert.SerializeObject(teams, Formatting.Indented);

        }
    }
}
