namespace VaporStore.DataProcessor
{
    using Data;
    using Newtonsoft.Json;
    using System.Globalization;
    using System.IO;
    using System.Text;
    using System.Xml.Linq;
    using System.Xml.Serialization;
    using VaporStore.Data.Models;
    using VaporStore.Data.Models.Enums;
    using VaporStore.DataProcessor.ExportDto;
    using VaporStore.DataProcessor.ImportDto;
    using VaporStore.Utilities;

    public static class Serializer
    {
        public static string ExportGamesByGenres(VaporStoreDbContext context, string[] genreNames)
        {
            var genreGames = context.Genres
              .Where(g => genreNames.Contains(g.Name))
              .Select(g => new
              {
                  Id = g.Id,
                  Name = g.Name,
                  Games = g.Games
                   .Where(games => games.Purchases.Any())
                   .Select(g => new
                   {
                       Id = g.Id,
                       Title = g.Name,
                       Developer = g.Developer,
                       Tags = string.Join(" ", g.GameTags)
                       .ToArray()
                       ,
                       Players = g.Purchases.Count()
                   })
                   .OrderByDescending(g => g.Players)
                   .ThenBy(g => g.Id).
                   ToArray()
                   ,
                  TotalPlayers = g.Games.Sum(ga => ga.Purchases.Count)


              })
                .OrderByDescending(g => g.TotalPlayers)
                   .ThenBy(g => g.Id).
                   ToArray();

            string json = JsonConvert.SerializeObject(genreGames, Formatting.Indented);

            return json;

        }

        static XmlHelper xmlHelper;
        public static string ExportUserPurchasesByType(VaporStoreDbContext context, string purchaseType)
        {
            StringBuilder sb = new StringBuilder();

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ExportUserDto[]), new XmlRootAttribute("Users"));
            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);

            using StringWriter stringWriter = new StringWriter(sb);

            PurchaseType purchaseTypeEnum = Enum.Parse<PurchaseType>(purchaseType);

            var users = context
                .Users
                .ToArray()
                .Where(u => u.Cards.Any(c => c.Purchases.Any()))
                .Select(u => new ExportUserDto()
                {
                    UserName  = u.Username,
                    Purchase = context
                        .Purchases
                        .ToArray()
                        .Where(p => p.Card.User.Username == u.Username && p.Type == purchaseTypeEnum)
                        .OrderBy(p => p.Date)
                        .Select(p => new ExportUserPurscheDto()
                        {
                            CardNumber = p.Card.Number,
                            Cvc = p.Card.Cvc,
                            Date = p.Date.ToString("yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture),
                            game = new ExportUserGame()
                            {
                                name = p.Game.Name,
                                Genre = p.Game.Genre.Name,
                                Price = p.Game.Price
                            }
                        })
                        .ToArray(),
                    TotalSpent = context
                        .Purchases
                        .ToArray()
                        .Where(p => p.Card.User.Username == u.Username && p.Type == purchaseTypeEnum)
                        .Sum(p => p.Game.Price)
                })
                .Where(u => u.Purchases.Length > 0)
                .OrderByDescending(u => u.TotalSpent)
                .ThenBy(u => u.Username)
                .ToArray();

            xmlSerializer.Serialize(stringWriter, users, namespaces);

            return sb.ToString().TrimEnd();
        }


    }
}



   