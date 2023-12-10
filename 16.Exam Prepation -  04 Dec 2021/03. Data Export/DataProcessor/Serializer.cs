namespace Theatre.DataProcessor
{
    using Newtonsoft.Json;
    using System;
    using Theatre.Data;

    public class Serializer
    {
        public static string ExportTheatres(TheatreContext context, int numbersOfHalls)
        {
            var result = context.Theatres.ToArray().Where(x => x.NumberOfHalls >= numbersOfHalls &&
            x.Tickets.Count() >= 20)
            .Select(x => new
            {
                Name = x.Name,
                Halls = x.NumberOfHalls,
                TotalIncome = x.Tickets.Where(x => x.RowNumber <= 5).Sum(x => x.Price),
                Tickets = x.Tickets.Where(x => x.RowNumber <= 5).Select(t => new
                {
                    Price = t.Price,
                    RowNumber = t.RowNumber
                })
                .OrderByDescending(p => p.Price)
                .ToArray()
            })
            .OrderByDescending(h => h.Halls)
            .ThenBy(n => n.Name);


            return JsonConvert.SerializeObject(result, Formatting.Indented);


        }
    }








    //public static string ExportPlays(TheatreContext context, double raiting)
    //{
    //    throw new NotImplementedException();
    //}
}

   

