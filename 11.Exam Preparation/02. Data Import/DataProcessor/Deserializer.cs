namespace Footballers.DataProcessor
{
    using Footballers.Data;
    using Footballers.Data.Models;
    using Footballers.Data.Models.Enums;
    using Footballers.DataProcessor.ImportDto;
    using Footballers.Utilities;
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;
    using System.Text;
    using System.Xml.Linq;

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

            CoachDto[] coachDtos = xmlHelper.Deserialize<CoachDto[]>(xmlString, "Coaches");

            ICollection<Coach> validCoaches = new HashSet<Coach>(); 

            foreach(CoachDto coachDto in coachDtos)
            {
                if(!IsValid(coachDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                ICollection<Footballer> validFootballer = new HashSet<Footballer>();    
                foreach(FootballerDto footballerDto in coachDto.footballers)
                {
                    if(!IsValid(footballerDto))
                    {
                        sb.AppendLine(ErrorMessage) ;
                        continue;
                    }

                    Footballer footballer = new Footballer()
                    {
                        Name = footballerDto.Name,
                        ContractStartDate =  footballerDto.ContractStartDate,
                        ContractEndDate = footballerDto.ContractEndDate,
                        BestSkillType = footballerDto.BestSkillType,
                        PositionType = footballerDto.PositionType

                    };
                    validFootballer.Add(footballer);
                }
                 Coach coach = new Coach()
                 {
                     Name = coachDto.Name,
                     Nationality = coachDto.Nationality,
                     Footballers = validFootballer
                 };

                validCoaches.Add(coach);
                sb.AppendLine(string.Format(SuccessfullyImportedCoach, coach.Name, validFootballer.Count()));

            }
            context.Coaches.AddRange(validCoaches);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportTeams(FootballersContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();
            ImportTeam[] teamDtos =
                JsonConvert.DeserializeObject<ImportTeam[]>(jsonString);

            ICollection<Team> validTeams= new HashSet<Team>();

            ICollection<int> existingFooballerId = context.Footballers
              .Select(t => t.Id)
              .ToArray();

            foreach (ImportTeam teamDto in teamDtos)
            {
                if(!IsValid(teamDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                if(teamDto.Trophies<=0)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Team team = new Team()
                {
                    Name = teamDto.Name,
                    Nationality=teamDto.Nationality,
                    Trophies = teamDto.Trophies,
                  
                };

                foreach(int footballerId in teamDto.FootballersId.Distinct())
                {
                    if(!existingFooballerId.Contains(footballerId))
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
                validTeams.Add(team);
                sb.AppendLine(String.Format(SuccessfullyImportedTeam, team.Name, team.TeamsFootballers.Count));
                    
            }
            context.Teams.AddRange(validTeams);
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
