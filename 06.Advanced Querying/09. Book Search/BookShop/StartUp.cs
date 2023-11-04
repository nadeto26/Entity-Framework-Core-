namespace BookShop
{
    using Data;
    using Initializer;
    using System.Globalization;

    public class StartUp
    {
        public static void Main()
        {
            using var db = new BookShopContext();
            DbInitializer.ResetDatabase(db);

            string input = Console.ReadLine();
            string result = GetBookTitlesContaining(db,input);
        }

        //Problem4
        public static string GetBooksByPrice(BookShopContext context)
        {
            string[] books = context.Books
                .Where(x => x.Price > 40)
                .OrderByDescending(x => x.Price)
                .Select(a=> $"{a.Title} - ${a.Price:f2}")
                .ToArray();

            return string.Join(Environment.NewLine, books);
        }

        //Problem5

        public static string GetBooksNotReleasedIn(BookShopContext context, int year)
        {
            string[] books = context.Books
                .Where(a => a.ReleaseDate.Value.Year != year)
                .OrderBy(a => a.BookId)
                .Select(a=>a.Title) .ToArray();

            return string.Join(Environment.NewLine, books);
        }

        //Problem7

        public static string GetBooksReleasedBefore(BookShopContext context, string date)
        {
            DateTime convertedDate = DateTime.ParseExact(date, "dd-MM-yyyy", CultureInfo.InvariantCulture);
            var books = context.Books
                .Where(b => b.ReleaseDate < convertedDate)
                .OrderByDescending(b => b.ReleaseDate)
                .Select(b => $"{b.Title} - {b.EditionType} - ${b.Price:F2}")
                .ToList();

            var result = String.Join(Environment.NewLine, books);
            return result;
        }

        //Problem9

        public static string GetBookTitlesContaining(BookShopContext context, string input)
        {
            var books = context.Books
            .Where(a=>a.Title.Contains(input, StringComparison.OrdinalIgnoreCase))
            .OrderBy(a=>a.Title)
            .Select(a=>a.Title)
            .ToArray();

            var result = String.Join(Environment.NewLine, books);
            return result;
        }
    }
}


