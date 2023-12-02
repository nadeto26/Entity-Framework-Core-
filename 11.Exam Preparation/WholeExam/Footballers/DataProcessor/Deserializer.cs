namespace Footballers.DataProcessor
{
    using Castle.Core.Internal;
    using Footballers.Data;
    using Footballers.Data.Models;
    using Footballers.Data.Models.Enums;
    using Footballers.DataProcessor.ImportDto;
    using Footballers.Utilities;
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Text;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedCoach
            = "Successfully imported coach - {0} with {1} footballers.";

        private const string SuccessfullyImportedTeam
            = "Successfully imported team - {0} with {1} footballers.";

        private static XmlHelper xmlHelper;
        public static string ImportCoaches(FootballersContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();
            xmlHelper = new XmlHelper();
            ImportCoachesDto[] cDtos = xmlHelper.Deserialize<ImportCoachesDto[]>(xmlString, "Coaches");

            ICollection<Coach> validCoaches = new HashSet<Coach>();

            foreach (ImportCoachesDto cDto in cDtos)
            {
                if (!IsValid(cDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                if (cDto.Nationality.IsNullOrEmpty())
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                ICollection<Footballer> validFootballers = new HashSet<Footballer>();

                foreach (ImportFootballerDto fDto in cDto.Footballers)
                {
                    if (!IsValid(fDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    DateTime releaseDate;
                    bool isReleaseDateValid = DateTime.TryParseExact(fDto.ContractStartDate, "dd/MM/yyyy",
                        CultureInfo.InvariantCulture, DateTimeStyles.None, out releaseDate);

                    if (!isReleaseDateValid)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    DateTime contractEndDate;
                    bool isContractEndDateValid = DateTime.TryParseExact(fDto.ContractEndDate, "dd/MM/yyyy",
                        CultureInfo.InvariantCulture, DateTimeStyles.None, out contractEndDate);

                    if (!isContractEndDateValid)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    if (contractEndDate < releaseDate)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    Footballer footballer = new Footballer()
                    {
                        Name = fDto.Name,
                        ContractEndDate = contractEndDate,
                        ContractStartDate = releaseDate,
                        BestSkillType = (BestSkillType)fDto.BestSkillType,
                        PositionType = (PositionType)fDto.PositionType
                    };
                    validFootballers.Add(footballer);

                }

                Coach coach = new Coach()
                {
                    Name = cDto.Name,
                    Nationality = cDto.Nationality,
                    Footballers = validFootballers
                };
                validCoaches.Add(coach);
                sb.AppendLine(string.Format(SuccessfullyImportedCoach, coach.Name, validFootballers.Count()));
            }
            context.Coaches.AddRange(validCoaches);
            context.SaveChanges();

            return sb.ToString().TrimEnd();


        }

        public static string ImportTeams(FootballersContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();
            ImportTeamDto[] tDtos = JsonConvert.DeserializeObject<ImportTeamDto[]>(jsonString); 

            ICollection<Team> validTeam = new HashSet<Team>();   
            ICollection<int> existingFootballers = context.Footballers
                .Select(f=>f.Id).ToArray();


            foreach (ImportTeamDto tDto in tDtos)
            {
                if(!IsValid(tDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
               
                Team team = new Team()
                {
                    Name = tDto.Name,
                    Nationality = tDto.Nationality,
                    Trophies = tDto.Trophies
                };

                if (tDto.Trophies == 0)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                int footballersCount = tDto.Footballers.Distinct().Count();
                foreach (int footballerId in tDto.Footballers.Distinct())
                {
                    if(!existingFootballers.Contains(footballerId))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    TeamFootballer teamFootballer = new TeamFootballer()
                    {
                        Team = team,
                        FootballerId = footballerId

                    };
                    team.TeamsFootballers.Add(teamFootballer);
                    
                }
                validTeam.Add(team);
                sb.AppendLine(string.Format(SuccessfullyImportedTeam, team.Name, team.TeamsFootballers.Count));


            }
            context.Teams.AddRange(validTeam);
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
