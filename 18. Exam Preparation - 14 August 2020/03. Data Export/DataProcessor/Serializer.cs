namespace VaporStore.DataProcessor
{ 
    using Data;
    using Newtonsoft.Json;
    using VaporStore.Data.Models;

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

        public static string ExportUserPurchasesByType(VaporStoreDbContext context, string purchaseType)
        {
            throw new NotImplementedException();
        }
    }
}