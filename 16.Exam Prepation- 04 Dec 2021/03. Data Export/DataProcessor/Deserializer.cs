namespace Theatre.DataProcessor
{
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using Theatre.Data;
    using Theatre.Data.Models;
    using Theatre.Data.Models.Enums;
    using Theatre.DataProcessor.ImportDto;
    using Theatre.Utilities;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfulImportPlay
            = "Successfully imported {0} with genre {1} and a rating of {2}!";

        private const string SuccessfulImportActor
            = "Successfully imported actor {0} as a {1} character!";

        private const string SuccessfulImportTheatre
            = "Successfully imported theatre {0} with #{1} tickets!";



        private static XmlHelper xmlHelper;
        public static string ImportPlays(TheatreContext context, string xmlString)
        {
            xmlHelper = new XmlHelper();
            StringBuilder sb = new StringBuilder();

            var minimumTime = new TimeSpan(1, 0, 0);

            ImportPlayDto[] pDtos = xmlHelper.Deserialize<ImportPlayDto[]>(xmlString, "Plays");

            var validGenreType = new string[] { "Drama", "Comedy", "Romance", "Musical" };

            var validPlays = new List<Play>();


            foreach (ImportPlayDto playDto in pDtos)
            {
                var currentTime = TimeSpan.ParseExact(playDto.Duration, "c", CultureInfo.InvariantCulture);

                if (!IsValid(playDto) || !validGenreType.Contains(playDto.Genre) || currentTime < minimumTime)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Play play = new Play()
                {
                    Title = playDto.Title,
                    Duration = TimeSpan.ParseExact(playDto.Duration, "c", CultureInfo.InvariantCulture),
                    Rating = playDto.Rating,
                    Genre = (Genre)Enum.Parse(typeof(Genre), playDto.Genre),
                    Description = playDto.Description,
                    Screenwriter = playDto.Screenwriter

                };

                validPlays.Add(play);
                sb.AppendLine(string.Format(SuccessfulImportPlay, play.Title, play.Genre, play.Rating));
            }
            context.SaveChanges();

            context.Plays.AddRange(validPlays);
            return sb.ToString().TrimEnd();

        }

        public static string ImportCasts(TheatreContext context, string xmlString)
        {
            xmlHelper = new XmlHelper();

            var sb = new StringBuilder();

            ImportCasId[] csDtps = xmlHelper.Deserialize<ImportCasId[]>(xmlString, "Casts");

            ICollection<Cast> validCasts = new HashSet<Cast>();

            foreach(ImportCasId csDto in csDtps)
            {
                if(!IsValid(csDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Cast cast = new Cast()
                {
                    FullName = csDto.FullName,
                    IsMainCharacter = csDto.IsMainCharacter,
                    PhoneNumber = csDto.PhoneNumber,
                    PlayId = csDto.PlayId
                };

                validCasts.Add(cast);
                var isMainActor = "";
                if(csDto.IsMainCharacter ==true)
                {
                    isMainActor = "main";
                }
                else
                {
                    isMainActor = "lesser";
                }

                sb.AppendLine(string.Format(SuccessfulImportActor, cast.FullName, isMainActor));
            }
            context.AddRange(validCasts);
            context.SaveChanges();
            return sb.ToString().TrimEnd();
        }

        public static string ImportTtheatersTickets(TheatreContext context, string jsonString)
        {
             StringBuilder sb = new StringBuilder();
            ImportTheatreDto[] thDtos = JsonConvert.DeserializeObject<ImportTheatreDto[]> (jsonString);

            ICollection<Theatre> validTheatre = new HashSet<Theatre>(); 
            ICollection<Ticket> validTicket = new HashSet<Ticket>();

            foreach(ImportTheatreDto thDto in thDtos)
            {
                if(!IsValid(thDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Theatre theatre = new Theatre()
                {
                    Name = thDto.Name,
                    NumberOfHalls = thDto.NumberOfHalls,
                    Director = thDto.Director
                };

              

                foreach(var tkDto in thDto.Tickets)
                {
                    if(!IsValid(tkDto))
                    {

                        sb.AppendLine(ErrorMessage);
                        continue;
                    }
                    Ticket ticket = new Ticket()
                    { 
                      Price = tkDto.Price,
                      RowNumber = tkDto.RowNumber,
                      PlayId = tkDto.PlayId,
                      Theatre = theatre
                    };

                    validTicket.Add(ticket);


                }
                theatre.Tickets = validTicket;
                validTheatre.Add(theatre);
                var totalTickets = theatre.Tickets.Count().ToString();
                sb.AppendLine(string.Format(SuccessfulImportTheatre, theatre.Name, totalTickets));
            }

            context.Theatres.AddRange(validTheatre);
            context.SaveChanges();
            return sb.ToString().TrimEnd();

       


    }


        private static bool IsValid(object obj)
        {
            var validator = new ValidationContext(obj);
            var validationRes = new List<ValidationResult>();

            var result = Validator.TryValidateObject(obj, validator, validationRes, true);
            return result;
        }
    }
}
