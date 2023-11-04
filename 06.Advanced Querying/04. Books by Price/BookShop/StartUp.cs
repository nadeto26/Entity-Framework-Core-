namespace BookShop
{
    using Data;
    using Initializer;

    public class StartUp
    {
        public static void Main()
        {
            using var db = new BookShopContext();
            DbInitializer.ResetDatabase(db);

            string result = GetBooksByPrice(db);
        }

        public static string GetBooksByPrice(BookShopContext context)
        {
            string[] books = context.Books
                .Where(x => x.Price > 40)
                .OrderByDescending(x => x.Price)
                .Select(a=> $"{a.Title} - ${a.Price:f2}")
                .ToArray();

            return string.Join(Environment.NewLine, books);
        }
    }
}


