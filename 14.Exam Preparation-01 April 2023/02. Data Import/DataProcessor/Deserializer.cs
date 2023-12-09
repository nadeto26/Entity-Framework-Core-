namespace Boardgames.DataProcessor
{
    using System.ComponentModel.DataAnnotations;
    using System.Diagnostics.Metrics;
    using System.Net;
    using System.Text;
    using Boardgames.Data;
    using Boardgames.Data.Models;
    using Boardgames.Data.Models.Enums;
    using Boardgames.DataProcessor.ImportDto;
    using Boardgames.Utilities;
    using Newtonsoft.Json;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedCreator
            = "Successfully imported creator – {0} {1} with {2} boardgames.";

        private const string SuccessfullyImportedSeller
            = "Successfully imported seller - {0} with {1} boardgames.";

        private static XmlHelper xmlHelper;
        public static string ImportCreators(BoardgamesContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();
            xmlHelper = new XmlHelper();
            ImportCreatorDto[] creatorDtos = xmlHelper.Deserialize<ImportCreatorDto[]>(xmlString, "Creators");

            ICollection<Creator> vlidCreators = new HashSet<Creator>(); 

            foreach(ImportCreatorDto cDto in creatorDtos)
            {
                if(!IsValid(cDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                ICollection<Boardgame> validBoardgames = new HashSet<Boardgame>();  
                foreach(ImportBoardGameDto bBDto in cDto.Boardgames)
                {
                    if(!IsValid(bBDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    Boardgame boardgame = new Boardgame()
                    {
                        Name = bBDto.Name,
                        Rating = bBDto.Rating,
                        YearPublished = bBDto.YearPublished,
                        CategoryType = (CategoryType)bBDto.CategoryType,
                        Mechanics = bBDto.Mechanics,
                    };
                    validBoardgames.Add(boardgame);
                }

                Creator creator = new Creator()
                {
                    FirstName = cDto.FirstName,
                    LastName = cDto.LastName,
                    Boardgames = validBoardgames
                };

                vlidCreators.Add(creator);

                sb.AppendLine(string.Format(SuccessfullyImportedCreator,creator.FirstName, creator.LastName,validBoardgames.Count()));

            }

            context.AddRange(vlidCreators);
            context.SaveChanges();
            return sb.ToString();
        }

        public static string ImportSellers(BoardgamesContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();
            ImportSellerDto[] sDtos = JsonConvert.DeserializeObject<ImportSellerDto[]>(jsonString);

            ICollection<Seller> validSellers = new HashSet<Seller>();
            ICollection<int> excitingBd = context.Boardgames
                .Select(s => s.Id).ToArray();

            foreach (ImportSellerDto sDto in sDtos)
            {
                if(!IsValid(sDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Seller seller = new Seller()
                {
                    Name = sDto.Name,
                    Address = sDto.Address,
                    Country = sDto.Country,
                    Website = sDto.Website,
                    


                };
                foreach(int bdId in sDto.BoardgamesIds.Distinct())
                {
                    if(!excitingBd.Contains(bdId))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    BoardgameSeller boardgameSeller = new BoardgameSeller()
                    {
                        Seller = seller,
                        BoardgameId = bdId
                    };

                    seller.BoardgamesSellers.Add(boardgameSeller);
                }
                validSellers.Add(seller);
                sb.AppendLine(string.Format(SuccessfullyImportedSeller, seller.Name, seller.BoardgamesSellers.Count()));         
            }
            context.Sellers.AddRange(validSellers);
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
